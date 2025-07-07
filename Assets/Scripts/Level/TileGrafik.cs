using System.Collections.Generic;
using UnityEngine;

public class TileGrafik : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
	public List<PlaceableObject> tileObjects = new List<PlaceableObject>();

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
		foreach (PlaceableObject tileObject in tileObjects)
		{
			if (tileObject == null) continue;
			tileObject.transform.position = position;
		}
	}

	public void SetRotation(Quaternion rotation)
	{
		spriteRenderer.transform.rotation = rotation;
		foreach (PlaceableObject tileObject in tileObjects)
		{
			if (tileObject == null) continue;
			tileObject.transform.rotation = Quaternion.identity;
		}
	}

	public void SetSprite(Sprite sprite)
	{
		spriteRenderer.sprite = sprite;
	}

	public void SetVisible(bool visible)
	{
		spriteRenderer.enabled = visible;
		foreach (PlaceableObject tileObject in tileObjects)
		{
			if (tileObject == null) continue;
			tileObject.gameObject.SetActive(visible);
		}
	}

	public void MoveObjects(TileGrafik source)
	{
		this.tileObjects = source.tileObjects;
		//this.spriteRenderer.transform.rotation = source.spriteRenderer.transform.rotation;
		source.tileObjects = new List<PlaceableObject>();
		foreach (PlaceableObject tileObject in tileObjects)
		{
			if (tileObject == null) continue;
			tileObject.owner = this;
			tileObject.transform.SetParent(transform);
			tileObject.transform.localPosition = Vector3.zero;
		}
	}

	public void PlaceObject(PlaceableObject placeableObject)
	{
		if (placeableObject == null) return;
		if (placeableObject.owner != null)
			placeableObject.owner.tileObjects.Remove(placeableObject);
		placeableObject.owner = this;
		placeableObject.transform.SetParent(transform);
		placeableObject.transform.localPosition = Vector3.zero;
		placeableObject.transform.rotation = Quaternion.identity;
		tileObjects.Add(placeableObject);

		Debug.Log("Objekt " + placeableObject.name + " plaziert auf " + transform.position);
	}

	public void RemoveObjects()
	{
		foreach (PlaceableObject placeableObject in tileObjects)
		{
			if (placeableObject != null && placeableObject.canBeRemoved)
			{
				Debug.Log("Objekt " + placeableObject.name + " entfernt von " + transform.position);
				Destroy(placeableObject.gameObject);
			}
		}
	}

	public void RemoveObject(PlaceableObject placeableObject)
	{
		tileObjects.Remove(placeableObject);
	}
}
