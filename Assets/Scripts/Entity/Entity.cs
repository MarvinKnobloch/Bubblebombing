using UnityEngine;

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
    public Vector2Int PositionOnGrid = new Vector2Int(0,0);
    private Direction facedDirection = Direction.Up;
    private Direction[] directionsToCheck = new Direction[4];
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
    }

    private Vector2Int GetNextTargetCoord()
    {
        float rand = UnityEngine.Random.Range(0f, 1f);
        directionsToCheck[0] = facedDirection;
        directionsToCheck[3] = LevelGrid.GetOppositeDirection(facedDirection);
        if (rand < 0.5)
        {
            directionsToCheck[1] = GetLeftDirection(facedDirection);
            directionsToCheck[2] = GetRightDirection(facedDirection);
        }
        else
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
                            neighborY = y + 1;
                            break;
                        case Direction.Down:
                            neighborY = y - 1;
                            break;
                        case Direction.Left:
                            neighborX = x - 1;
                            break;
                        case Direction.Right:
                            neighborX = x + 1;
                            break;
                    }
                    PositionOnGrid = new Vector2Int(neighborX, neighborY);
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
                throw new Exception("Invalid direction");
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
                throw new Exception("Invalid direction");
        }
    }
}

[System.Serializable]
public enum EntityState
{
    Idle,
    Move
}
