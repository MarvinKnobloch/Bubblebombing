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
    [SerializeField] private int currentHealth;
    [SerializeField] private Image[] healthIcons;

    [Header("ActionPoints")]
    [SerializeField] private Image actionPoints;
    [SerializeField] private TextMeshProUGUI actioPointsText;

    [Header("DialogBox")]
    public GameObject dialogBox;

    [Header("NextTurn")]
    [SerializeField] private GameObject startTurnButton;
    [SerializeField] private GameObject endTurnButton;

    [Header("Abilities")]
    public Abilties abilityUI;
    [SerializeField] private GameObject tooltipWindow;
    [SerializeField] private TextMeshProUGUI tooltipName;
    [SerializeField] private TextMeshProUGUI tooltipCost;
    [SerializeField] private TextMeshProUGUI tooltipDescription;

    private float timer;

    private void Awake()
    {
        controls = new Controls();
        currentHealth = healthIcons.Length;
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
    public void HealthUpdate(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;

        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (i < currentHealth) healthIcons[i].gameObject.SetActive(true);
            else healthIcons[i].gameObject.SetActive(false);
        }
        if(currentHealth <= 0)
        {
            //gameOver
        }
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
        AbilitiyUIToggle(false);
    }
    public void AbilitiyUIToggle(bool toggle)
    {
        abilityUI.gameObject.SetActive(toggle);
    }
    public void ToggleTooltipWindow(bool toggle, AbilityTooltipObj abilityTooltipObj)
    {
        tooltipWindow.SetActive(toggle);
        if(toggle == true)
        {
            tooltipName.text = abilityTooltipObj.name;
            tooltipCost.text = abilityTooltipObj.abilityCosts.ToString();
            tooltipDescription.text = abilityTooltipObj.abilityDescription;
        }
    }
}
