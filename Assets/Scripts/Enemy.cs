using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 1f;
    private float minSpeed = 0.5f;
    private float maxSpeed = 2f;

    private Vector3 moveDirection;
    private float changeDirectionTimer;
    private float minChangeTime = 1f;
    private float maxChangeTime = 5f;

    private int hitCount = 0;
    public int maxHits = 3;

    private EnemyManager enemyManager;

    void Start()
    {
        PickNewDirection();

        enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager == null)
        {
            Debug.LogError("EnemyManager not found in the scene! Make sure it's present.");
        }

        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }
    }

    void Update()
    {
        MoveInCurrentDirection();

        // Countdown timer
        changeDirectionTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0f)
        {
            PickNewDirection();
        }
    }

    void PickNewDirection()
    {
        // Random direction on XZ plane
        Vector2 randomXZ = Random.insideUnitCircle.normalized;
        moveDirection = new Vector3(randomXZ.x, 0f, randomXZ.y);

        // Random movement speed
        moveSpeed = Random.Range(minSpeed, maxSpeed);

        // Random time before next direction change
        changeDirectionTimer = Random.Range(minChangeTime, maxChangeTime);

        Debug.DrawRay(transform.position, moveDirection * 2f, Color.cyan, 1f);
    }

    void MoveInCurrentDirection()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void TakeDamage()
    {
        hitCount++;
        Debug.Log("Enemy Hit! Current hits: " + hitCount + " (Max: " + maxHits + ")");

        if (hitCount >= maxHits)
        {
            if (enemyManager != null)
            {
                enemyManager.OnEnemyDestroyed();
            }
            else
            {
                Debug.LogWarning("EnemyManager reference is null. Enemy count will not be updated.");
            }

            Destroy(gameObject);
            Debug.Log("Enemy Destroyed!");
        }
    }
}
