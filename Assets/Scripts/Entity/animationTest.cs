using UnityEngine;

public class animationTest : MonoBehaviour
{
    [SerializeField] private float lifeTime;

    private void Start()
    {
        Destroy(gameObject.transform.parent.gameObject, lifeTime);
    }
}
