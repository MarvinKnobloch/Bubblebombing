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

	public AudioData moveAudio;
	public AudioData rotateAudio;
	private AudioSource audioSource;

	public TileGrafik tempGrafik;

	private Vector3[] startPositions;
	private TileGrafik[] tileGrafics;
	
	private Vector3 tempStartPosition;
	private bool moveingLine = false;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void RotateTileCW(int tileId)
	{
		rotateAudio.Play(audioSource);
		StartCoroutine(RotateTile(tileId, 90));
	}

	public void RotateTileCCW(int tileId)
	{
		rotateAudio.Play(audioSource);
		StartCoroutine(RotateTile(tileId, -90));
	}

	private IEnumerator RotateTile(int tileId, float rotation)
	{
		Tile tile = LevelGrid.instance.tiles[tileId];
		if (tile.locked) yield break;
		tile.locked = true;
		TileGrafik spriteRenderer = GridRenderer.instance.GetTileGrafik(tileId);
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
		if (moveingLine) return;
		moveingLine = true;
		moveAudio.Play(audioSource);
		StartCoroutine(MoveRowAnimation(row, direction, newTileData));
	}

	public void MoveColumn(int column, Direction direction, TileData newTileData)
	{
		if (moveingLine) return;
		moveingLine = true;
		moveAudio.Play(audioSource);
		StartCoroutine(MoveColumnAnimation(column, direction, newTileData));
	}

	public void PlaceObject(int tileId, PlaceableObject placeableObject)
	{
		Tile tile = LevelGrid.instance.tiles[tileId];
		if (tile.locked) return;
		tile.locked = true;
		TileGrafik grafic = GridRenderer.instance.GetTileGrafik(tileId);
		if (grafic.tileObject == null)
			grafic.PlaceObject(Instantiate(placeableObject, grafic.transform));
		tile.locked = false;
	}

	public void RemoveObject(int tileId)
	{
		Tile tile = LevelGrid.instance.tiles[tileId];
		if (tile.locked) return;
		tile.locked = true;
		TileGrafik grafic = GridRenderer.instance.GetTileGrafik(tileId);
		Destroy(grafic.tileObject.gameObject);
		grafic.tileObject = null;
		tile.locked = false;
	}

	private IEnumerator MoveRowAnimation(int row, Direction direction, TileData newTileData)
	{
		for (int i = 0; i < LevelGrid.instance.width; i++)
		{
			Tile tile = LevelGrid.instance.tiles[i + row * LevelGrid.instance.width];
			if (tile.locked) yield break;
		}

		if (tileGrafics == null || tileGrafics.Length != LevelGrid.instance.width)
		{
			startPositions = new Vector3[LevelGrid.instance.width];
			tileGrafics = new TileGrafik[LevelGrid.instance.width];
		}

		float offset;
		if (direction == Direction.Right)
		{
			offset = 1;
			tempStartPosition = GridRenderer.instance.TileToWorldPosition(new Vector2Int(-1, row));
		}
		else
		{
			offset = -1;
			tempStartPosition = GridRenderer.instance.TileToWorldPosition(new Vector2Int(LevelGrid.instance.width, row));
		}
		tempGrafik.SetSprite(newTileData.sprite);
		tempGrafik.SetRotation(Quaternion.Euler(0, 0, newTileData.rotation));
		tempGrafik.SetVisible(true);

		for (int i = 0; i < LevelGrid.instance.width; i++)
		{
			Tile tile = LevelGrid.instance.tiles[i + row * LevelGrid.instance.width];
			tile.locked = true;
			tileGrafics[i] = GridRenderer.instance.GetTileGrafik(tile.index);
			startPositions[i] = tileGrafics[i].transform.localPosition;
		}

		Vector2 deltaPosition = new Vector2(offset, 0) * GridRenderer.instance.tileSize;
		yield return StartCoroutine(MoveLineAnimation(deltaPosition));

		int index;
		if (direction == Direction.Right)
		{
			for (int i = LevelGrid.instance.width - 1; i > 0; i--)
			{
				index = row * LevelGrid.instance.width + i;
				Tile tile = LevelGrid.instance.tiles[index] = LevelGrid.instance.tiles[index - 1];
				tileGrafics[i].PlaceObject(tileGrafics[i - 1].tileObject);
				tile.index = index;
				tile.locked = false;
			}
			index = row * LevelGrid.instance.width;
			LevelGrid.instance.tiles[index] = new Tile(newTileData, index);
		}
		else
		{
			for (int i = 0; i < LevelGrid.instance.width - 1; i++)
			{
				index = row * LevelGrid.instance.width + i;
				Tile tile = LevelGrid.instance.tiles[index] = LevelGrid.instance.tiles[index + 1];
				tileGrafics[i].PlaceObject(tileGrafics[i + 1].tileObject);
				tile.index = index;
				tile.locked = false;
			}
			index = row * LevelGrid.instance.width + LevelGrid.instance.width - 1;
			LevelGrid.instance.tiles[index] = new Tile(newTileData, index);
		}

		GridRenderer.instance.UpdateTiles();
		tempGrafik.SetVisible(false);
		moveingLine = false;
	}

	private IEnumerator MoveColumnAnimation(int column, Direction direction, TileData newTileData)
	{
		for (int i = 0; i < LevelGrid.instance.height; i++)
		{
			Tile tile = LevelGrid.instance.tiles[column + i * LevelGrid.instance.width];
			if (tile.locked) yield break;
		}

		if (tileGrafics == null || tileGrafics.Length != LevelGrid.instance.height)
		{
			startPositions = new Vector3[LevelGrid.instance.height];
			tileGrafics = new TileGrafik[LevelGrid.instance.height];
		}

		float offset;
		if (direction == Direction.Up)
		{
			offset = 1;
			tempStartPosition = GridRenderer.instance.TileToWorldPosition(new Vector2Int(column, -1));
		}
		else
		{
			offset = -1;
			tempStartPosition = GridRenderer.instance.TileToWorldPosition(new Vector2Int(column, LevelGrid.instance.height));
		}
		tempGrafik.SetSprite(newTileData.sprite);
		tempGrafik.SetRotation(Quaternion.Euler(0, 0, newTileData.rotation));
		tempGrafik.SetVisible(true);

		for (int i = 0; i < LevelGrid.instance.height; i++)
		{
			Tile tile = LevelGrid.instance.tiles[column + i * LevelGrid.instance.width];
			tile.locked = true;
			tileGrafics[i] = GridRenderer.instance.GetTileGrafik(tile.index);
			startPositions[i] = tileGrafics[i].transform.localPosition;
		}

		Vector2 deltaPosition = new Vector2(0, offset) * GridRenderer.instance.tileSize;
		yield return StartCoroutine(MoveLineAnimation(deltaPosition));

		int index;
		if (direction == Direction.Up)
		{
			for (int i = LevelGrid.instance.height - 1; i > 0; i--)
			{
				index = column + i * LevelGrid.instance.width;
				Tile tile = LevelGrid.instance.tiles[index] = LevelGrid.instance.tiles[index - LevelGrid.instance.width];
				tileGrafics[i].PlaceObject(tileGrafics[i - 1].tileObject);
				tile.index = index;
				tile.locked = false;
			}
			index = column;
			LevelGrid.instance.tiles[index] = new Tile(newTileData, index);
		}
		else
		{
			for (int i = 0; i < LevelGrid.instance.height - 1; i++)
			{
				index = column + i * LevelGrid.instance.width;
				Tile tile = LevelGrid.instance.tiles[index] = LevelGrid.instance.tiles[index + LevelGrid.instance.width];
				tileGrafics[i].PlaceObject(tileGrafics[i + 1].tileObject);
				tile.index = index;
				tile.locked = false;
			}
			index = column + (LevelGrid.instance.height - 1) * LevelGrid.instance.width;
			LevelGrid.instance.tiles[index] = new Tile(newTileData, index);
		}

		GridRenderer.instance.UpdateTiles();
		tempGrafik.SetVisible(false);
		moveingLine = false;
	}

	private IEnumerator MoveLineAnimation(Vector2 deltaPosition)
	{
		float animationTimer = 0f;
		while (animationTimer < linearMoveTime)
		{
			animationTimer += Time.deltaTime;
			for (int i = 0; i < tileGrafics.Length; i++)
			{
				Vector3 startPosition = startPositions[i];
				Vector3 endPosition = startPosition + new Vector3(deltaPosition.x, deltaPosition.y, 0);
				tileGrafics[i].transform.localPosition = Vector3.Lerp(startPosition, endPosition, linearMoveCurve.Evaluate(animationTimer / linearMoveTime));
			}
			Vector3 tempEndPosition = tempStartPosition + new Vector3(deltaPosition.x, deltaPosition.y, 0);
			tempGrafik.SetPosition(Vector3.Lerp(tempStartPosition, tempEndPosition, linearMoveCurve.Evaluate(animationTimer / linearMoveTime)));
			yield return null;
		}
	}
}
