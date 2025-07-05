using UnityEngine;

public class GridRenderer : MonoBehaviour
{
	/// <summary>
	/// Singleton
	/// </summary>
	public static GridRenderer instance;

	public Sprite dummySprite;
	public float tileSize = 1;

	private SpriteRenderer[] spriteRenderers;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		spriteRenderers = new SpriteRenderer[LevelGrid.instance.tiles.Length];

		for (int i = 0; i < spriteRenderers.Length; i++)
		{
			spriteRenderers[i] = new GameObject("Tile " + i).AddComponent<SpriteRenderer>();
			spriteRenderers[i].transform.SetParent(transform);
			spriteRenderers[i].transform.localPosition = new Vector3(i % LevelGrid.instance.width * tileSize, i / LevelGrid.instance.width * tileSize, 0);
		}
		UpdateTiles();
	}

	private void UpdateTiles()
	{
		for (int i = 0; i < LevelGrid.instance.tiles.Length; i++)
		{
			Tile tile = LevelGrid.instance.tiles[i];
			Sprite sprite = tile.GetSprite();
			spriteRenderers[i].sprite = sprite;
			spriteRenderers[i].transform.localRotation = Quaternion.Euler(0, 0, tile.GetSpriteRotation());
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
		int x = Mathf.FloorToInt(localPosition.x / tileSize);
		int y = Mathf.FloorToInt(localPosition.y / tileSize);
		
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
			tilePosition.x * tileSize + tileSize * 0.5f,
			tilePosition.y * tileSize + tileSize * 0.5f,
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
}