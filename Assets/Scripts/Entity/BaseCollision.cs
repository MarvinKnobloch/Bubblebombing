using UnityEngine;
using System.Linq;
public class BaseCollision : MonoBehaviour
{
    public LayerMask collisionLayers;
    protected CircleCollider2D circleCollider;

    protected virtual void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
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
        Debug.Log("ActionIsTriggered");
    }

    protected virtual bool TriggerConditionMet(GameObject target)
    {
        // Check if the layer is in the mask (more readable)
        return ((1 << target.gameObject.layer) & collisionLayers) != 0;
    }

    [ContextMenu("Test")]
    private void Test()
    {
        Debug.Log("Test");
    }
}