using UnityEngine;
using Unity.Netcode;

public class FoodStation : MonoBehaviour
{
    [SerializeField] private string stationFoodType; // Set to the specific type in Inspector

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            // Hand the food to the player script
            player.CollectFoodItem(stationFoodType);
        }
    }
}
