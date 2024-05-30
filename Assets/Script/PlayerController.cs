using System.Collections;
using TMPro;
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
    [SerializeField] AudioClip damageSoundClip;
    [SerializeField] CanvasGroup defeatUI;
    [SerializeField] CanvasGroup victoryUI;
    [SerializeField] GameObject bloodEffectPrefab;
    [SerializeField] TextMeshProUGUI totalScoreText;

    public Animator anim;
    private Vector3 respawnPosition = new Vector3(0, 5, 0);
    Rigidbody rb;
    Vector2 moveInput;
    Vector2 lookInput;
    Animator animator;
    bool isGrounded;
    private bool disableMovement = false;
    private bool deadAnimationPlayed = false;
    private AudioSource audioSource;

    public int maxHealth = 3;
    public int currentHealth;
    public HealthBar healthBar;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);

        audioSource = GetComponent<AudioSource>();

        defeatUI.alpha = 0;
        defeatUI.gameObject.SetActive(false);

        victoryUI.alpha = 0;
        victoryUI.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (disableMovement) return;

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
        if (disableMovement) return;

        MovePlayer();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (disableMovement) return;

        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (disableMovement) return;

        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (disableMovement) return;

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
        if (disableMovement) return;

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
        audioSource.clip = damageSoundClip;
        audioSource.Play();

        Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);

        if (currentHealth <= 0)
        {
            disableMovement = true;
            StartCoroutine(DelayedGameOver());
        }
    }

    private IEnumerator DelayedGameOver()
    {
        if (!deadAnimationPlayed)
        {
            Time.timeScale = 0.1f;
            var scoreController = FindObjectOfType<ScoreController>();
            if (scoreController != null)
            {
                scoreController.StopCountingScore();
            }
            totalScoreText.text = "Total Score: " + GameManager.PlayerScore.ToString();
            anim.SetBool("isDead", true);
            defeatUI.gameObject.SetActive(true);
            defeatUI.alpha = 1;

            GameManager.IsGameOver = true;
            ScoreController.gameTime = 0f;

            yield return new WaitForSecondsRealtime(6);
            Time.timeScale = 1f;
            deadAnimationPlayed = true;
        }
    }

    public void DisableMovement()
    {
        disableMovement = true;
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

public static class GameManager
{
    public static int PlayerScore { get; set; }
    public static bool IsGameOver { get; set; }
}

