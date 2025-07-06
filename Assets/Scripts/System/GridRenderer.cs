using UnityEngine;

public class GridRenderer : MonoBehaviour
{
	/// <summary>
	/// Singleton
	/// </summary>
	public static GridRenderer instance;

	public TileGrafik graficPrefab;
	public float tileSize = 1;
	private Vector2 halfTile;

	private TileGrafik[] spriteRenderers;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		halfTile = new Vector2(tileSize * 0.5f, tileSize * 0.5f);
		spriteRenderers = new TileGrafik[LevelGrid.instance.tiles.Length];

		for (int i = 0; i < spriteRenderers.Length; i++)
		{
			TileGrafik grafic = Instantiate(graficPrefab, transform);
			grafic.name = "Tile " + i;
			grafic.ApplyTile(LevelGrid.instance.tiles[i]);
			spriteRenderers[i] = grafic;
		}
		UpdateTiles();
	}

	public TileGrafik GetTileGrafik(int tileId)
	{
		return spriteRenderers[tileId];
	}

	public void UpdateTiles()
	{
		for (int i = 0; i < LevelGrid.instance.tiles.Length; i++)
		{
			Tile tile = LevelGrid.instance.tiles[i];
			spriteRenderers[i].ApplyTile(tile);
		}
	}

	/// <summary>
	/// Wandelt World-Koordinaten in Tile-Koordinaten um
	/// </summary>
	/// <param name="worldPosition">World-Position</param>
	/// <returns>Tile-Koordinaten (x, y)</returns>
	public Vector2Int WorldToTilePosition(Vector3 worldPosition)
	{
		// Konvertiere World-Position relativ zur Grid-Position
		Vector3 localPosition = worldPosition - transform.position;
		
		// Teile durch Tile-Größe und runde ab
		int x = Mathf.RoundToInt(localPosition.x / tileSize);
		int y = Mathf.RoundToInt(localPosition.y / tileSize);
		
		return new Vector2Int(x, y);
	}

	/// <summary>
	/// Wandelt Tile-Koordinaten in World-Koordinaten um
	/// </summary>
	/// <param name="tilePosition">Tile-Koordinaten (x, y)</param>
	/// <returns>World-Position (Zentrum der Tile)</returns>
	public Vector3 TileToWorldPosition(Vector2Int tilePosition)
	{
		// Berechne die lokale Position der Tile (Zentrum)
		Vector3 localPosition = new Vector3(
			tilePosition.x * tileSize,
			tilePosition.y * tileSize,
			0
		);
		
		// Konvertiere zu World-Position
		return transform.position + localPosition;
	}

	/// <summary>
	/// Überladung für separate x, y Parameter
	/// </summary>
	public Vector3 TileToWorldPosition(int x, int y)
	{
		return TileToWorldPosition(new Vector2Int(x, y));
	}

	/// <summary>
	/// Gibt die Größe eines einzelnen Tiles zurück
	/// </summary>
	/// <returns>Tile-Größe als Vector2</returns>
	public Vector2 GetTileSize()
	{
		return new Vector2(tileSize, tileSize);
	}

	/// <summary>
	/// Gibt die Bounds eines spezifischen Tiles zurück
	/// </summary>
	/// <param name="x">Tile X-Koordinate</param>
	/// <param name="y">Tile Y-Koordinate</param>
	/// <returns>Bounds des Tiles in World-Koordinaten</returns>
	public Bounds GetTileBounds(int x, int y)
	{
		Vector3 center = TileToWorldPosition(x, y);
		Vector3 size = new Vector3(tileSize, tileSize, 0);
		return new Bounds(center, size);
	}

	/// <summary>
	/// Gibt die Bounds eines spezifischen Tiles zurück
	/// </summary>
	/// <param name="tilePosition">Tile-Koordinaten</param>
	/// <returns>Bounds des Tiles in World-Koordinaten</returns>
	public Bounds GetTileBounds(Vector2Int tilePosition)
	{
		return GetTileBounds(tilePosition.x, tilePosition.y);
	}

	/// <summary>
	/// Gibt die Gesamtgröße des Grids in World-Koordinaten zurück
	/// </summary>
	/// <returns>Grid-Größe als Vector2</returns>
	public Vector2 GetGridSize()
	{
		return new Vector2(
			LevelGrid.instance.width * tileSize,
			LevelGrid.instance.height * tileSize
		);
	}

	/// <summary>
	/// Gibt die Bounds des gesamten Grids zurück
	/// </summary>
	/// <returns>Bounds des gesamten Grids in World-Koordinaten</returns>
	public Bounds GetGridBounds()
	{
		Vector2 gridSize = GetGridSize();
		Vector3 center = transform.position + new Vector3(gridSize.x * 0.5f, gridSize.y * 0.5f, 0) - new Vector3(halfTile.x, halfTile.y, 0);
		Vector3 size = new Vector3(gridSize.x, gridSize.y, 0);
		return new Bounds(center, size);
	}

	/// <summary>
	/// Gibt die Anzahl der Tiles in jeder Dimension zurück
	/// </summary>
	/// <returns>Grid-Dimensionen als Vector2Int (width, height)</returns>
	public Vector2Int GetGridDimensions()
	{
		return new Vector2Int(LevelGrid.instance.width, LevelGrid.instance.height);
	}
}