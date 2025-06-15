using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float lifeTime = 1f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}