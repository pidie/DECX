using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typeOfAction
{
    Ability,
    Hidden,
    Spell
}
public class CardData_Action : CardData
{
    public typeOfData Action;
    public int energyCost;
    public int cooldownPeriod;

    public bool dealsDamage;
    
    [Header("Summon")]
    public bool summonCreature;
    public CardData_Creature creatureSummoned;
    public int modifyHealth;
}
