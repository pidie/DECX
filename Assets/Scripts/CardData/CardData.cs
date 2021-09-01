using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typeOfData
{
    Action,
    Creature,
    Item
}
public class CardData : ScriptableObject
{
    public string title;
    public string ID;

    public string description;
    [TextArea(10,50)]   public string notes;
}
