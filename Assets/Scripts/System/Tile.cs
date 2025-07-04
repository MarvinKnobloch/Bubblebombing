using UnityEngine;

public class Tile
{
	public int index;
	// Mögliche Richtungen
    public Directions paths;
	public GameObject obj;

	public void RotateClockwise()
	{
		Directions temp = paths;
		paths.up = temp.right;
		paths.right = temp.down;
		paths.down = temp.left;
		paths.left = temp.up;
	}

	public void RotateCounterClockwise()
	{
		Directions temp = paths;
		paths.up = temp.left;
		paths.left = temp.down;
		paths.down = temp.right;
		paths.right = temp.up;
	}

	public bool IsDirectionFree(Direction direction)
	{
		switch (direction)
		{
			case Direction.Up:
				return paths.up;
			case Direction.Down:
				return paths.down;
			case Direction.Left:
				return paths.left;
			case Direction.Right:
				return paths.right;
			default:
				return false;
		}
	}
}
