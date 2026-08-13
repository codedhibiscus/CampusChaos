using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.VisualScripting;

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
    [SerializeField] private string specificStationName = "Pizza Station"; //"Grill", etc.
    [SerializeField] private string specificItemName = "Pepperoni Pizza Slice";


    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI stationTextLabel; //child text component will be here inside prefab


    private void Awake()
    {
        //Auto overwrite the label at runtime with custom station name
        if (stationTextLabel != null) stationTextLabel.text = specificStationName.ToUpper();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            // Hand the food to the player script
            player.CollectFoodFromCategory(stationFoodType, specificItemName);
        }
    }
}
