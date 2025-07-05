using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public static TurnController instance;

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
        yield return new WaitForSeconds(1);
        RollNpcMovment();
        GameManager.Instance.playerUI.EndTurnButtonToggle(true);
        GameManager.Instance.playerUI.AbilitiyUIToggle(true);
    }
    private void RollNpcMovment()
    {
        foreach (GameObject npc in npcs)
        {
            //GetNpcMinMovement
            //GetNpcMaxMovement
            int npcMinMovementPoints = 1;
            int npcMaxMovementPoints = 4;

            int movementpoints = Random.Range(npcMinMovementPoints, npcMaxMovementPoints + 1);
            //display Movementpoints
            //Set npc Movementpoints;
        }
    }
    public void NpcMovementFinished()
    {
        npcsNoMoreMovement++;
        if(npcsNoMoreMovement >= npcsOnField)
        {
            GameManager.Instance.playerUI.StartTurnButtonToggle(true);
        }
    }
    public void EndTurn() 
    {
        //StartNpcMovement
    }
}
