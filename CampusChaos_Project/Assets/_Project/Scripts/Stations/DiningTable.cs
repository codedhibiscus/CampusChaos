using UnityEngine;
using Unity.Netcode;

public class DiningTable : MonoBehaviour
{
    [SerializeField] private bool isDirty = false;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        
        if (player != null && player.IsOwner)
        {
            //Does the player actually have their full meal?
            if (isDirty)
            {
                Debug.Log("[Table] This table is dirty! Go find another one.");
                return;
            }
            
            if (player.CheckIfIventoryMatchesTicket())
            {
                Debug.Log("[Table] Order delivered successfully! Scoring points.");

                player.RequestNewTicket();

                //TODO: Trigger the dirty dishes visual loop here
            }
            else
            {
                Debug.Log("[Table] Your order is incomplete! Look closely at your ticket requirements.");
            }
        }
    }
}
