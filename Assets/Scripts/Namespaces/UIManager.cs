using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

enum GameError
{
	CardPositionNotEmpty,
	PlayerNotEnoughEnergy
}
namespace UIManager
{
	static class UIErrorMessage
	{
		public static void DisplayErrorMessage(GameError message, float time = 2.0f)
		{
			string msg = CompileErrorMessage(message);
			TMP_Text errorMessageBox = GameObject.Find("ErrorMessage").GetComponent<TMP_Text>();
			float alpha = errorMessageBox.alpha;
			errorMessageBox.text = msg;
		}

		private static string CompileErrorMessage(GameError message)
		{
			switch (message)
			{
				case GameError.CardPositionNotEmpty:
					return "Cannot play this card here";
				case GameError.PlayerNotEnoughEnergy:
					return "You do not have enough energy to play this card";
				default:
					return "default_error_message";
			}
		}
	}
}