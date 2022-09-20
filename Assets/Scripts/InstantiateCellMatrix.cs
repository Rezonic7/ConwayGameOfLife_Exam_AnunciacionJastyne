using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ManagerCellMatrix))]
public class InstantiateCellMatrix : MonoBehaviour, IInstantiateCellMatrix
{
    private IManagerCellMatrix iManagerCellMatrix;

    private void Awake()
    {
        iManagerCellMatrix = GetComponent<IManagerCellMatrix>();
    }
    public void Instantiate(int Columns, int Rows, GameObject cellPrefab, Transform cellParent)
    {
        InstantiateMatrix(Columns, Rows, cellPrefab, cellParent);
    }
    private void InstantiateMatrix(int Columns, int Rows, GameObject cellPrefab, Transform cellParent)
    {
        Cell[,] instantiatedCells = new Cell[Columns, Rows];
        for (int ri = 0; ri < Rows; ri++)
        {
            for (int ci = 0; ci < Columns; ci++)
            {
                Cell cell = Instantiate(cellPrefab, new Vector3(ci, -ri, 0), Quaternion.identity, cellParent).GetComponent<Cell>();
                cell.transform.name = ("Cell: Column - " + ci + ", Row - " + ri);
                instantiatedCells[ci, ri] = cell;
            }
        }
        iManagerCellMatrix.InitializeCells(instantiatedCells);
    }
}
