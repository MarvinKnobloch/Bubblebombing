using UnityEngine;

public class LevelDialog : MonoBehaviour
{
    [SerializeField] private bool playStartDialog;
    [SerializeField] private DialogObj dialogObj;
    void Start()
    {
        if (playStartDialog) GameManager.Instance.playerUI.StartDialog(dialogObj);
    }
}
