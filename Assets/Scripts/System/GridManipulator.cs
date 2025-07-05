using UnityEngine;
using System.Collections;

public class GridManipulator : MonoBehaviour
{
	public float rotateSpeed = 1f;

	private float animationTimer;

	public void RotateTileCW(int tileId)
	{
		StartCoroutine(RotateTile(tileId, 90));
	}

	public void RotateTileCCW(int tileId)
	{
		StartCoroutine(RotateTile(tileId, -90));
	}

	private IEnumerator RotateTile(int tileId, float rotation)
	{
		Tile tile = LevelGrid.instance.tiles[tileId];
		SpriteRenderer spriteRenderer = GridRenderer.instance.GetSpriteRenderer(tileId);
		float startRotation = spriteRenderer.transform.rotation.eulerAngles.z;

		animationTimer = 0f;
		while (animationTimer < 1f)
		{
			animationTimer += Time.deltaTime * rotateSpeed;
			spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(startRotation, startRotation + rotation, animationTimer));
			yield return null;
		}

		if (rotation > 0)
			tile.RotateClockwise();
		else
			tile.RotateCounterClockwise();
	}
}
