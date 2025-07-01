using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public float moveDistance = 1f;
    public float moveSpeed = 1f;

    public enum MoveDirection { Horizontal, Vertical }
    public MoveDirection moveDirection = MoveDirection.Horizontal;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool movingPositive = true;

    private int hitCount = 0; // Tracks the number of hits
    public int maxHits = 5;   // The maximum hits before destruction

    // Optimized: Get reference to EnemyManager once at Start
    private EnemyManager enemyManager;

    void Start()
    {
        initialPosition = transform.position;
        SetTargetPosition();

        // Find and store the reference to EnemyManager
        enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager == null)
        {
            Debug.LogError("EnemyManager not found in the scene! Make sure it's present.");
        }

        // Set animation for continuous walk if applicable
        if (animator != null)
        {
            // Assuming "IsWalking" is a boolean parameter in your Animator
            // and controls the default walking animation.
            animator.SetBool("IsWalking", true);
        }
    }

    void Update()
    {
        AutoMove();
        // UpdateAnimations() is no longer called every frame if "IsWalking" is set once in Start.
        // If you have other animations to trigger based on actions, add them here.
    }

    void AutoMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            movingPositive = !movingPositive;
            SetTargetPosition();
        }
    }

    void SetTargetPosition()
    {
        // Use transform.right for X-axis and transform.forward for Z-axis movement relative to the enemy's local orientation.
        // If you want world X or Z, use Vector3.right or Vector3.forward respectively.
        Vector3 directionVector = moveDirection == MoveDirection.Horizontal ? transform.right : transform.forward;
        targetPosition = initialPosition + (movingPositive ? directionVector : -directionVector) * moveDistance;
    }

    // Public method to be called when the enemy takes damage
    public void TakeDamage()
    {
        hitCount++;
        Debug.Log("Enemy Hit! Current hits: " + hitCount + " (Max: " + maxHits + ")");

        // Optional: Add visual/audio feedback here for when the enemy is hit but not destroyed
        // Example: Play a hit sound, change color temporarily, etc.

        if (hitCount >= maxHits)
        {
            // Call OnEnemyDestroyed on the EnemyManager to update the remaining enemy count
            if (enemyManager != null)
            {
                enemyManager.OnEnemyDestroyed();
            }
            else
            {
                Debug.LogWarning("EnemyManager reference is null. Enemy count will not be updated.");
            }

            Destroy(gameObject); // Destroy the enemy GameObject
            Debug.Log("Enemy Destroyed!");
        }
    }
}