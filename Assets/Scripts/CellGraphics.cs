using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InstantiateCells))]
public class CellGraphics : MonoBehaviour
{
    private IUI iUI;
    private InstantiateCells instantiatedCells;
    private void Awake()
    {
        iUI = GetComponent<IUI>();
        instantiatedCells = GetComponent<InstantiateCells>();
    }
    public void RefreshGraphics()
    {
        int aliveCells = 0;
        for (int ri = 0; ri < instantiatedCells.Rows; ri++)
        {
            for (int ci = 0; ci < instantiatedCells.Columns; ci++)
            {
                if (instantiatedCells.CurrentCells[ci, ri].Alive)
                {
                    instantiatedCells.MatrixSR[ci, ri].color = Color.white;
                    aliveCells += 1;
                }
                else
                {
                    instantiatedCells.MatrixSR[ci, ri].color = Color.black;
                }
            }
        }
        iUI.UpdateCurrentAliveCellsText(aliveCells);
    }
}
