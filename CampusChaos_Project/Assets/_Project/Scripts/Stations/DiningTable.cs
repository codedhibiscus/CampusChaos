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
            //Is the table currently dirty?
            if (isDirty)
            {
                Debug.Log("[Table] This table is dirty! Go find another one.");
                return;
            }
            
            //Does the player's order match the ticket
            if (player.CheckIfIventoryMatchesTicket())
            {
                Debug.Log("[Table] Order delivered successfully! Scoring points.");

                player.RequestNewTicket();


                TriggerDirtyDishesCooldown(); //Triggers the lockout loop
            }
            else
            {
                Debug.Log("[Table] Your order is incomplete! Look closely at your ticket requirements.");
            }
        }
    }

    private void TriggerDirtyDishesCooldown()
    {
        isDirty = true;

        //Spawn placeholder trash if an asset is assigned
        if (dirtyTrayPrefab != null)
        {
            Vector3 spawnPos = traySpawnPoint != null ? traySpawnPoint.position : transform.position + Vector3.up * 0.5f;
            Quaternion spawnRot = traySpawnPoint != null ? traySpawnPoint.rotation : Quaternion.identity;

            currentTray = Instantiate(dirtyTrayPrefab, spawnPos, spawnRot);
        }

        //Start the cooldown to cleanup table
        StartCoroutine(CleanupTableRoutine());
    }

    private IEnumerator CleanupTableRoutine()
    {
        yield return new WaitForSeconds(cleanupCooldown);

        if(currentTray != null) Destroy(currentTray);
        isDirty = false;
        Debug.Log($"[Table Reset] {gameObject.name} has been cleared and is ready for new people!");
    }
}
