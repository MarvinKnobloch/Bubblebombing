using UnityEngine;

public class Tile
{
	public int index;
	// Mögliche Richtungen
    public Directions paths;
	public GameObject obj;

	private TileData tileData;

	public Tile(TileData tileData, int index)
	{
		this.tileData = tileData;
		this.index = index;
		this.paths = tileData.paths;
	}

	public Sprite GetSprite()
	{
		return tileData.sprite;
	}

	public float GetSpriteRotation()
	{
		return tileData.rotation;
	}

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
		return direction switch
		{
			Direction.Up => paths.up,
			Direction.Down => paths.down,
			Direction.Left => paths.left,
			Direction.Right => paths.right,
			_ => false,
		};
	}
}
