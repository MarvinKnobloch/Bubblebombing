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
    }
    private void OnEnable()
    {
        baseColor = buttons[0].GetComponent<Image>().color;
        controls.Enable();
    }
    private void Update()
    {
        if (controls.Menu.MenuESC.WasPerformedThisFrame()) ResetType();
        else if (controls.Player.Ability1.WasPerformedThisFrame()) SetMoveOption();
        else if (controls.Player.Ability2.WasPerformedThisFrame()) SetRotationOption();
        else if (controls.Player.Ability3.WasPerformedThisFrame()) SetLureOption();
        else if (controls.Player.Ability4.WasPerformedThisFrame()) SetHorrifyOption();

        //else if (controls.Player.Ability5.WasPerformedThisFrame())
        //{
        //    CreateBoostpadPreview();
        //}
        //else if (controls.Player.Ability6.WasPerformedThisFrame())
        //{
        //    CreateSlowPadPreview();
        //}
        //else if (controls.Player.RMB.WasPerformedThisFrame())
        //{
        //    CurrentPreviewDestroy();
        //}
    }
    public void SetMoveOption()
    {
        gridInteractionUI.SetInteractionType(GridInteractionType.MoveLine);
        ButtonUpdate(0);
    }
    public void SetRotationOption() 
    {
        gridInteractionUI.SetInteractionType(GridInteractionType.RotateTile);
        ButtonUpdate(1);
    }
    public void SetLureOption()
    {
        gridInteractionUI.SetInteractionType(GridInteractionType.PlaceObject, lurePrefab);
        ButtonUpdate(2);
    }
    public void SetHorrifyOption()
    {
        if (horrifyPrefab == null) return;
        gridInteractionUI.SetInteractionType(GridInteractionType.PlaceObject, horrifyPrefab);
        ButtonUpdate(3);
    }
    public void SetBoostOption()
    {
        if (boostPrefab == null) return;
        gridInteractionUI.SetInteractionType(GridInteractionType.PlaceObject, boostPrefab);
        ButtonUpdate(4);
    }
    public void SetSlowOption()
    {
        if (slowPrefab == null) return;
        gridInteractionUI.SetInteractionType(GridInteractionType.PlaceObject, slowPrefab);
        ButtonUpdate(5);
    }
    private void ResetType()
    {
        gridInteractionUI.SetInteractionType(GridInteractionType.None);
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
