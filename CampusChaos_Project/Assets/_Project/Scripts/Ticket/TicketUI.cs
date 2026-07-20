using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TicketUI : MonoBehaviour
{
    public static TicketUI Instance {get; private set;}

    [Header("Ticket Container UI")]
    [SerializeField] private GameObject TicketPanel;

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
}