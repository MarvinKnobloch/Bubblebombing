using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingPreview : MonoBehaviour
{
    private Camera cam;
    private void Awake()
    {
        cam = Camera.main;
    }
    void Update()
    {
        Vector3 mouseposi = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = new Vector3(Mathf.Round(mouseposi.x + 0.5f) - 0.5f, Mathf.Round(mouseposi.y + 0.5f) - 0.5f, 0);

        //Vector2 position = GridRenderer.instance.WorldToTilePosition(mouseposi);
        //transform.position = position;

        //Vector3 mouseposi = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //mouseposi.z = 0;
        //gameObject.transform.position = mouseposi;
    }
}
