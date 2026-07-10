using UnityEngine;
using Unity.Netcode;

public class FoodStation : MonoBehaviour
{
    public enum StationCategory
    {
        MainCourse,
        SidesAndSalads,
        DrinksAndSweets
    }

    [Header("Station Settings")]
    [SerializeField] private StationCategory stationFoodType; // Set to the specific type in Inspector
    [SerializeField] private string specificStationName; //"Grill", etc.

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            // Hand the food to the player script
            player.CollectFoodFromCategory(stationFoodType);
        }
    }
}
