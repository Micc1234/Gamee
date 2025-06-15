using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;

    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 3f;

    private float horizontalInput;
    private float verticalInput;
    private bool isRunning;

    private float mouseX;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb.angularDamping = 10f;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        isRunning = Input.GetKey(KeyCode.LeftShift) && verticalInput > 0;

        mouseX = Input.GetAxis("Mouse X");
        mouseX = Mathf.Clamp(mouseX, -1f, 1f); 

        UpdateAnimations();
    }

    void FixedUpdate()
    {
        MoveCharacter();
        RotateCharacter();
    }

    void UpdateAnimations()
    {
        if (verticalInput == 0 && horizontalInput == 0)
        {
            animator.SetTrigger("idle");
        }
        else if (verticalInput > 0 && !isRunning)
        {
            animator.SetTrigger("walk");
        }
        else if (verticalInput < 0)
        {
            animator.SetTrigger("walkback");
        }
        else if (isRunning)
        {
            animator.SetTrigger("run");
        }
        else if (horizontalInput != 0)
        {
            animator.SetTrigger("walk");
        }
    }

    void MoveCharacter()
    {
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput) * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);
    }

    void RotateCharacter()
    {
        float yRotation = mouseX * rotationSpeed;

        Quaternion deltaRotation = Quaternion.Euler(0f, yRotation, 0f);

        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}