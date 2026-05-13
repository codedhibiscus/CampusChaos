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
            //Get the camera's forward and right vectors
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            //Flatten them so the player doesn't try to walk "up" into the sky
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            //Calculate the new movement direction relative to the camera
            Vector3 relativeMove = (camForward * moveInput.z) + (camRight * moveInput.x);

            //Rotate the player to face this new relative direction
            Quaternion targetRotation = Quaternion.LookRotation(relativeMove);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            //Move the player in that relative direction
            rb.MovePosition(rb.position + relativeMove * moveSpeed * Time.fixedDeltaTime);
        }
    }
}