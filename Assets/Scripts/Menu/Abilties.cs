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
    public void CreateBoostpadPreview()
    {
        CurrentPreviewDestroy();
       currentPreview = Instantiate(boostpadPreview, Mouse.current.position.ReadValue(), Quaternion.identity);  
    }
    public void CreateSlowPadPreview()
    {
        CurrentPreviewDestroy();
        currentPreview = Instantiate(slowPadPreview, Mouse.current.position.ReadValue(), Quaternion.identity);
    }
    private void CurrentPreviewDestroy()
    {
        if (currentPreview != null) Destroy(currentPreview);
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
