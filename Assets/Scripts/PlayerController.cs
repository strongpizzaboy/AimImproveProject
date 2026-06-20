using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 movement;
    private float verticalVelocity;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private Transform ground_check;
    [SerializeField] private float ground_distance = 0.4f;
    [SerializeField] private LayerMask ground_mask;

    private bool is_grounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        CheckGround();
        HandleInput();
        ApplyGravity();
        Move();
    }

    void CheckGround()
    {
        if (ground_check)
        {
            is_grounded = Physics.CheckSphere(
                ground_check.position,
                ground_distance,
                ground_mask
            );
        }
        else
        {
            is_grounded = characterController.isGrounded;
        }
    }

    void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        movement = transform.right * horizontalInput + transform.forward * verticalInput;
        movement = movement.normalized * moveSpeed;

        if (Input.GetButtonDown("Jump") && is_grounded)
        {
            verticalVelocity = jumpForce;
        }
    }

    void ApplyGravity()
    {
        if (is_grounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        verticalVelocity -= gravity * Time.deltaTime;
        movement.y = verticalVelocity;
    }

    void Move()
    {
        characterController.Move(movement * Time.deltaTime);
    }
}