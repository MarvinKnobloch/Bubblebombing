using UnityEngine;
using UnityEngine.InputSystem;

public class Abilties : MonoBehaviour
{
    [SerializeField] private GameObject boostpadPreview;
    public void CreatBoostpadPreview()
    {
        Instantiate(boostpadPreview, Mouse.current.position.ReadValue(), Quaternion.identity);  
    }
}
