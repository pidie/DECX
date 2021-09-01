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
    public typeOfData TypeOfData = typeOfData.Action;
    public int energyCost;
    public int cooldownPeriod;

    [Header("Damage")]
    public bool dealsDamage;
    public int baseDamage;
    public int damageOverTime;
    public int damageOverTimeDuration;

    [Header("Heal")] 
    public bool heals;
    public int baseHeal;
    public int healsOverTime;
    public int healsOverTimeDuration;
    
    [Header("Summon")]
    public bool summonCreature;
    public CardData_Creature creatureSummoned;
    public int modifyHealth;
    public int modifyDamage;
}
