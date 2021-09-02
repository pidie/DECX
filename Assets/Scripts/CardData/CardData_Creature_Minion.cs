using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Data/Creature/Minion", fileName = "New Minion")]
public class CardData_Creature_Minion : CardData_Creature
{
    [HideInInspector]   public typeOfCreature TypeOfCreature = typeOfCreature.Minion;

    [Header("Minion Information")] 
    public bool isBanished;
}
