using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    
    [Header("Camera Settings")] //Header for visual organization in Unity Editor
    [SerializeField] private float distance = 7.0f;        // Distance away from the player capsule
    [SerializeField] private float heightOffset = 1.5f;    // Looks at torso/head height instead of ground level
    [SerializeField] private float sensitivityX = 3.0f;    // Horizontal mouse speed
    [SerializeField] private float sensitivityY = 2.0f;    // Vertical mouse speed
    
    [SerializeField] private float minYAngle = -20f;       // Prevents the camera from clipping into the ground
    [SerializeField] private float maxYAngle = 80f;        // Stops camera directly overhead

    private float currentX = 0.0f;                         // Tracks horizontal orbit degrees
    private float currentY = 20.0f;                        // Tracks vertical orbit degrees


    void LateUpdate()
    {
        if (target == null)
        {
            FindLocalPlayer();
            return;
        }

        // Only modify orbit angles if the window is active AND the right mouse button is held down
        if (Application.isFocused && Mouse.current != null && Mouse.current.rightButton.isPressed)
        {
            // Read the delta (change in mouse movement) this frame
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            currentX += mouseDelta.x * sensitivityX;
            currentY -= mouseDelta.y * sensitivityY;

            // Enforce camera boundaries
            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        }

        // Convert the tracking degree values into a 3D spherical point around your target
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 targetPosition = target.position + Vector3.up * heightOffset;
        Vector3 desiredPosition = targetPosition - (rotation * Vector3.forward * distance);

        // Snap to calculated orbital position and focus cleanly on the player target point
        transform.position = desiredPosition;
        transform.LookAt(targetPosition);
    }

    private void FindLocalPlayer()
    {
        PlayerController[] players = Object.FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        foreach (PlayerController p in players)
        {
            NetworkObject networkObj = p.GetComponent<NetworkObject>();
            if (networkObj != null && networkObj.IsLocalPlayer)
            {
                target = p.transform;
                return;
            }
        }
    }
}