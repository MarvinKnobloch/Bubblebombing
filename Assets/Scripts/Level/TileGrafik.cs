using UnityEngine;

public class TileGrafik : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
	public PlaceableObject tileObject;

	public int sortingOrder { get => spriteRenderer.sortingOrder; set => spriteRenderer.sortingOrder = value; }

	public void ApplyTile(Tile tile)
	{
		SetSprite(tile.GetSprite());
		SetRotation(Quaternion.Euler(0, 0, tile.GetSpriteRotation()));
		Vector3 pos = GridRenderer.instance.TileToWorldPosition(new Vector2Int(tile.index % LevelGrid.instance.width, tile.index / LevelGrid.instance.width));
		SetPosition(pos);
	}

	public void SetTransform(Vector3 position, Quaternion rotation)
	{
		SetPosition(position);
		SetRotation(rotation);
	}

	public void SetPosition(Vector3 position)
	{
		spriteRenderer.transform.position = position;
		if (tileObject != null)
			tileObject.transform.position = position;
	}

	public void SetRotation(Quaternion rotation)
	{
		spriteRenderer.transform.rotation = rotation;
		if (tileObject != null)
			tileObject.transform.rotation = rotation;
	}

	public void SetSprite(Sprite sprite)
	{
		spriteRenderer.sprite = sprite;
	}

	public void SetVisible(bool visible)
	{
		spriteRenderer.enabled = visible;
		if (tileObject != null)
			tileObject.gameObject.SetActive(visible);
	}

	public void PlaceObject(PlaceableObject placeableObject)
	{
		if (tileObject != null)
		{
			RemoveObject();
		}
		if (placeableObject == null) return;
		tileObject = placeableObject;
		if (tileObject.owner != null)
			tileObject.owner.tileObject = null;
		tileObject.owner = this;
		tileObject.transform.SetParent(transform);
		tileObject.transform.localPosition = Vector3.zero;

		Debug.Log("Objekt " + placeableObject.name + " plaziert auf " + transform.position);
	}

	public void RemoveObject()
	{
		if (tileObject != null)
		{
			Debug.Log("Objekt " + tileObject.name + " entfernt von " + transform.position);
			Destroy(tileObject.gameObject);
			tileObject = null;
		}
	}
}
