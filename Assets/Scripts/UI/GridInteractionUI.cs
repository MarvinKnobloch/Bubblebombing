using UnityEngine;
using UnityEngine.InputSystem;

public class GridInteractionUI : MonoBehaviour
{
	public GridManipulator gridManipulator;

	public SpriteRenderer highlightRect;
	public SpriteRenderer highlightArrow;
	public float arrowOffset = 1f;

	public bool selectRow = true;
	public bool selectCol = false;

	public Controls inputActions;
	private Vector2 mousePosition;

	void Start()
	{
		SetVisible(false);
		inputActions = new Controls();
		inputActions.Enable();
		inputActions.Player.Cursor.performed += OnMousePos;
		inputActions.Player.LMB.performed += OnLeftClick;
		inputActions.Player.RMB.performed += OnRightClick;
	}

	public void MarkGridRow(int row, Direction direction)
	{
		if (direction == Direction.Up || direction == Direction.Down) return;

		GridRenderer renderer = GridRenderer.instance;
		Bounds bounds = renderer.GetGridBounds();
		Bounds tileBounds = renderer.GetTileBounds(0, row);

		highlightRect.transform.position = new Vector3(bounds.center.x, tileBounds.center.y, 0);
		highlightRect.transform.localScale = new Vector3(bounds.size.x, tileBounds.size.y, 1);

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
		highlightRect.transform.localScale = new Vector3(tileBounds.size.x, bounds.size.y, 1);

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
		highlightRect.transform.localScale = new Vector3(tileBounds.size.x, tileBounds.size.y, 1);

		SetVisible(true);
	}

	void SetVisible(bool visible)
	{
		highlightRect.gameObject.SetActive(visible);
		highlightArrow.gameObject.SetActive(visible);
	}

	void OnMousePos(InputAction.CallbackContext context)
	{
		mousePosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());

		highlightArrow.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);

		Vector2Int tilePosition = GridRenderer.instance.WorldToTilePosition(mousePosition);

		if (tilePosition.x < 0 || tilePosition.x >= LevelGrid.instance.width || tilePosition.y < 0 || tilePosition.y >= LevelGrid.instance.height)
		{
			SetVisible(false);
			return;
		}

		Vector3 gridCenter = GridRenderer.instance.GetGridBounds().center;
		if (selectRow)
			MarkGridRow(tilePosition.y, mousePosition.x < gridCenter.x ? Direction.Right : Direction.Left);
		else if (selectCol)
			MarkGridCol(tilePosition.x, mousePosition.y < gridCenter.y ? Direction.Down : Direction.Up);
		else
			MarkTile(tilePosition.x, tilePosition.y);
	}

	void OnLeftClick(InputAction.CallbackContext context)
	{
		Vector2Int tilePosition = GridRenderer.instance.WorldToTilePosition(mousePosition);
		Debug.Log("Left Click On " + tilePosition);

		Tile tile = LevelGrid.instance.GetTile(tilePosition.x, tilePosition.y);
		if (tile == null) return;

		if (selectRow)
			return;
		else if (selectCol)
			return;
		else
			gridManipulator.RotateTileCW(tile.index);
	}

	void OnRightClick(InputAction.CallbackContext context)
	{
		Vector2Int tilePosition = GridRenderer.instance.WorldToTilePosition(mousePosition);
		Tile tile = LevelGrid.instance.GetTile(tilePosition.x, tilePosition.y);
		if (tile == null) return;

		if (!selectRow && !selectCol)
			gridManipulator.RotateTileCCW(tile.index);
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
