using UnityEngine;
using System.Collections;

public class GridManipulator : MonoBehaviour
{
	public float upscaleTime = 0.5f;
	public float rotateTime = 0.5f;
	public float downscaleTime = 0.5f;

	public AnimationCurve upscaleCurve;
	public AnimationCurve downscaleCurve;
	public AnimationCurve animationCurve;

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
		if (tile.locked) yield break;
		tile.locked = true;
		SpriteRenderer spriteRenderer = GridRenderer.instance.GetSpriteRenderer(tileId);
		float startRotation = spriteRenderer.transform.rotation.eulerAngles.z;

		spriteRenderer.sortingOrder = 100;

		float animationTimer = 0f;
		while (animationTimer < upscaleTime)
		{
			animationTimer += Time.deltaTime;
			spriteRenderer.transform.localScale = Vector3.one + upscaleCurve.Evaluate(animationTimer / upscaleTime) * Vector3.one;
			yield return null;
		}

		animationTimer = 0f;
		while (animationTimer < rotateTime)
		{
			animationTimer += Time.deltaTime;
			spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, animationCurve.Evaluate(animationTimer / rotateTime) * rotation + startRotation);
			yield return null;
		}

		animationTimer = 0f;
		while (animationTimer < downscaleTime)
		{
			animationTimer += Time.deltaTime;
			spriteRenderer.transform.localScale = Vector3.one + downscaleCurve.Evaluate(animationTimer / downscaleTime) * Vector3.one;
			yield return null;
		}
		spriteRenderer.transform.localScale = Vector3.one;
		spriteRenderer.sortingOrder = 0;

		if (rotation > 0)
			tile.RotateClockwise();
		else
			tile.RotateCounterClockwise();
		tile.locked = false;
	}
}
