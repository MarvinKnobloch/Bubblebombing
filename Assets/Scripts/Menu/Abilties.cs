using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

    [Header("LevelAbilityCount")]
    [SerializeField] private int level1AbilityCount;
    [SerializeField] private int level2AbilityCount;
    [SerializeField] private int level3AbilityCount;

    private void Awake()
    {
        controls = new Controls();
        baseColor = buttons[0].GetComponent<Image>().color;

        if (SceneManager.GetActiveScene().buildIndex == (int)Scenes.Level1) ButtonDisable(level1AbilityCount);
        else if (SceneManager.GetActiveScene().buildIndex == (int)Scenes.Level2) ButtonDisable(level2AbilityCount);
        else if (SceneManager.GetActiveScene().buildIndex == (int)Scenes.Level3) ButtonDisable(level3AbilityCount);
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
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

    private void ButtonDisable(int availableAbilities)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if(i + 1 > availableAbilities)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
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
        int currentNumber = 2;
        if (buttons[currentNumber].gameObject.activeSelf == false) return;

        int costs = buttons[currentNumber].GetComponent<AbilityToolTip>().abilityTooltipObj.abilityCosts;
        ButtonUpdate(currentNumber);
        gridInteractionUI.SetInteractionType(GridInteractionType.PlaceObject, costs, lurePrefab);
    }
    public void SetHorrifyOption()
    {
        int currentNumber = 3;
        if (buttons[currentNumber].gameObject.activeSelf == false) return;

        int costs = buttons[currentNumber].GetComponent<AbilityToolTip>().abilityTooltipObj.abilityCosts;
        ButtonUpdate(currentNumber);
        gridInteractionUI.SetInteractionType(GridInteractionType.PlaceObject, costs, horrifyPrefab);
    }
    public void SetBoostOption()
    {
        int currentNumber = 4;
        if (buttons[currentNumber].gameObject.activeSelf == false) return;

        int costs = buttons[currentNumber].GetComponent<AbilityToolTip>().abilityTooltipObj.abilityCosts;
        ButtonUpdate(currentNumber);
        gridInteractionUI.SetInteractionType(GridInteractionType.PlaceObject, costs, boostPrefab);
    }
    public void SetSlowOption()
    {
        int currentNumber = 5;
        if (buttons[currentNumber].gameObject.activeSelf == false) return;

        int costs = buttons[currentNumber].GetComponent<AbilityToolTip>().abilityTooltipObj.abilityCosts;
        ButtonUpdate(currentNumber);
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
