using Unity.Netcode;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    
    [Header("Positioning")] //Header for visual organization in Unity Editor
    [SerializeField] private Vector3 offset = new Vector3(0, 4, -6); // Up 4 units, Back 6 units
    [SerializeField] private float smoothSpeed = 0.125f;
    private Vector3 currentVelocity = Vector3.zero;
    

    // LateUpdate runs AFTER the player has moved, preventing the camera from "jittering"
    void LateUpdate()
    {
        //Find the Player if we don't have one yet
        if (target == null)
        {
            FindLocalPlayer();
            return;
        }

        
        //Calculate the position based on the player's current rotation
        Vector3 desiredPosition = target.position + (target.rotation * offset);
        
        //Smoothly slide the camera toward that goal
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);

        //Always look at the player's center
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }

    private void FindLocalPlayer()
    {
        PlayerController[] players = Object.FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        foreach (PlayerController p in players)
        {
            // Only follow the player that belongs to THIS computer
            NetworkObject networkObj = p.GetComponent<NetworkObject>();
            if (networkObj != null && networkObj.IsLocalPlayer)
            {
                target = p.transform;
                return;
            }
        }
    }
}