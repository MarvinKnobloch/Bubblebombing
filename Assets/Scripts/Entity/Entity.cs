using System;
using TMPro;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public SpriteManager spriteManager;
    public string entityName = "Point";
    [SerializeField] private float moveSpeed = 5f;
    public int remainingSteps = 3;
    public float t = 0;
    private float animationT = 0;
    public int respawnTime;
    private int remainingRespawnTime;
    [SerializeField] public EntityState state;
    public Rigidbody2D rb;
    public Vector2 target;
    private Vector2 oldPosition;
    public Vector2Int PositionOnGrid = new Vector2Int(0,0);
    public Direction facedDirection = Direction.Up;
    private Direction[] directionsToCheck = new Direction[4];
    private bool stopMoving;

    [Header("Other")]
    [SerializeField] private Transform childSprite;
    public NpcValuesObj npcValuesObj;
    [SerializeField] private TextMeshProUGUI movementText;
    void Start()
    {
        Face(facedDirection);
        rb.position = GridRenderer.instance.TileToWorldPosition(PositionOnGrid);

        TurnController.instance.AddNpc(gameObject);
        //startTurn();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case EntityState.Idle:
                movementText.color = Color.white;
                IdleUpdate();
                break;
            case EntityState.Move:
                movementText.color = Color.black;
                MoveUpdate();
                break;
            case EntityState.Spawning:
                movementText.color = Color.red;
                MoveUpdate();
                SpawnUpdate();
                break;
            default:
                throw new Exception("Invalid State");
        }
    }

    [ContextMenu("Start turn")]
    public void StartTurn()
    {
        if (remainingRespawnTime <= 0)
        {
            state = EntityState.Idle;
        }
        if (state == EntityState.Idle)
        {
            GetNextTarget();
        }
        t = 0;
        animationT = 0;

        if (state == EntityState.Idle)
		{
			state = EntityState.Move;
            stopMoving = false;
		}
	}

    private void MoveUpdate()
    {
        t += moveSpeed * Time.deltaTime;
        if (t >= 1)
        {
            if (state != EntityState.Spawning)
            {
                OnTargetReached();
            }
            else
            {
                stopMoving = true;
            }
        }
        else if (!stopMoving)
        {
            rb.MovePosition(Vector2.Lerp(oldPosition, target, t));
        }
    }

    private void SpawnUpdate()
    {
        animationT += moveSpeed * Time.deltaTime;
        if ((animationT >= 1)&&(animationT < 2))
        {
            SpawnTick();
        }
		else if (animationT < 2)
		{
			//Debug.Log("Update transparency!");
		}

    }

    private void SpawnTick()
    {
        Debug.Log("Spawn Tick");
        remainingRespawnTime -= 1;
        TurnController.instance.NpcMovementFinished();
        animationT = 2;

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
			GetComponent<PlaceableObject>().UpdatePosition(PositionOnGrid);
        }
        else
        {
            GetNextTarget();
        }
        movementText.text = remainingSteps.ToString();
    }
    public void SetRemaingSteps(int steps)
    {
        remainingSteps = steps;
        movementText.text = remainingSteps.ToString();
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
        return GridRenderer.instance.WorldToTilePosition(transform.position);
    }

    private void Face(Direction direction)
    {
        facedDirection = direction;
        spriteManager.Face(direction);
        //switch (direction)
        //{
        //    case Direction.Up:
        //        childSprite.rotation = Quaternion.Euler(0, 0, 0);
        //        break;
        //    case Direction.Left:
        //        childSprite.rotation = Quaternion.Euler(0, 0, 90);
        //        break;
        //    case Direction.Down:
        //        childSprite.rotation = Quaternion.Euler(0, 0, 180);
        //        break;
        //    case Direction.Right:
        //        childSprite.rotation = Quaternion.Euler(0, 0, 270);
        //        break;
        //}
    }

    public void Fear(Vector2 sourcePos)
    {
        Direction closestFacingDirection = GetClosestFacing(transform.position, sourcePos);
        Face(LevelGrid.GetOppositeDirection(closestFacingDirection));
    }
    public void Lure(Vector2 sourcePos)
    {
        Direction closestFacingDirection = GetClosestFacing(transform.position, sourcePos);
        Face(closestFacingDirection);
    }
    [ContextMenu("1 Damage")]
    public void Damage()
    {
        GameManager.Instance.playerUI.HealthUpdate(1);
    }

    public void Respawn()
    {
        if (state != EntityState.Spawning)
        {
            remainingRespawnTime = respawnTime;
            TurnController.instance.NpcMovementFinished();
            Debug.Log("Respawning " + entityName);
            state = EntityState.Spawning;
            animationT = 2;
        }
    }

    public void ChangeSteps(int val)
    {
        remainingSteps += val;
        remainingSteps = Math.Max(remainingSteps, 0);
    }


    public Direction GetClosestFacing(Vector2 V1, Vector2 V2)
    {
        // Calculate direction vector from V1 to V2
        Vector3 direction = (V2 - V1).normalized;

        // Get the angle in degrees (-180 to 180)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Normalize angle to 0-360 range
        if (angle < 0)
            angle += 360;

        if (angle >= 315 || angle < 45)
            return Direction.Right;
        else if (angle >= 45 && angle < 135)
            return Direction.Up;
        else if (angle >= 135 && angle < 225)
            return Direction.Left;
        else // angle >= 225 && angle < 315
            return Direction.Down;
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
    Move,
    Spawning
}
