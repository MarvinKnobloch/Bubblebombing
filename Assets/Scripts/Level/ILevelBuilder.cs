using UnityEngine;

public interface ILevelBuilder
{
	Tile[] GenerateGrid(int width, int height, GameObject gridRenderer);
}
