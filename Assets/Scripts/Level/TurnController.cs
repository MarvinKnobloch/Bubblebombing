using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public static TurnController instance;

    [Header("ActionPoints")]
    [SerializeField] private int maxActionPoints;
    [SerializeField] private int actionPointsEachRound;
    private int currentActionPoints;

    [Header("TurnValues")]
    [SerializeField] private bool autoStartRound;
    [SerializeField] private float rollDelay;
    [SerializeField] private float delayAfterMovementFinished;
    private List<GameObject> npcs = new List<GameObject>();
    private int npcsOnField;
    private int npcsNoMoreMovement;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }
    public void AddNpc(GameObject npc)
    {
        if(npcs.Contains(npc) == false)
        {
            npcs.Add(npc);
        }
        npcsOnField = npcs.Count;
    }
    public void StartTurn()
    {
        npcsNoMoreMovement = 0;
        StartCoroutine(RollAnimation());
    }
    IEnumerator RollAnimation()
    {
        RollNpcMovment();
        yield return new WaitForSeconds(rollDelay);
        GameManager.Instance.playerUI.EndTurnButtonToggle(true);
        GameManager.Instance.playerUI.AbilitiyUIToggle(true);

        currentActionPoints += actionPointsEachRound;
        if (currentActionPoints > maxActionPoints) currentActionPoints = maxActionPoints;
        ActionPointsUpdate(0);
    }
    public bool CheckForActionPoints(int amount)
    {
        if (currentActionPoints >= amount) return true;
        else return false;
    }
    public void ActionPointsUpdate(int amount)
    {
        currentActionPoints -= amount;
        GameManager.Instance.playerUI.ActionPoints(currentActionPoints);
    }
    private void RollNpcMovment()
    {
        foreach (GameObject npc in npcs)
        {
            Entity entity = npc.GetComponent<Entity>();
            entity.RollNewMovement();
        }
    }
    public void NpcMovementFinished()
    {
        npcsNoMoreMovement++;
        if(npcsNoMoreMovement >= npcsOnField)
        {
            CheckForNextRound();
        }
    }
    public void CheckForNextRound()
    {
        if (autoStartRound)
        {
            if (GameManager.Instance.playerUI.isGameOver) return;
            StartCoroutine(StartNextTurn());
        }
        else GameManager.Instance.playerUI.StartTurnButtonToggle(true);
    }
    IEnumerator StartNextTurn()
    {
        yield return new WaitForSeconds(delayAfterMovementFinished);
        StartTurn();
    }
    public void EndTurn() 
    {
        foreach (GameObject npc in npcs)
        {
            Entity entity = npc.GetComponent<Entity>();
            entity.StartTurn();
        }
        //StartNpcMovement
    }
}
