using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UIManager
{
	static class UIErrorMessage
	{
		public static void DisplayErrorMessage(string message, float time = 2.0f)
		{
			TMP_Text errorMessageBox = GameObject.Find("ErrorMessage").GetComponent<TMP_Text>();
			errorMessageBox.text = message;
		}
	}
}