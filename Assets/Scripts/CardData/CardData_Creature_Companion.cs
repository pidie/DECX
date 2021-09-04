using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Data/Creature/Companion", fileName = "New Companion")]
public class CardData_Creature_Companion : CardData_Creature
{
    [HideInInspector]   public typeOfCreature TypeOfCreature = typeOfCreature.Companion;

    [Header("Companion Information")] 
    public int companionLevel = 1;
    public int experiencePoints;
}
