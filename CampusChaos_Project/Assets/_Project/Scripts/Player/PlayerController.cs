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

    [Header("Current Order Inventory Status")]
    public bool HasMainCourse = false;
    public bool HasSidesAndSalads = false;
    public bool HasDrinksAndSweets = false;

    [Header("Active Order System")]
    public Ticket ActiveTicket;


    private Rigidbody rb;
    private Vector3 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start runs when the player script is first initialized in the scene
    private void Start()
    {
        // Only print the order for the player sitting at THIS computer
        if (IsOwner)
        {
            RequestNewTicket();
        }
    }

    public void RequestNewTicket()
    {
        HasMainCourse = false;
        HasSidesAndSalads = false;
        HasDrinksAndSweets = false;

        if(TicketManager.Instance != null)
        {
            ActiveTicket = TicketManager.Instance.GenerateRandomTicket();

            //Print the active objective
            string orderString = "Your order ticket: ";
            if (ActiveTicket.RequiresMain) orderString += "[Main Course] ";
            if (ActiveTicket.RequiresSide) orderString += "[Side/Salad] ";
            if (ActiveTicket.RequiresDrink) orderString += "[Drink/Sweet] ";
            Debug.Log($"[UI Ticket Notification] {orderString}");
        }
    }

    /* Checks if the player has collected everything they need */
    public bool CheckIfIventoryMatchesTicket()
    {
        if (ActiveTicket == null) return false;
        return ActiveTicket.IsOrderComplete(HasMainCourse, HasSidesAndSalads, HasDrinksAndSweets);
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

    //Runs when a player runs into any valid food station
    public void CollectFoodFromCategory(FoodStation.StationCategory category)
    {
        if(!IsOwner) return;
        
        switch (category)
        {
            case FoodStation.StationCategory.MainCourse:
                if (!HasMainCourse)
                {
                    HasMainCourse = true;
                    Debug.Log("[Inventory] Added a Main Course item to your tray!");
                }
                break;

            case FoodStation.StationCategory.SidesAndSalads:
                if (!HasSidesAndSalads)
                {
                    HasSidesAndSalads = true;
                    Debug.Log("[Inventory] Added a Side/Salad item to your tray!");
                }
                break;

            case FoodStation.StationCategory.DrinksAndSweets:
                if (!HasDrinksAndSweets)
                {
                    HasDrinksAndSweets = true;
                    Debug.Log("[Inventory] Added a Drink/Sweet item to your tray!");
                }
                break;
        }

        //TODO: Maybe call a ticket tracker check here later to see if the whole order is completed
    }
}