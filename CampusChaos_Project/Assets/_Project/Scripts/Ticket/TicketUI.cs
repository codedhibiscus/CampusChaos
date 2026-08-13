using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class TicketUI : MonoBehaviour
{
    public static TicketUI Instance {get; private set;}

    [Header("Ticket Container UI")]
    [SerializeField] private GameObject ticketPanel;

    [Header("Item Requirement Rows")]
    [SerializeField] private GameObject mainRow;
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private Image mainCheckmark;

    [SerializeField] private GameObject sideRow;
    [SerializeField] private TextMeshProUGUI sideText;
    [SerializeField] private Image sideCheckmark;

    [SerializeField] private GameObject drinkRow;
    [SerializeField] private TextMeshProUGUI drinkText;
    [SerializeField] private Image drinkCheckmark;

    [Header("UI Visual Configuration")]
    [SerializeField] private Color itemPendingColor = Color.white;
    [SerializeField] private Color itemCollectedColor = Color.green;

    [Header("Pickup Popup UI")]
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private float notificationDuration = 2.0f;

    private Coroutine notificationCoroutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /* Refresh the HUD panel to match the player's active requirements*/
    public void UpdateTicketDisplay (Ticket ticket, bool hasMain, bool hasSide, bool hasDrink)
    {
        if (ticket == null)
        {
            if (ticketPanel != null) ticketPanel.SetActive(false);
            return;
        }

        if (ticketPanel != null) ticketPanel.SetActive(true);

        //Update the Main Course Row
        if (mainRow != null)
        {
            mainRow.SetActive(ticket.RequiresMain);
            if(ticket.RequiresMain) SetRowState(mainText, mainCheckmark, hasMain);
        }

        if (sideRow != null)
        {
            sideRow.SetActive(ticket.RequiresSide);
            if(ticket.RequiresSide) SetRowState(sideText, sideCheckmark, hasSide);
        }

        if (drinkRow != null)
        {
            drinkRow.SetActive(ticket.RequiresDrink);
            if(ticket.RequiresDrink) SetRowState(drinkText, drinkCheckmark, hasDrink);
        }
    }

    private void SetRowState(TextMeshProUGUI label, Image checkmark, bool isCollected)
    {
        if (label != null)
        {
            label.color = isCollected ? itemCollectedColor : itemPendingColor;
        }

        if (checkmark != null)
        {
            checkmark.enabled = isCollected;
        }
    }

    public void ShowPickupNotification(string message)
    {
        if (notificationText == null) return;
        if (notificationCoroutine != null) StopCoroutine(notificationCoroutine);
    
        notificationCoroutine = StartCoroutine(DisplayNotificationRoutine(message));
    }

    private System.Collections.IEnumerator DisplayNotificationRoutine(string message)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);

        yield return new WaitForSeconds(notificationDuration);

        notificationText.gameObject.SetActive(false);
    }
}