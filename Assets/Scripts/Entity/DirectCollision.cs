using Unity.VisualScripting;
using UnityEngine;

public class DirectCollision : BaseCollision
{
    public ObjectType objectType;
    public bool isDestroyable;
    protected override void TriggerAction(GameObject target)
    {
        if(target.TryGetComponent(out Entity targetEntity))
        {
            switch (objectType)
            {
                case ObjectType.Damage:
                    target.GetComponent<Entity>().Damage();
                    break;
                case ObjectType.Speed:
                    target.GetComponent<Entity>().ChangeSteps(1);
                    break;
                case ObjectType.Slow:
                    target.GetComponent<Entity>().ChangeSteps(-1);
                    break;
                case ObjectType.Goal:
                    if (target.layer == 8)
                    {
                        GameManager.Instance.playerUI.Victory();
                    }
                    break;
            }
            if (isDestroyable)
            {
                Destroy(gameObject);
            }
        }
    }

}

public enum ObjectType
{
    Damage,
    Speed,
    Slow,
    Goal
}