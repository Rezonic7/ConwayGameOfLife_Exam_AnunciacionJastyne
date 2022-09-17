using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManger : MonoBehaviour, IUI
{
    public Text generationText;
    public int currentGeneration;

    public Text cellsAliveText;

    public void UpdateCurrentGenerationText(int currentGeneration)
    {
        generationText.text = "Current Generation: " + currentGeneration;
    }
    public void UpdateCurrentAliveCellsText(int currentAliveCells)
    {
        cellsAliveText.text = "Currently Alive Cells: " + currentAliveCells;
    }
}
