using UnityEngine;

[CreateAssetMenu(fileName = "LevelBuilder", menuName = "Scriptable Objects/LevelBuilder")]
public class LevelBuilder : MonoBehaviour, ILevelBuilder
{
	public LevelGenerator levelGenerator;

    public Tile[] GenerateGrid(int width, int height, GameObject gridRenderer)
    {
		Tile[] tiles = new Tile[width * height];

		for (int i = 0; i < width * height; i++)
		{
			TileData tileData = levelGenerator.tileData[Random.Range(0, levelGenerator.tileData.Length)];
			tiles[i] = new Tile(tileData, i);
		}

		return tiles;
    }
}
