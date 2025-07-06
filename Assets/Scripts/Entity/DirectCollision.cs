using UnityEngine;

public class DirectCollision : BaseCollision
{
    public ObjectType objectType;
    public bool isDestroyable;
    protected override void TriggerAction(GameObject target)
    {
        switch (objectType)
        {
            case ObjectType.Damge:
                target.GetComponent<Entity>().Damage();
                break;
            case ObjectType.Speed:
                target.GetComponent<Entity>().ChangeSteps(1);
                break;
            case ObjectType.Slow:
                target.GetComponent<Entity>().ChangeSteps(-1);
                break;

        }
        if (isDestroyable)
        {
            Destroy(this);
        }
    }

}

public enum ObjectType
{
    Damge,
    Speed,
    Slow
}