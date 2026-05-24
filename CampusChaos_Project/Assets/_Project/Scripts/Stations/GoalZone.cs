using UnityEngine;
using Unity.Netcode;

public class GoalZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        
        if (player != null)
        {
            //Does the player actually have their full meal?
            if (player.hasAllFood)
            {
                NetworkObject netObj = player.GetComponent<NetworkObject>();
                Debug.Log($"[Table] Success! Player {netObj.OwnerClientId} sat down and ate their meal!");

                // RESET LOOP: Give them a new order ticket!
                player.needsPizza = true;
                player.needsSalad = true;
                player.needsBurger = false;
                player.hasAllFood = false;
                Debug.Log("[Table] New order assigned!: Pizza, Salad");
            }
            else
            {
                Debug.Log("[Table] You can't sit down yet, your tray is missing food!");
            }
        }
    }
}
