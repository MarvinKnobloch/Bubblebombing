using System;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Entity : MonoBehaviour
{
    private string entityName = "Point";
    [SerializeField] private float moveSpeed = 5f;
    public int remainingSteps = 3;
    public float t = 0;
    [SerializeField] private EntityState state;
    public Rigidbody2D rb;
    public Vector2 target;
    private Vector2 oldPosition;
    private Vector2Int PositionOnGrid = new Vector2Int(0,0);
    private Direction facedDirection = Direction.Up;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.position = GridRenderer.instance.TileToWorldPosition(PositionOnGrid);
        startTurn();
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

    private void startTurn()
    {
        state = EntityState.Move;
        GetNextTarget();
    }

    private void MoveUpdate()
    {
        t += moveSpeed * Time.deltaTime;
        if (t >= 1)
        {
            OnTargetReached();
        }
        else
        {
            rb.MovePosition(Vector2.Lerp(oldPosition, target, t));
        }
    }

    private void IdleUpdate()
    {

    }

    private void OnTargetReached()
    {
        t = 0;
        remainingSteps -= 1;
        if (remainingSteps <= 0)
        {
            TurnController.instance.NpcMovementFinished();
            state = EntityState.Idle;
        }
        else
        {
            GetNextTarget();
        }
    }
    private void GetNextTarget()
    {
        Vector2Int targetTile = GetNextTargetCoord();
        Vector3 target3D = GridRenderer.instance.TileToWorldPosition(targetTile);
        target = new Vector2(target3D.x, target3D.y);
        oldPosition = rb.position;
        Debug.Log("Set new target to: "+target.ToString());
    }

    private Vector2Int GetNextTargetCoord()
    {
        float rand = 0;
        Direction[] directionsToCheck = { facedDirection, GetLeftDirection(facedDirection), GetRightDirection(facedDirection), LevelGrid.GetOppositeDirection(facedDirection) };
        if (rand < 0.5)
        {
            directionsToCheck[2] = GetLeftDirection(facedDirection);
            directionsToCheck[1] = GetRightDirection(facedDirection);
        }
        for (int i = 0; i < directionsToCheck.Length; i++)
        {
            if (LevelGrid.instance.IsDirectionFree(PositionOnGrid.x, PositionOnGrid.y, directionsToCheck[i]))
            {
                //get neighbour coords
                int x = PositionOnGrid.x;
                int y = PositionOnGrid.y;
                int neighborX = x, neighborY = y;
                switch (directionsToCheck[i])
                {
                    case Direction.Up:
                        neighborY = y - 1;
                        break;
                    case Direction.Down:
                        neighborY = y + 1;
                        break;
                    case Direction.Left:
                        neighborX = x - 1;
                        break;
                    case Direction.Right:
                        neighborX = x + 1;
                        break;
                }
                PositionOnGrid = new Vector2Int(neighborX, neighborY);
                Debug.Log(entityName + " has CHoosen direction " + directionsToCheck[i]);
                Debug.Log("Target is at pos("+neighborX+ ", " + neighborY + ")");
                Face(directionsToCheck[i]);
                return new Vector2Int(neighborX, neighborY);
            }
        }
        return new Vector2Int(0,0);
    }

    private void Face(Direction direction)
    {
        Debug.Log("Facing direction" + direction);
        facedDirection = direction;
        switch (direction)
        {
            case Direction.Up:
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.Left:
                this.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.Down:
                this.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case Direction.Right:
                this.transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
        }
    }
    public static Direction GetLeftDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Left;
            case Direction.Down:
                return Direction.Right;
            case Direction.Left:
                return Direction.Down;
            case Direction.Right:
                return Direction.Up;
            default:
                return Direction.Up;
        }
    }
    public static Direction GetRightDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Right;
            case Direction.Down:
                return Direction.Left;
            case Direction.Left:
                return Direction.Up;
            case Direction.Right:
                return Direction.Down;
            default:
                return Direction.Up;
        }
    }
}

[System.Serializable]
public enum EntityState
{
    Idle,
    Move
}
