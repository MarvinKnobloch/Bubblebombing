using UnityEngine;

/// <summary>
/// A PlaceableObject is an object that can be placed on a tile.
/// </summary>
public class PlaceableObject : MonoBehaviour
{
	public bool canBeRemoved = true;
	
	[System.NonSerialized] public TileGrafik owner;

	public void UpdatePosition(Vector2Int gridPosition)
	{
		int tileId = LevelGrid.instance.GetTile(gridPosition.x, gridPosition.y).index;
		TileGrafik tileGrafik = GridRenderer.instance.GetTileGrafik(tileId);
		tileGrafik.PlaceObject(this);
	}
}
