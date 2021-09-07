using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int currentEnergyPoints;
    public int maxEnergyPoints;
    public int maxNumberOfCompanions;

    public TMP_Text EnergyPoints;

    private void Awake()
    {
        maxEnergyPoints = 4;
        currentEnergyPoints = maxEnergyPoints;
        maxNumberOfCompanions = 2;
    }

    private void Update()
    {
        EnergyPoints.text = FormatEnergyPoints();
    }

    private string FormatEnergyPoints()
    {
        return $"{currentEnergyPoints} / {maxEnergyPoints}";
    }
}
