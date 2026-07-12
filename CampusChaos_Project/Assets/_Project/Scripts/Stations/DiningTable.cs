using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class DiningTable : MonoBehaviour
{
    public enum TableType
    {
        OneTop, 
        FourTop,
        SixTop
    }

    [Header("Table Configuration")]
    [SerializeField] private TableType configType;
    [SerializeField] private int tableCapacity = 4;

    [Header("Dirty Tray Mechanics")]
    [SerializeField] private GameObject dirtyTrayPrefab; //TODO: Create this later
    [SerializeField] private Transform traySpawnPoint;
    [SerializeField] private float cleanupCooldown = 5.0f; //Seconds the table stays dirty

    [Header("Runtime State")]
    [SerializeField] private bool isDirty = false;
    private GameObject currentTray;

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
