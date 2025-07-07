using UnityEngine;
using System.Collections;

public class SoundCollision : BaseCollision
{
    public SoundType soundType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("Trigger detection")]
    private void TriggerFromMenu()
    {
        StartCoroutine(TriggerDetection());
    }

    private IEnumerator TriggerDetection()
    {
        circleCollider.enabled = true;
        yield return new WaitForFixedUpdate();
        circleCollider.enabled = false;
    }
    protected override void TriggerAction(GameObject target)
    {
        switch (soundType)
        {
            case SoundType.Fear:
                target.GetComponent<Entity>().Fear(transform.position);
                break;
            case SoundType.Lure:
                target.GetComponent<Entity>().Lure(transform.position);
                break;

        }
    }
}

public enum SoundType
{
    Lure,
    Fear
}