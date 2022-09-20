
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(ManagerCellMatrix))]
public class UICellMatrix : MonoBehaviour, IUICellMatrix
{
    [SerializeField] private Text generationText;
    [SerializeField] private Text cellsAliveText;

    public void UpdateCurrentGenerationText(int currentGeneration)
    {
        generationText.text = "Current Generation: " + currentGeneration;
    }
    public void UpdateCurrentAliveCellsText(int currentAliveCells)
    {
        cellsAliveText.text = "Currently Alive Cells: " + currentAliveCells;
    }

}
