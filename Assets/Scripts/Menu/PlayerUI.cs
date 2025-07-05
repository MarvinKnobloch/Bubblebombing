using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using static GameManager;

public class PlayerUI : MonoBehaviour
{
    private Controls controls;

    [Header("Interaction")]
    [SerializeField] private GameObject interactionField;
    [SerializeField] private TextMeshProUGUI interactionText;

    [Header("Health")]
    [SerializeField] private Image healthbar;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("ActionPoints")]
    [SerializeField] private Image actionPoints;
    [SerializeField] private TextMeshProUGUI actioPointsText;

    [Header("DialogBox")]
    public GameObject dialogBox;

    [Header("NextTurn")]
    [SerializeField] private GameObject startTurnButton;
    [SerializeField] private GameObject endTurnButton;

    private float timer;

    private void Awake()
    {
        controls = new Controls();
    }
    private void Start()
    {
        StartCoroutine(InteractionFieldDisable());
    }
    IEnumerator InteractionFieldDisable()
    {
        yield return null;
        interactionField.SetActive(false);
        interactionField.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }
    public void HandleInteractionBox(bool state)
    {
        if (interactionField != null) interactionField.SetActive(state);
    }
    public void InteractionTextUpdate(string text)
    {
        interactionText.text = text + " (<color=green>" + controls.Player.Interact.GetBindingDisplayString() + "</color>)";
    }
    public void HealthUIUpdate(int current, int max)
    {
        healthbar.fillAmount = (float)current / max;
        healthText.text = current + "/" + max;
    }
    public void ActionPoints(int current, int max)
    {
        actionPoints.fillAmount = (float)current / max;
        actioPointsText.text = current + "/" + max;
    }
    public void StartDialog(DialogObj dialogObj)
    {
        dialogBox.GetComponent<DialogBox>().DialogStart(dialogObj);
        dialogBox.SetActive(true);
    }
    public void StartTurnButtonToggle(bool toggle)
    {
        startTurnButton.SetActive(toggle);
    }
    public void StartTurnButtonPress()
    {
        TurnController.instance.StartTurn();
        StartTurnButtonToggle(false);
    }
    public void EndTurnButtonToggle(bool toggle)
    {
        endTurnButton.SetActive(toggle);
    }
    public void EndTurnButtonPress()
    {
        TurnController.instance.EndTurn();
        EndTurnButtonToggle(false);
    }
}
