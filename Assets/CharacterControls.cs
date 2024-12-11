using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControls : MonoBehaviour
{

    private Animator animator;
    Vector2 moveInput;

    private int groundLayer;
    private bool grounded;
    [SerializeField] private float movementSpeed = 5f;
    private Rigidbody rb;

    private float fallingTimer = 0f;

    private PlayerControls playerControls;
    // Start is called before the first frame update
    void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>(); 
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        if (playerControls is not null)
            playerControls.Enable();
    StartCoroutine(PrintGroundedStatus());

    }

    private void OnDisable()
    {
        if (playerControls is not null)
            playerControls.Disable();
    StopCoroutine(PrintGroundedStatus());


    }

    // Update is called once per frame
    void FixedUpdate()
{
    if (playerControls is not null)
    {
        grounded = IsGrounded();
        // Read player input and update animator
        if (grounded)
        {
            moveInput = playerControls.Player.Move.ReadValue<Vector2>();
            fallingTimer = 0f;
        }
        else
            fallingTimer += Time.deltaTime;
        animator.SetFloat("SpeedX", moveInput.x);
        animator.SetFloat("SpeedZ", moveInput.y);

        if(!grounded && fallingTimer > 0.1f)
        {
            moveInput = Vector2.Lerp(moveInput, Vector2.zero, Time.deltaTime * 2);
            animator.Play("player_falling");
        }
        else if (moveInput == Vector2.zero)
        {
            animator.Play("player_idle");
            rb.linearVelocity = Vector3.zero; // Stop the character if there is no movement
        }
        else
        {
            animator.Play("player_walk");
            Vector3 movement3D = new Vector3(moveInput.x, 0, moveInput.y);
            rb.linearVelocity = movement3D * movementSpeed; // Assign velocity instead of MovePosition
        }

        // Apply scaled gravity when not grounded
        if (!grounded)
        {
            float gravityMultiplier = 8f; // Adjust the multiplier as needed
            rb.AddForce(Physics.gravity * (gravityMultiplier - 1), ForceMode.Acceleration);
        }
    }
}

// Helper function to check if the player is grounded
private bool IsGrounded()
{
    float checkRadius = 1.4f; // Adjust based on player size
    float checkDistance = 3.0f; // Adjust based on player height
    Vector3 groundCheckPosition = transform.position + Vector3.down * (checkDistance - checkRadius);
    return Physics.CheckSphere(groundCheckPosition, checkRadius, groundLayer);
}

private void OnDrawGizmos()
{
    float checkRadius = 1.4f; // Adjust based on player size
    float checkDistance = 3.0f; // Adjust based on player height
    Gizmos.color = Color.red;
    Vector3 groundCheckPosition = transform.position + Vector3.down * (checkDistance - checkRadius);

    Gizmos.DrawWireSphere(groundCheckPosition, checkRadius); // Correct position for the sphere
}

private IEnumerator PrintGroundedStatus()
{
    while (true)
    {
        bool isGrounded = IsGrounded();
        if (!isGrounded)
        {
            Debug.Log("Is Grounded: " + isGrounded);
        }
        yield return new WaitForSeconds(0.1f);
    }
}



}
