using UnityEngine;
using UnityEngine.InputSystem;

public class GridInteractionUI : MonoBehaviour
{
	public SpriteRenderer highlightRect;
	public SpriteRenderer highlightArrow;

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
	}

	public void MarkGridRow(int row, Direction direction)
	{
		if (direction == Direction.Up || direction == Direction.Down) return;

		GridRenderer renderer = GridRenderer.instance;
		Bounds bounds = renderer.GetGridBounds();
		Bounds tileBounds = renderer.GetTileBounds(0, row);

		highlightRect.transform.position = new Vector3(bounds.center.x, tileBounds.center.y, 0);
		highlightRect.transform.localScale = new Vector3(bounds.size.x, tileBounds.size.y, 1);

		SetVisible(true);
	}

	public void MarkGridCol(int col, Direction direction)
	{

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

		if (selectRow)
			MarkGridRow(tilePosition.y, Direction.Right);
		else if (selectCol)
			MarkGridCol(tilePosition.x, Direction.Down);
		else
			MarkTile(tilePosition.x, tilePosition.y);
	}

	void OnLeftClick(InputAction.CallbackContext context)
	{
		Vector2Int tilePosition = GridRenderer.instance.WorldToTilePosition(mousePosition);
		Debug.Log("Left Click On " + tilePosition);
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
