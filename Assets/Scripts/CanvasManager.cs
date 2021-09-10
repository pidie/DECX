using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DECX.UIManager;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public Button drawCardButton;
    public TMP_Text errorMessage;

    [Header("Player Data")] 
    public TMP_Text energyPointsDisplay;

    [Header("Images")] 
    public Image energyPointsBackgroundImage;
    public Texture energyPointsBackgroundIcon;

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
