using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DECX.UIManager;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public TMP_Text errorMessage;

    private void Awake()
    {
        errorMessage.text = "";
    }

    private void Update()
    {
        // Debug.Log("VAR");
        HUD.FadeErrorMessages(errorMessage);
    }
}
