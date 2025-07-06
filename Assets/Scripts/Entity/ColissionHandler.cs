using UnityEngine;
using System.Collections;
public class ColissionHandler : MonoBehaviour
{
    private CircleCollider2D circleCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TriggerConditionMet(collision.gameObject))
        {
            TriggerAction(collision.gameObject);
        }
    }

    private void TriggerAction(GameObject target)
    {
        Debug.Log("ActionIsTriggered");
    }

    private bool TriggerConditionMet(GameObject target)
    {
        return target.layer == gameObject.layer;
    }

    [ContextMenu("Test")]
    private void Test()
    {
        Debug.Log("Test");
    }
}