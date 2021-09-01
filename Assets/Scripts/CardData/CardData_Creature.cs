using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typeOfCreature
{
    Boss,
    Companion,
    Minion,
    Player
}
public class CardData_Creature : CardData
{
    public typeOfData Creature;
    [Header("Creature Health")]
    public int healthPoints;
    public int armorPoints;
    public int shieldPoints;
    public int spellShieldPoints;
    [Header("Creature Actions")]
    public int numOfActions;
    public int energyPoints;
    [Header("Creature Information")]
    public bool isInanimate;
}
