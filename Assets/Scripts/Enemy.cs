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

    private int hitCount = 0;
    public int maxHits = 5;

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
        hitCount++;
        if (hitCount >= maxHits)
        {
            Destroy(gameObject); // Musuh mati
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(); // Tambah 1 hit
            Destroy(other.gameObject); // Hancurkan peluru
        }
    }
}
