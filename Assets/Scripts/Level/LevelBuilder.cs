using UnityEngine;

[System.Serializable]
public struct TileFrequency
{
	[Header("Tile Frequencies")]
	[Range(0f, 100f)] public float curveWeight;
	[Range(0f, 100f)] public float straightWeight;
	[Range(0f, 100f)] public float tCrossWeight;
	[Range(0f, 100f)] public float endWeight;
	[Range(0f, 100f)] public float crossWeight;
}

public class LevelBuilder : MonoBehaviour, ILevelBuilder
{
	public LevelGenerator levelGenerator;
	public TileFrequency tileFrequency = new TileFrequency
	{
		curveWeight = 30f,
		straightWeight = 25f,
		tCrossWeight = 20f,
		endWeight = 15f,
		crossWeight = 10f
	};

    public Tile[] GenerateGrid(int width, int height, GameObject gridRenderer)
    {
		Tile[] tiles = new Tile[width * height];

		for (int i = 0; i < width * height; i++)
		{
			TileData tileData = GetRandomTileDataByWeight();
			tiles[i] = new Tile(tileData, i);
		}
		// Force Corners
		SetTile(tiles, 0, levelGenerator.tileData[1]);
		SetTile(tiles, width - 1, levelGenerator.tileData[2]);
		SetTile(tiles, width * (height - 1), levelGenerator.tileData[0]);
		SetTile(tiles, width * height - 1, levelGenerator.tileData[3]);

		return tiles;
    }

	private void SetTile(Tile[] tiles, int index, TileData tileData)
	{
		tiles[index] = new Tile(tileData, index);
	}

	private TileData GetRandomTileDataByWeight()
	{
		// Berechne die Gesamtgewichtung
		float totalWeight = tileFrequency.curveWeight + tileFrequency.straightWeight + 
		                   tileFrequency.tCrossWeight + tileFrequency.endWeight + tileFrequency.crossWeight;
		
		if (totalWeight <= 0f)
		{
			// Fallback: Zufällige Auswahl wenn keine Gewichtung vorhanden
			return levelGenerator.tileData[Random.Range(0, levelGenerator.tileData.Length)];
		}

		// Zufällige Zahl zwischen 0 und totalWeight
		float randomValue = Random.Range(0f, totalWeight);
		
		// Bestimme welcher Tile-Typ basierend auf Gewichtung
		TileType selectedType = GetTileTypeFromWeight(randomValue);
		
		// Hole ein zufälliges Tile des ausgewählten Typs
		return GetRandomTileOfType(selectedType);
	}

	private TileType GetTileTypeFromWeight(float randomValue)
	{
		float currentWeight = 0f;
		
		currentWeight += tileFrequency.curveWeight;
		if (randomValue <= currentWeight) return TileType.Curve;
		
		currentWeight += tileFrequency.straightWeight;
		if (randomValue <= currentWeight) return TileType.Straight;
		
		currentWeight += tileFrequency.tCrossWeight;
		if (randomValue <= currentWeight) return TileType.TCross;
		
		currentWeight += tileFrequency.endWeight;
		if (randomValue <= currentWeight) return TileType.End;
		
		return TileType.Cross;
	}

	private TileData GetRandomTileOfType(TileType tileType)
	{
		switch (tileType)
		{
			case TileType.Curve:
				// Kurven: Index 0-7 (8 Permutationen)
				return levelGenerator.tileData[Random.Range(0, 4)];
			
			case TileType.Straight:
				// Geraden: Index 8-9 (2 Permutationen)
				return levelGenerator.tileData[Random.Range(4, 6)];
			
			case TileType.TCross:
				// T-Cross: Index 10-13 (4 Permutationen)
				return levelGenerator.tileData[Random.Range(6, 10)];
			
			case TileType.End:
				// Endstücke: Index 14-17 (4 Permutationen)
				return levelGenerator.tileData[Random.Range(10, 14)];
			
			case TileType.Cross:
				// Kreuzung: Index 18 (1 Permutation)
				return levelGenerator.tileData[14];
			
			default:
				// Fallback: Zufällige Auswahl
				return levelGenerator.tileData[Random.Range(0, levelGenerator.tileData.Length)];
		}
	}
}

public enum TileType
{
	Curve,
	Straight,
	TCross,
	End,
	Cross
}
