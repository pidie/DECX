using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Data", fileName = "New Card Data")]
public class CardData : ScriptableObject
{
    public string title;
    public string ID;
    public int energyCost;

    public string description;
    [TextArea(10,50)]   public string notes;
}
