using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class GridInteractionUI : MonoBehaviour
{
	public static GridInteractionUI instance;
	[Header("Objekt zum plazieren")]
	public PlaceableObject placeableObjectPrefab;
	[Header("TileData für neue Tiles")]
	public TileData newTileData;

	[Space]
	public GridManipulator gridManipulator;

	public TileData insertTileData;

	public SpriteRenderer highlightRect;
	public SpriteRenderer highlightArrow;
	public Color allowedColor = Color.green;
	public Color notAllowedColor = Color.red;
	public float arrowOffset = 1f;
	
	[Header("Interaction Type")]
	public GridInteractionType interactionType = GridInteractionType.None;
	private bool moveRow = false;
	private int currentInteractionCosts;

	public Controls inputActions;
	private Vector2 mousePosition;
	private Color currentColor;

    private void Awake()
    {
        if (instance == null)
		{
            instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		currentColor = allowedColor;
    }

    void Start()
	{
		SetVisible(false);
		inputActions = new Controls();
		inputActions.Enable();
		inputActions.Player.Cursor.performed += OnMousePos;
		inputActions.Player.LMB.performed += OnLeftClick;
		inputActions.Player.RMB.performed += OnRightClick;
		inputActions.Player.Space.performed += OnToggleInteractionType;

		GameManager.Instance.playerUI.abilityUI.gridInteractionUI = this;
	}
    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void MarkGridRow(int row, Direction direction)
	{
		if (direction == Direction.Up || direction == Direction.Down) return;

		GridRenderer renderer = GridRenderer.instance;
		Bounds bounds = renderer.GetGridBounds();
		Bounds tileBounds = renderer.GetTileBounds(0, row);

		highlightRect.transform.position = new Vector3(bounds.center.x, tileBounds.center.y, 0);
		highlightRect.size = new Vector2(bounds.size.x, tileBounds.size.y);

		if (direction == Direction.Right)
		{
			highlightArrow.transform.position = new Vector3(bounds.min.x - arrowOffset, tileBounds.center.y, 0);
			highlightArrow.transform.rotation = Quaternion.Euler(0, 0, -90);
		}
		else
		{
			highlightArrow.transform.position = new Vector3(bounds.max.x + arrowOffset, tileBounds.center.y, 0);
			highlightArrow.transform.rotation = Quaternion.Euler(0, 0, 90);
		}

		SetVisible(true);
	}

	public void MarkGridCol(int col, Direction direction)
	{
		if (direction == Direction.Left || direction == Direction.Right) return;

		GridRenderer renderer = GridRenderer.instance;
		Bounds bounds = renderer.GetGridBounds();
		Bounds tileBounds = renderer.GetTileBounds(col, 0);
		
		highlightRect.transform.position = new Vector3(tileBounds.center.x, bounds.center.y, 0);
		highlightRect.size = new Vector2(tileBounds.size.x, bounds.size.y);

		if (direction == Direction.Down)
		{
			highlightArrow.transform.position = new Vector3(tileBounds.center.x, bounds.min.y - arrowOffset, 0);
			highlightArrow.transform.rotation = Quaternion.Euler(0, 0, 0);
		}
		else
		{
			highlightArrow.transform.position = new Vector3(tileBounds.center.x, bounds.max.y + arrowOffset, 0);
			highlightArrow.transform.rotation = Quaternion.Euler(0, 0, 180);
		}

		SetVisible(true);
	}

	public void MarkTile(int x, int y)
	{
		GridRenderer renderer = GridRenderer.instance;
		Bounds tileBounds = renderer.GetTileBounds(x, y);

		highlightRect.transform.position = new Vector3(tileBounds.center.x, tileBounds.center.y, 0);
		highlightRect.size = new Vector2(tileBounds.size.x, tileBounds.size.y);
		highlightRect.color = currentColor;

		highlightArrow.transform.position = new Vector3(tileBounds.center.x, tileBounds.center.y + 0.1f, 0);
		highlightArrow.transform.rotation = Quaternion.Euler(0, 0, 180);
		highlightArrow.color = currentColor;

		SetVisible(true);
	}

	void SetVisible(bool visible)
	{
		highlightRect.gameObject.SetActive(visible);
		highlightArrow.gameObject.SetActive(visible && interactionType != GridInteractionType.RotateTile);
		highlightRect.color = currentColor;
		highlightArrow.color = currentColor;
	}

	void OnMousePos(InputAction.CallbackContext context)
	{
		mousePosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
		UpdateTool();
	}

	public void UpdateTool()
	{
        Vector2Int tilePosition = GridRenderer.instance.WorldToTilePosition(mousePosition);
		currentColor = allowedColor;

		if (interactionType == GridInteractionType.None)
		{
            SetVisible(false);
        }
        if (interactionType == GridInteractionType.RotateTile)
        {
            if (!IsValidTilePosition(tilePosition.x, tilePosition.y))
            {
                SetVisible(false);
                return;
            }

			if (!IsTileAllowedToMove(tilePosition.x, tilePosition.y))
			{
				currentColor = notAllowedColor;
			}

            MarkTile(tilePosition.x, tilePosition.y);
            return;
        }

        if (interactionType == GridInteractionType.MoveLine)
        {
            if (!IsValidGroupPosition(tilePosition.x, tilePosition.y))
            {
                SetVisible(false);
                return;
            }

            Vector3 gridCenter = GridRenderer.instance.GetGridBounds().center;

            // Berechne die Entfernung von der Mitte in beiden Richtungen
            float distanceX = Mathf.Abs(mousePosition.x - gridCenter.x);
            float distanceY = Mathf.Abs(mousePosition.y - gridCenter.y);

            // Wähle Zeile oder Spalte basierend auf der größeren Entfernung
            if (distanceX > distanceY)
            {
				for (int i = 0; i < LevelGrid.instance.width; i++)
				{
					if (!IsTileAllowedToMove(i, tilePosition.y))
					{
						currentColor = notAllowedColor;
					}
				}

                // Horizontal weiter entfernt -> Zeile auswählen
                MarkGridRow(tilePosition.y, mousePosition.x < gridCenter.x ? Direction.Right : Direction.Left);
                moveRow = true;
            }
            else
            {
				for (int i = 0; i < LevelGrid.instance.height; i++)
				{
					if (!IsTileAllowedToMove(tilePosition.x, i))
					{
						currentColor = notAllowedColor;
					}
				}

                // Vertikal weiter entfernt -> Spalte auswählen
                MarkGridCol(tilePosition.x, mousePosition.y < gridCenter.y ? Direction.Down : Direction.Up);
                moveRow = false;
            }
        }

        if (interactionType == GridInteractionType.PlaceObject)
        {
            MarkTile(tilePosition.x, tilePosition.y);
        }
    }


	bool IsValidTilePosition(int x, int y)
	{
		if (x < 0 || x >= LevelGrid.instance.width || y < 0 || y >= LevelGrid.instance.height)
			return false;

		return true;
	}

	bool IsValidGroupPosition(int x, int y)
	{
		if (x < -1 || x > LevelGrid.instance.width || y < -1 || y > LevelGrid.instance.height)
			return false;

		if ((x == -1 || x == LevelGrid.instance.width) && (y == -1 || y == LevelGrid.instance.height))
			return false;

		return true;
	}

	void OnLeftClick(InputAction.CallbackContext context)
	{
		if (interactionType == GridInteractionType.None) return;

        Vector2Int tilePosition = GridRenderer.instance.WorldToTilePosition(mousePosition);
		//Debug.Log("Left Click On " + tilePosition);

		Tile tile = LevelGrid.instance.GetTile(tilePosition.x, tilePosition.y);
		if (tile == null) return;

        if (TurnController.instance.CheckForActionPoints(currentInteractionCosts) == false)
        {
            GetComponent<GridManipulator>().cantPlaceAudio.Play(GetComponent<AudioSource>());
            return;
        }

        if (interactionType == GridInteractionType.RotateTile)
		{
			if (!IsTileAllowedToMove(tilePosition.x, tilePosition.y)) return;
			gridManipulator.RotateTileCW(tile.index);
            TurnController.instance.ActionPointsUpdate(currentInteractionCosts);
        }

		if (interactionType == GridInteractionType.MoveLine)
		{
			if (moveRow)
			{
				for (int i = 0; i < LevelGrid.instance.width; i++)
				{
					if (!IsTileAllowedToMove(i, tilePosition.y)) return;
				}
				gridManipulator.MoveRow(tilePosition.y, mousePosition.x < GridRenderer.instance.GetGridBounds().center.x ? Direction.Right : Direction.Left);
			}
			else
			{
				for (int i = 0; i < LevelGrid.instance.height; i++)
				{
					if (!IsTileAllowedToMove(tilePosition.x, i)) return;
				}
				gridManipulator.MoveColumn(tilePosition.x, mousePosition.y < GridRenderer.instance.GetGridBounds().center.y ? Direction.Up : Direction.Down);
			}
            TurnController.instance.ActionPointsUpdate(currentInteractionCosts);
        }

		if (interactionType == GridInteractionType.PlaceObject)
		{
			gridManipulator.PlaceObject(tile.index, placeableObjectPrefab);
			TurnController.instance.ActionPointsUpdate(currentInteractionCosts);
        }

		if (interactionType == GridInteractionType.RemoveObject)
		{
			gridManipulator.RemoveObject(tile.index);
		}
	}

	void OnRightClick(InputAction.CallbackContext context)
	{
        if (interactionType == GridInteractionType.None) return;


        Vector2Int tilePosition = GridRenderer.instance.WorldToTilePosition(mousePosition);
		Tile tile = LevelGrid.instance.GetTile(tilePosition.x, tilePosition.y);
		if (tile == null) return;

        if (TurnController.instance.CheckForActionPoints(currentInteractionCosts) == false)
        {
            GetComponent<GridManipulator>().cantPlaceAudio.Play(GetComponent<AudioSource>());
            return;
        }

        if (interactionType == GridInteractionType.RotateTile)
		{
			gridManipulator.RotateTileCCW(tile.index);
            TurnController.instance.ActionPointsUpdate(currentInteractionCosts);
        }
	}

	void OnToggleInteractionType(InputAction.CallbackContext context)
	{
		interactionType = interactionType == GridInteractionType.RotateTile ? GridInteractionType.None : GridInteractionType.RotateTile; 
    }

	public void SetInteractionType(GridInteractionType type, int actionCost, PlaceableObject placementPrefab = null)
    {
        interactionType = type;
		currentInteractionCosts = actionCost;
		placeableObjectPrefab = placementPrefab;

		UpdateTool();
    }

	public void EntityRotate(Vector2Int pos)
	{
        Tile tile = LevelGrid.instance.GetTile(pos.x, pos.y);
        gridManipulator.RotateTileCW(tile.index);
    }

	public bool IsTileAllowedToMove(int x, int y)
	{
		if (x == 0 && y == 0) return false;
		if (x == LevelGrid.instance.width - 1 && y == LevelGrid.instance.height - 1) return false;
		if (GridRenderer.instance.IsTileBlocked(x, y)) return false;
		return true;
	}

	/*void LateUpdate()
	{
		if (selectRow)
		{
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2Int tilePosition = GridRenderer.instance.WorldToTilePosition(mousePosition);

			MarkTile(tilePosition.x, tilePosition.y);
		}
	}*/
}
