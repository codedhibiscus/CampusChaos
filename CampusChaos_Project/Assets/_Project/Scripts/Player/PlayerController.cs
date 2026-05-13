using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : NetworkBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private Rigidbody rb;
    private Vector3 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // FixedUpdate is best for physics-based movement (Rigidbody)
    private void FixedUpdate()
    {
        //Only move the character if WE are the one controlling it
        if (!IsOwner) return;

        MovePlayer();
    }

    private void Update()
    {
        if (!IsOwner) return;
        
        //Check the keyboard state directly for WASD or Arrow Keys
        Vector2 inputVector = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) inputVector.y = 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) inputVector.y = -1;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) inputVector.x = -1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) inputVector.x = 1;
        }

        moveInput = new Vector3(inputVector.x, 0, inputVector.y).normalized;
    }

    private void MovePlayer()
    {
        if (moveInput.magnitude >= 0.1f)
        {
            // Move the Rigidbody
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

            // Rotate to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}