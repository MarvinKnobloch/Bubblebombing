using System.Text.RegularExpressions;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private string entityName;
    [SerializeField] private float moveSpeed = 5f;
    private int remainingSteps = 3;
    private float t = 0;
    [SerializeField] private EntityState state;
    public Rigidbody2D rb;
    public Vector2 target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case EntityState.Idle:
                IdleUpdate();
                break;
            case EntityState.Move:
                MoveUpdate();
                break;
            default:
                Debug.Log("Incorect EntityState");
                break;
        }
    }

    private void MoveUpdate()
    {
        t += moveSpeed * Time.fixedDeltaTime;
        if (t > 1)
        {
            OnTargetReached();
        }
        rb.MovePosition(Vector2.Lerp(rb.position, target, t));
    }

    private void IdleUpdate()
    {

    }

    private void OnTargetReached()
    {
        t = 0;
        remainingSteps += -1;
        if (remainingSteps <= 0)
        {
            TurnController.instance.NpcMovementFinished();

        }
        else
        {
            target = GetNextTarget();
        }
    }

    private Vector2 GetNextTarget()
    {
        float x = Random.Range(-2f, 2f);
        float y = Random.Range(-2f, 2f);
        return new Vector2(x, y);
    }
}

[System.Serializable]
public enum EntityState
{
    Idle,
    Move
}