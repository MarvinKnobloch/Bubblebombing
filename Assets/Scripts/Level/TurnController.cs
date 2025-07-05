using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public static TurnController instance;

    private List<GameObject> npcs = new List<GameObject>();

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
    }
    public void StartTurn()
    {
        StartCoroutine(RollAnimation());
    }
    IEnumerator RollAnimation()
    {
        yield return new WaitForSeconds(1);
        RollNpcMovment();
        GameManager.Instance.playerUI.EndTurnButtonToggle(true);
    }
    private void RollNpcMovment()
    {
        foreach (GameObject npc in npcs)
        {
            //GetNpcMaxMovment
            int npcMovementPoints = 4;

            int movementpoints = Random.Range(1, npcMovementPoints + 1);
            //display Movementpoints
            //Set npc Movementpoints;
        }
    }
    public void EndTurn() 
    {
        //StartNpcMovement
    }
}
