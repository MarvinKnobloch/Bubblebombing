using UnityEngine;

/// <summary>
///  Generiert die Tiles in allen Ausprägungen
/// </summary>
public class LevelGenerator : MonoBehaviour
{
	public Sprite curveSprite;
	public Sprite straightSprite;
	public Sprite tCrossSprite;
	public Sprite endSprite;
	public Sprite crossSprite;

	// Liste aller möglichen Plättchen
	public TileData[] tileData;

	[ContextMenu("Generate Tiles")]
    void GenerateTiles()
    {
        // Lösche vorhandene Tiles
        tileData = new TileData[0];
        
        // Kurve von unten nach rechts - alle 8 Permutationen
        GenerateCurvePermutations();
        
        // Gerade - 2 Permutationen
        GenerateStraightPermutations();
        
        // T-Cross - 4 Permutationen
        GenerateTCrossPermutations();
        
        // Endstück - 4 Permutationen
        GenerateEndPermutations();
        
        // Kreuzung - 1 Permutation
        GenerateCrossPermutation();
    }
    
    void GenerateCurvePermutations()
    {
        // Basis-Kurve: von unten nach rechts
        Directions baseCurve = new Directions
        {
            up = false,
            down = true,
            left = false,
            right = true
        };
        
        // 4 Rotationen
        for (int rotation = 0; rotation < 4; rotation++)
        {
            TileData tileData = new TileData
            {
                sprite = curveSprite,
                rotation = rotation * 90f,
                paths = RotateDirections(baseCurve, rotation)
            };
            
            AddTileData(tileData);
        }
        
        // 4 gespiegelte Versionen (horizontal gespiegelt, dann rotiert)
        Directions mirroredCurve = MirrorDirectionsHorizontal(baseCurve);
        
        for (int rotation = 0; rotation < 4; rotation++)
        {
            TileData tileData = new TileData
            {
                sprite = curveSprite,
                rotation = rotation * 90f,
                paths = RotateDirections(mirroredCurve, rotation)
            };
            
            AddTileData(tileData);
        }
        
        Debug.Log($"Generierte {tileData.Length} Kurven-Permutationen");
    }
    
    void GenerateStraightPermutations()
    {
        // Basis-Gerade: von unten nach oben (vertikal)
        Directions baseStraight = new Directions
        {
            up = true,
            down = true,
            left = false,
            right = false
        };
        
        // Vertikale Gerade
        TileData verticalStraight = new TileData
        {
            sprite = straightSprite,
            rotation = 0f,
            paths = baseStraight
        };
        AddTileData(verticalStraight);
        
        // Horizontale Gerade (um 90° rotiert)
        TileData horizontalStraight = new TileData
        {
            sprite = straightSprite,
            rotation = 90f,
            paths = RotateDirections(baseStraight, 1)
        };
        AddTileData(horizontalStraight);
        
        Debug.Log($"Generierte 2 Geraden-Permutationen, insgesamt {tileData.Length} Tiles");
    }
    
    void GenerateTCrossPermutations()
    {
        // Basis-T-Cross: unten, links und rechts (wie ein T)
        Directions baseTCross = new Directions
        {
            up = false,
            down = true,
            left = true,
            right = true
        };
        
        // 4 Rotationen
        for (int rotation = 0; rotation < 4; rotation++)
        {
            TileData tileData = new TileData
            {
                sprite = tCrossSprite,
                rotation = rotation * 90f,
                paths = RotateDirections(baseTCross, rotation)
            };
            
            AddTileData(tileData);
        }
        
        Debug.Log($"Generierte 4 T-Cross-Permutationen, insgesamt {tileData.Length} Tiles");
    }
    
    void GenerateEndPermutations()
    {
        // Basis-Endstück: nach unten gerichtet
        Directions baseEnd = new Directions
        {
            up = false,
            down = true,
            left = false,
            right = false
        };
        
        // 4 Rotationen
        for (int rotation = 0; rotation < 4; rotation++)
        {
            TileData tileData = new TileData
            {
                sprite = endSprite,
                rotation = rotation * 90f,
                paths = RotateDirections(baseEnd, rotation)
            };
            
            AddTileData(tileData);
        }
        
        Debug.Log($"Generierte 4 Endstück-Permutationen, insgesamt {tileData.Length} Tiles");
    }
    
    void GenerateCrossPermutation()
    {
        // Kreuzung: alle 4 Richtungen frei
        Directions crossDirections = new Directions
        {
            up = true,
            down = true,
            left = true,
            right = true
        };
        
        TileData crossTile = new TileData
        {
            sprite = crossSprite,
            rotation = 0f,
            paths = crossDirections
        };
        
        AddTileData(crossTile);
        
        Debug.Log($"Generierte 1 Kreuzung-Permutation, insgesamt {tileData.Length} Tiles");
    }
    
    Directions RotateDirections(Directions original, int rotations)
    {
        Directions result = original;
        
        for (int i = 0; i < rotations; i++)
        {
            Directions temp = result;
            result.up = temp.left;
            result.right = temp.up;
            result.down = temp.right;
            result.left = temp.down;
        }
        
        return result;
    }
    
    Directions MirrorDirectionsHorizontal(Directions original)
    {
        return new Directions
        {
            up = original.up,
            down = original.down,
            left = original.right,  // Links und rechts tauschen
            right = original.left
        };
    }
    
    void AddTileData(TileData newTileData)
    {
        // Erweitere das Array um ein Element
        TileData[] newArray = new TileData[tileData.Length + 1];
        
        // Kopiere bestehende Elemente
        for (int i = 0; i < tileData.Length; i++)
        {
            newArray[i] = tileData[i];
        }
        
        // Füge neues Element hinzu
        newArray[newArray.Length - 1] = newTileData;
        tileData = newArray;
    }
}
