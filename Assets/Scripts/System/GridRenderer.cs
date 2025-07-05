using UnityEngine;

public class GridRenderer : MonoBehaviour
{
	public Sprite dummySprite;
	public float tileSize = 1;

	private SpriteRenderer[] spriteRenderers;

	private void Awake()
	{
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
}