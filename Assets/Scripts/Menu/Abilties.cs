using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Abilties : MonoBehaviour
{
    private Controls controls;

    [SerializeField] private GameObject boostpadPreview;
    [SerializeField] private GameObject slowPadPreview;

    private GameObject currentPreview;
    public GridInteractionUI gridInteractionUI;

    [SerializeField] private Button[] buttons;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color selectedColor;

    [Header("PlacementPrefabs")]
    [SerializeField] private PlaceableObject lurePrefab;
    [SerializeField] private PlaceableObject horrifyPrefab;
    [SerializeField] private PlaceableObject boostPrefab;
    [SerializeField] private PlaceableObject slowPrefab;

    private void Awake()
    {
        controls = new Controls();
        baseColor = buttons[0].GetComponent<Image>().color;
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void Update()
    {
        if (controls.Menu.MenuESC.WasPerformedThisFrame()) ResetType();
        else if (controls.Player.Ability1.WasPerformedThisFrame()) SetMoveOption();
        else if (controls.Player.Ability2.WasPerformedThisFrame()) SetRotationOption();
        else if (controls.Player.Ability3.WasPerformedThisFrame()) SetLureOption();
        else if (controls.Player.Ability4.WasPerformedThisFrame()) SetHorrifyOption();
        else if (controls.Player.Ability5.WasPerformedThisFrame()) SetBoostOption();
        else if (controls.Player.Ability6.WasPerformedThisFrame()) SetSlowOption();
    }
    public void SetMoveOption()
    {
        int costs = buttons[0].GetComponent<AbilityToolTip>().abilityTooltipObj.abilityCosts;
        ButtonUpdate(0);
        gridInteractionUI.SetInteractionType(GridInteractionType.MoveLine, costs);
    }
    public void SetRotationOption() 
    {
        int costs = buttons[1].GetComponent<AbilityToolTip>().abilityTooltipObj.abilityCosts;
        ButtonUpdate(1);
        gridInteractionUI.SetInteractionType(GridInteractionType.RotateTile, costs);
    }
    public void SetLureOption()
    {
        int costs = buttons[2].GetComponent<AbilityToolTip>().abilityTooltipObj.abilityCosts;
        ButtonUpdate(2);
        gridInteractionUI.SetInteractionType(GridInteractionType.PlaceObject, costs, lurePrefab);
    }
    public void SetHorrifyOption()
    {
        int costs = buttons[3].GetComponent<AbilityToolTip>().abilityTooltipObj.abilityCosts;
        ButtonUpdate(3);
        gridInteractionUI.SetInteractionType(GridInteractionType.PlaceObject, costs, horrifyPrefab);
    }
    public void SetBoostOption()
    {
        int costs = buttons[4].GetComponent<AbilityToolTip>().abilityTooltipObj.abilityCosts;
        ButtonUpdate(4);
        gridInteractionUI.SetInteractionType(GridInteractionType.PlaceObject, costs, boostPrefab);
    }
    public void SetSlowOption()
    {
        int costs = buttons[5].GetComponent<AbilityToolTip>().abilityTooltipObj.abilityCosts;
        ButtonUpdate(5);
        gridInteractionUI.SetInteractionType(GridInteractionType.PlaceObject, costs, slowPrefab);
    }
    private void ResetType()
    {
        gridInteractionUI.SetInteractionType(GridInteractionType.None, 0);
        ButtonUpdate(-1);
    }
    private void ButtonUpdate(int selectedNumber)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if(i == selectedNumber) buttons[selectedNumber].GetComponent<Image>().color = selectedColor;
            else buttons[i].GetComponent<Image>().color = baseColor;
        }       
    }
}
