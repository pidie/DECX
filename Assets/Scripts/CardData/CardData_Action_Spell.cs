using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Data/Action/Spell", fileName = "New Spell")]
public class CardData_Action_Spell : CardData_Action
{
	[HideInInspector]	public typeOfAction TypeOfAction = typeOfAction.Spell;
	// [Header("Spell Information")]
}
