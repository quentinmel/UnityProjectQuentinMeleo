using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject cam;
    [SerializeField] float moveSpeed = 50f;
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] float jumpForce = 7;
    [SerializeField] float talkRange = 3f;

    public Animator anim;
    private Vector3 respawnPosition = new Vector3(0, 5, 0);
    Rigidbody rb;
    Vector2 moveInput;
    Vector2 lookInput;
    Animator animator;
    bool isGrounded;

    public int maxHealth = 3;
    public int currentHealth;
    public HealthBar healthBar;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);
    }

    private void Update()
    {
        anim.SetFloat("vertical", Input.GetAxis("Vertical"));
        anim.SetFloat("horizontal", Input.GetAxis("Horizontal"));

        if (moveInput.x < 0f)
        {
            animator.SetFloat("left", 1f);
            animator.SetFloat("right", 0f);
        }
        else if (moveInput.x > 0f)
        {
            animator.SetFloat("left", 0f);
            animator.SetFloat("right", 1f);
        }
        else
        {
            animator.SetFloat("left", 0f);
            animator.SetFloat("right", 0f);
        }

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.8f);
        animator.SetBool("jumping", !isGrounded);

        if (transform.position.y < -5f)
        {
            Respawn();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            Jump();
        }
    }

    private void MovePlayer()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        Vector3 targetPosition = rb.position + transform.TransformDirection(moveDirection) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, smoothTime));

        if (lookInput.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, Mathf.Atan2(lookInput.x, lookInput.y) * Mathf.Rad2Deg, 0f);
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, smoothTime));
        }

        float cameraHeight = 5f;
        float cameraDistance = 5f;
        float cameraAngle = 0f;

        Vector3 cameraOffset = Quaternion.Euler(cameraAngle, 0f, 0f) * Vector3.back * cameraDistance;
        Vector3 cameraPosition = transform.position + Vector3.up * cameraHeight + cameraOffset;
        cam.transform.position = Vector3.Lerp(cam.transform.position, cameraPosition, smoothTime);

        Vector3 lookDir = transform.position - cam.transform.position;
        cam.transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void TalkToPNJ(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, talkRange);
            foreach (Collider col in hitColliders)
            {
                if (col.CompareTag("PNJ"))
                {
                    Debug.Log("Parler aux PNJ");
                    break;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void Respawn()
    {
        transform.position = respawnPosition;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            TakeDamage(1);
        }
    }
}
