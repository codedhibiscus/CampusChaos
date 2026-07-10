using Unity.Netcode;
using UnityEngine;
using System;
using Unity.VisualScripting;


[Serializable]
public class Ticket
{
    //The required states for this specific ticket
    public bool RequiresMain;
    public bool RequiresSide;
    public bool RequiresDrink;

    public Ticket(bool main, bool side, bool drink)
    {
        RequiresMain = main;
        RequiresSide = side;
        RequiresDrink = drink;
    }

    //Helper that checks if an inventory matches this ticket
    public bool IsOrderComplete (bool hasMain, bool hasSide, bool hasDrink)
    {
        if (RequiresMain && !hasMain) return false;
        if (RequiresSide && !hasSide) return false;
        if (RequiresDrink && !hasDrink) return false;
        return true;
    }
}