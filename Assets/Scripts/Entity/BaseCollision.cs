using UnityEngine;
using System.Linq;
public class BaseCollision : MonoBehaviour
{
    public LayerMask collisionLayers;
    protected CircleCollider2D circleCollider;
    public Vector2Int PositionOnGrid = new Vector2Int(0, 0);
    public bool setPositionOnStart = false;

    protected virtual void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        if (setPositionOnStart)
            transform.position = GridRenderer.instance.TileToWorldPosition(PositionOnGrid);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TriggerConditionMet(collision.gameObject))
        {
            TriggerAction(collision.gameObject);
        }
    }

    protected virtual void TriggerAction(GameObject target)
    {
    }

    protected virtual bool TriggerConditionMet(GameObject target)
    {
        // Check if the layer is in the mask (more readable)
        return ((1 << target.gameObject.layer) & collisionLayers) != 0;
    }
}