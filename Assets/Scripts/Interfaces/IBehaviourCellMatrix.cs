using UnityEngine;
public interface IBehaviourCellMatrix
{
    void Behaviour(int Columns, int Rows, Cell[,] currentGenerationCell, CellClass[,] nextGenerationCell);
}

