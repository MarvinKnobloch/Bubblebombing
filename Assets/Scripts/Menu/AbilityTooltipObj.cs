using UnityEngine;

[CreateAssetMenu(fileName = "Abilities", menuName = "ScriptableObjects/AbilityTooltip")]
public class AbilityTooltipObj : ScriptableObject
{
    public string abilityName;
    public int abilityCosts;
    [TextArea]
    public string abilityDescription;
}
