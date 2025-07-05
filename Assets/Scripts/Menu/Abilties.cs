using UnityEngine;
using UnityEngine.InputSystem;

public class Abilties : MonoBehaviour
{
    private Controls controls;

    [SerializeField] private GameObject boostpadPreview;
    [SerializeField] private GameObject slowPadPreview;

    private GameObject currentPreview;

    private void Awake()
    {
        controls = new Controls();
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void Update()
    {
        //if(controls.Player)
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
}
