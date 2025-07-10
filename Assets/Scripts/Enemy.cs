using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour
{
    public Animator animator;
    public float moveDistance = 10f;
    public float moveSpeed = 5f;

    public enum MoveDirection { Horizontal, Vertical }
    public MoveDirection moveDirection = MoveDirection.Horizontal;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool movingPositive = true;

    private int hitCount = 0;
    public int maxHits = 5;

    private EnemyManager enemyManager;
    private Rigidbody rb;

    void Start()
    {
        initialPosition = transform.position;
        SetTargetPosition();

        // Ambil reference Rigidbody
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Supaya tidak jatuh atau miring

        // Cek EnemyManager
        enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager == null)
        {
            Debug.LogError("EnemyManager not found in the scene! Make sure it's present.");
        }

        // Animasi jalan
        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }
    }

    void FixedUpdate()
    {
        AutoMove();
    }

    void AutoMove()
    {
        Vector3 nextPosition = Vector3.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(nextPosition);

        if (Vector3.Distance(rb.position, targetPosition) < 0.01f)
        {
            movingPositive = !movingPositive;
            SetTargetPosition();
        }
    }

    void SetTargetPosition()
    {
        Vector3 directionVector = moveDirection == MoveDirection.Horizontal ? transform.right : transform.forward;
        targetPosition = initialPosition + (movingPositive ? directionVector : -directionVector) * moveDistance;
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
