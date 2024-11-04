using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControls : MonoBehaviour
{

    private Animator animator;
    Vector2 moveInput;

    [SerializeField] private float movementSpeed = 5f;
    private Rigidbody rb;

    private PlayerControls playerControls;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>(); 
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        if (playerControls is not null)
            playerControls.Enable();
    }

    private void OnDisable()
    {
        if (playerControls is not null)
            playerControls.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ( playerControls is not null)
        {
            moveInput = playerControls.Player.Move.ReadValue<Vector2>();
            animator.SetFloat("SpeedX", moveInput.x);
            animator.SetFloat("SpeedZ", moveInput.y);
            if (moveInput == Vector2.zero)
            {
                animator.Play("player_idle");
            }
            else
            {
                animator.Play("player_walk");
            }
            Vector3 movement3D = new Vector3(moveInput.x, 0, moveInput.y);
            rb.MovePosition(rb.position + movement3D * (movementSpeed * Time.fixedDeltaTime));
        }
    }
}
