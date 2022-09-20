using UnityEngine;

[RequireComponent(typeof(ManagerCellMatrix))]
public class BehaviourCellMatrix : MonoBehaviour, IBehaviourCellMatrix
{
    private IManagerCellMatrix iManagerCellMatrix;

    private void Awake()
    {
        iManagerCellMatrix = GetComponent<IManagerCellMatrix>();
    }
    public void Behaviour(int Columns, int Rows, Cell[,] currentGenerationCells, CellClass[,] nextGenerationCells)
    {
        MoveToNextGeneration(Columns, Rows, currentGenerationCells, nextGenerationCells);
    }

    public void MoveToNextGeneration(int Columns, int Rows, Cell[,] currentGenerationCells, CellClass[,] nextGenerationCells)
    {
        for (int ri = 0; ri < Rows; ri++)
        {
            for (int ci = 0; ci < Columns; ci++)
            {
                int noOfNeighbors = CountAliveNeighbors(currentGenerationCells, ci, ri, Columns, Rows);

                if (currentGenerationCells[ci, ri].CellClass.Alive)
                {
                    if (noOfNeighbors < 2 || (noOfNeighbors > 3))
                    {
                        nextGenerationCells[ci, ri].Alive = false;
                    }
                    else
                    {
                        nextGenerationCells[ci, ri].Alive = true;
                    }
                }
                else
                {
                    if (noOfNeighbors == 3)
                    {
                        nextGenerationCells[ci, ri].Alive = true;
                    }
                    else
                    {
                        nextGenerationCells[ci, ri].Alive = false;
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
        //        if (currentGenerationCells[dci, dri].CellClass.Alive)
        //        {
        //            beforeDebug += "1 ";
        //        }
        //        else
        //        {
        //            beforeDebug += "0 ";
        //        }

        //        if (nextGenerationCells[dci, dri].Alive)
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

        PopulateCurrentCells(Columns, Rows, currentGenerationCells, nextGenerationCells);
    }
    
    private void PopulateCurrentCells(int Columns, int Rows, Cell[,] currentGenerationCells, CellClass[,] nextGenerationCells)
    {
        for (int ri = 0; ri < Rows; ri++)
        {
            for (int ci = 0; ci < Columns; ci++)
            {
                currentGenerationCells[ci, ri].CellClass.Alive = nextGenerationCells[ci, ri].Alive;
            }
        }
        iManagerCellMatrix.UpdateCurrentMatix(currentGenerationCells);
    }
    private int CountAliveNeighbors(Cell[,] cellGeneration, int currentCol, int currentRow, int maxCol, int maxRow)
    {
        int sum = 0;
        for (int rii = -1; rii < 2; rii++)
        {
            for (int cii = -1; cii < 2; cii++)
            {
                if (IsInBoundsOfCellMatrix(maxCol, maxRow, (currentCol + cii), (currentRow + rii)))
                {
                    if (cellGeneration[currentCol + cii, currentRow + rii].CellClass.Alive)
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
    private bool IsInBoundsOfCellMatrix(int maxCol, int maxRow, int neighborCol, int neighborRow)
    {
        if (neighborCol < 0 || neighborCol > (maxCol - 1) || neighborRow < 0 || neighborRow > (maxRow - 1))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
