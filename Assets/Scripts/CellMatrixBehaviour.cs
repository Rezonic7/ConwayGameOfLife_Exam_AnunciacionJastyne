using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InstantiateCellMatrix))]
public class CellMatrixBehaviour : MonoBehaviour, IInputReciever, ICallableCellMatrix
{
    private IUI iUI;
    private InstantiateCellMatrix instantiatedCells;

    private int currentIntGeneration;

    private CellClass[,] _currentCellGeneration;
    private CellClass[,] nextCellGeneration;

    public CellClass[,] CurrentCellGeneration { get { return _currentCellGeneration; } set { _currentCellGeneration = value; } }

    private void Awake()
    {
        iUI = GetComponent<IUI>();
        instantiatedCells = GetComponent<InstantiateCellMatrix>();

        _currentCellGeneration = new CellClass[instantiatedCells.Columns, instantiatedCells.Rows];
        nextCellGeneration = new CellClass[instantiatedCells.Columns, instantiatedCells.Rows];

       
        currentIntGeneration = 0;
    }

    public void Start()
    {
        for (int ri = 0; ri < instantiatedCells.Rows; ri++)
        {
            for (int ci = 0; ci < instantiatedCells.Columns; ci++)
            {
                _currentCellGeneration[ci,ri] = instantiatedCells.Cells[ci,ri].CellClass;
                nextCellGeneration[ci,ri] = instantiatedCells.Cells[ci,ri].CellClass;
            }
        }

    }
    public void MoveToNextGeneration()
    {
    for (int ri = 0; ri < instantiatedCells.Rows; ri++)
    {
        for (int ci = 0; ci < instantiatedCells.Columns; ci++)
        {
            if (_currentCellGeneration[ci, ri].Alive)
            {
                if (CountAliveNeighbors(_currentCellGeneration, ci, ri) < 2 || (CountAliveNeighbors(_currentCellGeneration, ci, ri) > 3))
                {
                    nextCellGeneration[ci, ri].Alive = false;
                }
                else
                {
                    nextCellGeneration[ci, ri].Alive = true;
                }
            }
            else
            {
                if (CountAliveNeighbors(_currentCellGeneration, ci, ri) == 3)
                {
                    nextCellGeneration[ci, ri].Alive = true;
                }
                else
                {
                    nextCellGeneration[ci, ri].Alive = false;
                }
            }
        }
    }
    #region Debug
    string beforeDebug = "";
    string afterDebug = "";
    for (int dri = 0; dri < instantiatedCells.Rows; dri++)
    {
        beforeDebug += "\n";
        afterDebug += "\n";
        for (int dci = 0; dci < instantiatedCells.Columns; dci++)
        {
            if (_currentCellGeneration[dci, dri].Alive)
            {
                beforeDebug += "1 ";
            }
            else
            {
                beforeDebug += "0 ";
            }

            if (nextCellGeneration[dci, dri].Alive)
            {
                afterDebug += "1 ";
            }
            else
            {
                afterDebug += "0 ";
            }
        }
    }
    Debug.Log("Before Matrix:" + beforeDebug);
    Debug.Log("After Matrix:" + afterDebug);
    #endregion
    currentIntGeneration += 1;

    iUI.UpdateCurrentGenerationText(currentIntGeneration);
    PopulateCurrentCells();
    }
    public void ChangeState(Cell cellToChange)
    {
        UpdateCellOnMatrix(cellToChange);
    }
    public void UpdateCellOnMatrix(Cell cellToUpdate)
    {
        for (int ri = 0; ri < instantiatedCells.Rows; ri++)
        {
            for (int ci = 0; ci < instantiatedCells.Columns; ci++)
            {
                if (_currentCellGeneration[ci, ri] == cellToUpdate.CellClass)
                {
                    Debug.Log(cellToUpdate.gameObject.name);
                    Debug.Log(cellToUpdate.CellClass.Alive);
                    Debug.Log(_currentCellGeneration[ci, ri].Alive);
                    Debug.Log(nextCellGeneration[ci, ri].Alive);


                    _currentCellGeneration[ci, ri].Alive = cellToUpdate.CellClass.Alive;
                    Debug.Log(cellToUpdate.CellClass.Alive);
                    Debug.Log(nextCellGeneration[ci, ri].Alive);



                    string beforeDebug = "";
                    string afterDebug = "";
                    for (int dri = 0; dri < instantiatedCells.Rows; dri++)
                    {
                        beforeDebug += "\n";
                        afterDebug += "\n";
                        for (int dci = 0; dci < instantiatedCells.Columns; dci++)
                        {
                            if (_currentCellGeneration[dci, dri].Alive)
                            {
                                beforeDebug += "1 ";
                            }
                            else
                            {
                                beforeDebug += "0 ";
                            }

                            if (nextCellGeneration[dci, dri].Alive)
                            {
                                afterDebug += "1 ";
                            }
                            else
                            {
                                afterDebug += "0 ";
                            }
                        }
                    }
                    Debug.Log("Before Matrix:" + beforeDebug);
                    Debug.Log("After Matrix:" + afterDebug);

                }
            }
        }
    }
    private void PopulateCurrentCells()
    {
        for (int ri = 0; ri < instantiatedCells.Rows; ri++)
        {
            for (int ci = 0; ci < instantiatedCells.Columns; ci++)
            {
                _currentCellGeneration[ci, ri].Alive = nextCellGeneration[ci, ri].Alive;
            }
        }
        ChangeEachCellGraphics();

        //cellGraphics.RefreshGraphics();
    }

    private void ChangeEachCellGraphics()
    {
        
    }
    private float CountAliveNeighbors(CellClass[,] cellGeneration, int cols, int rows)
    {
        int sum = 0;
        for (int rii = -1; rii < 2; rii++)
        {
            for (int cii = -1; cii < 2; cii++)
            {
                if (IsInBoundsOfCellMatrix(cols + cii, rows + rii))
                {
                    if (cellGeneration[cols + cii, rows + rii].Alive)
                    {
                        if (rii == 0 && cii == 0)
                        {
                            continue;
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
    private bool IsInBoundsOfCellMatrix(int colOffset, int rowOffset)
    {
        for (int ri = 0; ri < instantiatedCells.Rows; ri++)
        {
            for (int ci = 0; ci < instantiatedCells.Columns; ci++)
            {
                if (colOffset < 0 || colOffset > (instantiatedCells.Columns - 1) || rowOffset < 0 || rowOffset > (instantiatedCells.Rows - 1))
                {
                    return false;
                }

                if (_currentCellGeneration[ci, ri] == _currentCellGeneration[colOffset, rowOffset])
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void InputRecieved()
    {
        MoveToNextGeneration();
    }

    
}
