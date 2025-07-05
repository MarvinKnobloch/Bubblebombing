using UnityEngine;
using System.Collections;

public class GridManipulator : MonoBehaviour
{
	public float upscaleTime = 0.5f;
	public float rotateTime = 0.5f;
	public float downscaleTime = 0.5f;
	public float linearMoveTime = 0.5f;

	public AnimationCurve upscaleCurve;
	public AnimationCurve downscaleCurve;
	public AnimationCurve animationCurve;
	public AnimationCurve linearMoveCurve;

	private Vector3[] startPositions;
	private SpriteRenderer[] spriteRenderers;
	public SpriteRenderer tempSpriteRenderer;

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

	public void MoveRow(int row, Direction direction, TileData newTileData)
	{
		StartCoroutine(MoveRowAnimation(row, direction, newTileData));
	}

	private IEnumerator MoveRowAnimation(int row, Direction direction, TileData newTileData)
	{
		for (int i = 0; i < LevelGrid.instance.width; i++)
		{
			Tile tile = LevelGrid.instance.tiles[i + row * LevelGrid.instance.width];
			if (tile.locked) yield break;
		}

		if (spriteRenderers == null)
		{
			startPositions = new Vector3[LevelGrid.instance.width+1];
			spriteRenderers = new SpriteRenderer[LevelGrid.instance.width+1];
		}

		int j;
		float animationTimer = 0f;
		float offset;
		if (direction == Direction.Right)
		{
			j = 0;
			offset = 1;
			spriteRenderers[LevelGrid.instance.width] = tempSpriteRenderer;
			tempSpriteRenderer.transform.localPosition = new Vector3(-GridRenderer.instance.tileSize, 0, 0);
		}
		else
		{
			j = 1;
			offset = -1;
			spriteRenderers[0] = tempSpriteRenderer;
			tempSpriteRenderer.transform.localPosition = new Vector3(LevelGrid.instance.width * GridRenderer.instance.tileSize, 0, 0);
		}
		for (int i = 0; i < LevelGrid.instance.width; i++)
		{
			Tile tile = LevelGrid.instance.tiles[i + row * LevelGrid.instance.width];
			tile.locked = true;
			spriteRenderers[j] = GridRenderer.instance.GetSpriteRenderer(tile.index);
			startPositions[j] = spriteRenderers[j].transform.localPosition;
			j++;
		}

		tempSpriteRenderer.sprite = newTileData.sprite;
		tempSpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, newTileData.rotation);

		while (animationTimer < linearMoveTime)
		{
			animationTimer += Time.deltaTime;

			for (int i = 0; i <= LevelGrid.instance.width; i++)
			{
				Vector3 startPosition = startPositions[i];
				Vector3 endPosition = startPosition + new Vector3(offset, 0, 0);
				spriteRenderers[i].transform.localPosition = Vector3.Lerp(startPosition, endPosition, linearMoveCurve.Evaluate(animationTimer / linearMoveTime));
			}
			yield return null;
		}

		for (int i = 0; i < LevelGrid.instance.width; i++)
		{
			Tile tile = LevelGrid.instance.tiles[i + row * LevelGrid.instance.width];
			tile.locked = false;
			tile.paths = newTileData.paths;
			spriteRenderers[i].transform.localPosition = Vector3.zero;
		}
	}
}
