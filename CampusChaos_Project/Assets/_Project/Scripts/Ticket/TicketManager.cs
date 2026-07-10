using UnityEngine;

public class TicketManager : MonoBehaviour
{
    public static TicketManager Instance { get; private set; }

    private void Awake()
    {
        //Singleton for a quick reference by players and tables
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /* Generates a completely randomized ticket requires 1, 2, or all 3 food groups. */
    public Ticket GenerateRandomTicket()
    {
        bool main = false;
        bool side = false;
        bool drink = false;

        //Ensure at least ONE item is picked so an empty ticket isn't generated
        while (!main && !side && !drink)
        {
            main = Random.value > 0.4f; //60% chance
            side = Random.value > 0.5f; //50% chance
            drink = Random.value > 0.5f; //50% chance
        }

        Ticket newTicket = new Ticket(main, side, drink);

        Debug.Log($"[TicketManager] New Ticket Generated! Main: {main}, Side: {side}, Drink: {drink}");
        return newTicket;
    }
}