using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkConnect : MonoBehaviour
{
    [Header("UI Buttons")] //Header for visual organization in Unity Editor
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;

    [Header("UI Panels")] //Header for visual organization in Unity Editor
    [SerializeField] private GameObject menuPanel; // To hide the menu after connecting
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Hook up the button clicks to our custom functions
        if (hostButton != null) hostButton.onClick.AddListener(OnHostClicked);
        if (joinButton != null) joinButton.onClick.AddListener(OnJoinClicked);
    }

    private void OnHostClicked()
    {
        Debug.Log("Starting Host...");
        NetworkManager.Singleton.StartHost();
        HideMenu();
    }

    private void OnJoinClicked()
    {
        Debug.Log("Starting Client... Attempting to join Host.");
        NetworkManager.Singleton.StartClient();
        HideMenu();
    }

    private void HideMenu()
    {
        // Hides the UI panel so it doesn't block the screen while playing
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        // Clean up listeners when the object is destroyed to prevent memory leaks
        if (hostButton != null) hostButton.onClick.RemoveListener(OnHostClicked);
        if (joinButton != null) joinButton.onClick.RemoveListener(OnJoinClicked);
    }
}
