using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : NetworkBehaviour
{
    [Header("Movement Settings")] //Header for visual organization in Unity Editor
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Dining Hall Dash Game Loop")]
    public bool needsPizza = true;    // The player's random checklist
    public bool needsSalad = true;
    public bool needsBurger = true;
    public bool hasAllFood = false;   // Becomes true when all above are false

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
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 relativeMove = (camForward * moveInput.z) + (camRight * moveInput.x);

            // Rotation - Keep this the same
            Quaternion targetRotation = Quaternion.LookRotation(relativeMove);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            // CHANGE THIS: Instead of MovePosition, we set Velocity
            // This works better with Rigidbody Interpolation
            Vector3 targetVelocity = relativeMove * moveSpeed;
            rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
        }
        else
        {
            // Stop moving when no keys are pressed (optional, prevents sliding)
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    public void CollectFoodItem(string stationName)
    {
        if(!IsOwner) return;
        if (stationName == "Pizza" && needsPizza)
        {
            needsPizza = false;
            Debug.Log($"[Inventory] Grabbed Pizza!");
        }
        else if (stationName == "Salad" && needsSalad)
        {
            needsSalad = false;
            Debug.Log($"[Inventory] Grabbed Salad!");
        }
        else if (stationName == "Burger" && needsBurger)
        {
            needsBurger = false;
            Debug.Log($"[Inventory] Grabbed Burger!");
        }

        // Check if the ticket is cleared
        if (!(needsPizza || needsSalad || needsBurger))
        {
            hasAllFood = true;
            Debug.Log("[Inventory] Order complete! Go find an open Table!");
        }
    }
}