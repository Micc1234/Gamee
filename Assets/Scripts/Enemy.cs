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

    void Start()
    {
        initialPosition = transform.position;
        SetTargetPosition();
    }

    void Update()
    {
        AutoMove();
        UpdateAnimations();
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
        Vector3 directionVector = moveDirection == MoveDirection.Horizontal ? transform.right : transform.forward;
        targetPosition = initialPosition + (movingPositive ? directionVector : -directionVector) * moveDistance;
    }

    void UpdateAnimations()
    {
        if (animator != null)
            animator.SetTrigger("walk");
    }

    public void TakeDamage()
    {
        hitCount++; // Increment hitCount each time TakeDamage is called
        Debug.Log("Enemy Hit! Current hits: " + hitCount + " (Max: " + maxHits + ")"); // Added for debugging
        if (hitCount >= maxHits) // Check if hitCount has reached or exceeded maxHits
        {
            Destroy(gameObject); // If so, destroy the GameObject (makes it disappear)
            Debug.Log("Enemy Destroyed!"); // Added for debugging
        }
    }

    // --- PENTING: HAPUS ATAU KOMENTARI BLOK KODE INI ---
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(); // Ini akan memicu hit kedua jika peluru visual kena
            Destroy(other.gameObject);
        }
    }
    */
}