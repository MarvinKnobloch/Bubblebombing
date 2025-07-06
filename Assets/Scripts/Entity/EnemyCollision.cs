using UnityEngine;

public class EnemyCollision : DirectCollision
{    protected override void TriggerAction(GameObject target)
    {
		if (gameObject.GetComponent<Entity>().state != EntityState.Spawning)
		{
			base.TriggerAction(target);
			gameObject.GetComponent<Entity>().Respawn();
		}
    }
}
