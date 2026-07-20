using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
}