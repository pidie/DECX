using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Texture imageTexture;
    public string description;
    [TextArea(10,30)]   public string notes;
}
