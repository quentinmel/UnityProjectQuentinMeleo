using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject cam;
    [SerializeField] float moveSpeed = 50f;
    [SerializeField] float lookSpeed = 2f;
    [SerializeField] float smoothTime = 0.1f;

    public Animator anim;
    Rigidbody rb;
    Vector2 moveInput;
    Vector2 lookInput;
    Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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

        float cameraHeight = 2f;
        float cameraDistance = 4f;
        float cameraAngle = 5f;

        Vector3 cameraOffset = Quaternion.Euler(cameraAngle, 0f, 0f) * Vector3.back * cameraDistance;
        Vector3 cameraPosition = transform.position + Vector3.up * cameraHeight + cameraOffset;
        cam.transform.position = Vector3.Lerp(cam.transform.position, cameraPosition, smoothTime);

        Vector3 lookDir = transform.position - cam.transform.position;
        cam.transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);

        if (moveInput.magnitude > 0.1f)
        {
            animator.SetTrigger("Walk");
        }
    }

    public void OnFire()
    {
        Debug.Log("fired");
    }
}
