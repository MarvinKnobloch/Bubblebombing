using UnityEngine;

public class Grid : MonoBehaviour
{
	/// <summary>
	///  Singleton
	/// </summary>
	public static Grid instance;

	public int width;
	public int height;

	public Tile[] tiles;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		tiles = new Tile[width * height];
	}

	public Tile GetTile(int x, int y)
	{
		return tiles[x + y * width];
	}

	public void SetTile(int x, int y, Tile tile)
	{
		tiles[x + y * width] = tile;
	}

	public Tile GetNeighborTile(int x, int y, Direction direction)
	{
		int neighborX = x, neighborY = y;
		
		switch (direction)
		{
			case Direction.Up:
				neighborY = y - 1;
				break;
			case Direction.Down:
				neighborY = y + 1;
				break;
			case Direction.Left:
				neighborX = x - 1;
				break;
			case Direction.Right:
				neighborX = x + 1;
				break;
		}
		
		// Bounds-Check
		if (neighborX < 0 || neighborX >= width || neighborY < 0 || neighborY >= height)
			return null;
		
		return GetTile(neighborX, neighborY);
	}

	public static Direction GetOppositeDirection(Direction direction)
	{
		switch (direction)
		{
			case Direction.Up:
				return Direction.Down;
			case Direction.Down:
				return Direction.Up;
			case Direction.Left:
				return Direction.Right;
			case Direction.Right:
				return Direction.Left;
			default:
				return Direction.Up;
		}
	}

	public bool IsDirectionFree(int x, int y, Direction direction)
	{
		Tile tile = GetTile(x, y);
		
		// Prüfe zuerst, ob die aktuelle Tile in die gewünschte Richtung frei ist
		if (!tile.IsDirectionFree(direction))
			return false;
		
		// Hole die Nachbartile
		Tile neighborTile = GetNeighborTile(x, y, direction);
		
		// Wenn keine Nachbartile existiert (außerhalb der Grenzen), ist die Richtung nicht frei
		if (neighborTile == null)
			return false;
		
		// Bestimme die gegenüberliegende Richtung
		Direction oppositeDirection = GetOppositeDirection(direction);
		
		// Prüfe, ob die Nachbartile in die gegenüberliegende Richtung frei ist
		return neighborTile.IsDirectionFree(oppositeDirection);
	}
}
