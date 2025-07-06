using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public static TurnController instance;

    [SerializeField] private bool autoStartRound;
    private List<GameObject> npcs = new List<GameObject>();
    [SerializeField] private int npcsOnField;
    [SerializeField] private int npcsNoMoreMovement;

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
        yield return new WaitForSeconds(0.3f);
        RollNpcMovment();
        GameManager.Instance.playerUI.EndTurnButtonToggle(true);
        GameManager.Instance.playerUI.AbilitiyUIToggle(true);
    }
    private void RollNpcMovment()
    {
        foreach (GameObject npc in npcs)
        {
            Entity entity = npc.GetComponent<Entity>();

            int npcMinMovementPoints = entity.npcValuesObj.minMovementValue;
            int npcMaxMovementPoints = entity.npcValuesObj.maxMovementValue;

            int movementpoints = Random.Range(npcMinMovementPoints, npcMaxMovementPoints + 1);
            entity.SetRemaingSteps(movementpoints);
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
            if (GameManager.Instance.playerUI.gameOver) return;

            StartTurn();
        }
        else GameManager.Instance.playerUI.StartTurnButtonToggle(true);
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
