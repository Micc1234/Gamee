using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public Vector3 spawnAreaMin = new Vector3(-50, 0, -50);
    public Vector3 spawnAreaMax = new Vector3(50, 0, 50);

    void Start()
    {
        // Delay the random positioning slightly to ensure all systems initialize first
        Invoke(nameof(RandomizePosition), 0.1f);
    }

    void RandomizePosition()
    {
        Vector3 randomPos = GetRandomPosition();
        transform.position = randomPos;
        Debug.Log($"{gameObject.name} moved to {randomPos}");
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float z = Random.Range(spawnAreaMin.z, spawnAreaMax.z);
        float y = spawnAreaMin.y; // adjust based on terrain if needed
        return new Vector3(x, y, z);
    }
}
