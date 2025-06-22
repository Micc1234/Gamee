using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;

    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 3f;

    [Header("Jump Settings")]
    public float jumpHeight = 2f; 
    private float gravity = 9.81f;

    private float horizontalInput;
    private float verticalInput;
    private bool isRunning;

    private float mouseX;
    private float mouseY;
    private float verticalRotation = 0f;

    private bool isGrounded;

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
        mouseY = Input.GetAxis("Mouse Y");

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            float jumpForce = Mathf.Sqrt(2 * gravity * jumpHeight);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }

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
        float xRotation = -mouseY * rotationSpeed;

        verticalRotation += xRotation;
        verticalRotation = Mathf.Clamp(verticalRotation, -60f, 60f);

        Vector3 newRotation = new Vector3(verticalRotation, transform.localEulerAngles.y + yRotation, 0f);
        transform.localEulerAngles = newRotation;
    }
}
