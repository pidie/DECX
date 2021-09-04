using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typeOfAction
{
    Ability,
    Spell
}
public class CardData_Action : CardData
{
    [HideInInspector]   public typeOfData TypeOfData = typeOfData.Action;

    [Header("Conditions")] 
    public bool isNPCOnly;
    public bool isHidden;
    public bool isSelfCast;

    [Header("Action Informaion")]
    public int energyCost;
    public int cooldownPeriod;
    public int range;

    [Header("Melee Information")]
    public bool isMelee;
    public bool hasReach;

    [Header("Ranged Information")] 
    public bool isRanged;
    [Range(0, 10)]  public int minRange = 1;

    [Header("AOE Information")] 
    public bool isAOE;
    public bool AOERange;

    [Header("Multitarget Information")] 
    public bool isMultitarget;
    public int minNumOfTargets;
    public int maxNumOfTargets;
    
    [Header("Damage Information")]
    public bool dealsDamage;
    public int baseDamage;
    public int damageOverTime;
    public int damageOverTimeDuration;

    [Header("Heal Information")] 
    public bool heals;
    public int baseHeal;
    public int healsOverTime;
    public int healsOverTimeDuration;
    
    [Header("Summon Information")]
    public bool summonCreature;
    public CardData_Creature creatureSummoned;
    public int modifyHealth;
    public int modifyDamage;
}
