using UnityEngine;

public class LevelDialog : MonoBehaviour
{
    [SerializeField] private bool playStartDialog;
    [SerializeField] private DialogObj dialogObj;

    [SerializeField] private VoidEventChannel startRound;

    private void OnEnable()
    {
        startRound.OnEventRaised += StartRound;
    }
    private void OnDisable()
    {
        startRound.OnEventRaised -= StartRound;
    }
    void Start()
    {
        if (playStartDialog && dialogObj != null) GameManager.Instance.playerUI.StartDialog(dialogObj);
        else StartRound();
    }

    private void StartRound()
    {
        TurnController.instance.CheckForNextRound();
    }
}
