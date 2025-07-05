using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AbilityTooltipObj abilityTooltipObj;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(abilityTooltipObj != null)
        {
            GameManager.Instance.playerUI.ToggleTooltipWindow(true, abilityTooltipObj);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.playerUI.ToggleTooltipWindow(false, abilityTooltipObj);       
    }
}
