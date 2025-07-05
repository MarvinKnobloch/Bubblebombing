using UnityEngine;

[CreateAssetMenu(fileName = "NpcValue", menuName = "ScriptableObjects/NpcValues")]
public class NpcValuesObj : ScriptableObject
{
    public int minMovementValue;
    public int maxMovementValue;
}
