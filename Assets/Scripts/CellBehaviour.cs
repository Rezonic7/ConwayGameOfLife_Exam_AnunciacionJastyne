using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InstantiateCells))]
public class CellBehaviour : MonoBehaviour, IInput
{
    private IUI iUI;
    private InstantiateCells instantiatedCells;
    private CellGraphics cellGraphics;

    private int currentGeneration;

    private void Awake()
    {
        iUI = GetComponent<IUI>();
        instantiatedCells = GetComponent<InstantiateCells>();
        cellGraphics = GetComponent<CellGraphics>();

        currentGeneration = 0;
    }
    public void OnSwitchState()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int ri = 0; ri < instantiatedCells.Rows; ri++)
        {
            for (int ci = 0; ci < instantiatedCells.Columns; ci++)
            {
                if (Vector2.Distance(instantiatedCells.MatrixSR[ci, ri].transform.position, mousePos) <= 0.5f)
                {
                    if (instantiatedCells.CurrentCells[ci, ri].Alive)
                    {
                        instantiatedCells.CurrentCells[ci, ri].Alive = false;
                        cellGraphics.RefreshGraphics();
                    }
                    else
                    {
                        instantiatedCells.CurrentCells[ci, ri].Alive = true;
                        cellGraphics.RefreshGraphics();
                    }
                }
            }
        }
    }
    public void OnNextGeneration()
    {
        for (int ri = 0; ri < instantiatedCells.Rows; ri++)
        {
            for (int ci = 0; ci < instantiatedCells.Columns; ci++)
            {
                if (instantiatedCells.CurrentCells[ci, ri].Alive)
                {
                    if (CountAliveNeighbors(instantiatedCells.CurrentCells, ci, ri) < 2 || (CountAliveNeighbors(instantiatedCells.CurrentCells, ci, ri) > 3))
                    {
                        instantiatedCells.NextCells[ci, ri].Alive = false;
                    }
                    else
                    {
                        instantiatedCells.NextCells[ci, ri].Alive = true;
                    }
                }
                else
                {
                    if (CountAliveNeighbors(instantiatedCells.CurrentCells, ci, ri) == 3)
                    {
                        instantiatedCells.NextCells[ci, ri].Alive = true;
                    }
                    else
                    {
                        instantiatedCells.NextCells[ci, ri].Alive = false;
                    }
                }
            }
        }
        #region Debug
        //string beforeDebug = "";
        //string afterDebug = "";
        //for (int dri = 0; dri < Rows; dri++)
        //{
        //    beforeDebug += "\n";
        //    afterDebug += "\n";
        //    for (int dci = 0; dci < Columns; dci++)
        //    {
        //        if (currentCells[dci, dri].Alive)
        //        {
        //            beforeDebug += "1 ";
        //        }
        //        else
        //        {
        //            beforeDebug += "0 ";
        //        }

        //        if (nextCells[dci, dri].Alive)
        //        {
        //            afterDebug += "1 ";
        //        }
        //        else
        //        {
        //            afterDebug += "0 ";
        //        }
        //    }
        //}
        //Debug.Log("Before Matrix:" + beforeDebug);
        //Debug.Log("After Matrix:" + afterDebug);
        #endregion
        currentGeneration += 1;
        iUI.UpdateCurrentGenerationText(currentGeneration);
        PopulateCurrentCells();
    }
    private void PopulateCurrentCells()
    {
        for (int ri = 0; ri < instantiatedCells.Rows; ri++)
        {
            for (int ci = 0; ci < instantiatedCells.Columns; ci++)
            {
                instantiatedCells.CurrentCells[ci, ri].Alive = instantiatedCells.NextCells[ci, ri].Alive;
            }
        }
        cellGraphics.RefreshGraphics();
    }
    private float CountAliveNeighbors(CellClass[,] cellMatrix, int cols, int rows)
    {
        int sum = 0;
        for (int rii = -1; rii < 2; rii++)
        {
            for (int cii = -1; cii < 2; cii++)
            {
                if (ContainsCell(cols + cii, rows + rii))
                {
                    if (cellMatrix[cols + cii, rows + rii].Alive)
                    {
                        if (rii == 0 && cii == 0)
                        {

                        }
                        else
                        {
                            sum += 1;
                        }
                    }
                }
            }
        }
        return sum;
    }
    private bool ContainsCell(int colOffset, int rowOffset)
    {
        for (int ri = 0; ri < instantiatedCells.Rows; ri++)
        {
            for (int ci = 0; ci < instantiatedCells.Columns; ci++)
            {
                if (colOffset < 0 || colOffset > (instantiatedCells.Columns - 1) || rowOffset < 0 || rowOffset > (instantiatedCells.Rows - 1))
                {
                    return false;
                }

                if (instantiatedCells.CurrentCells[ci, ri] == instantiatedCells.CurrentCells[colOffset, rowOffset])
                {
                    return true;
                }
            }
        }
        return false;
    }
    
}
