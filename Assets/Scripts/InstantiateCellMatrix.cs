using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCellMatrix : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject cellHolder;

    [SerializeField] private int _rows;
    [SerializeField] private int _columns;

    private Cell[,] _cells;
    
    public int Rows { get { return _rows; } }
    public int Columns { get { return _columns; } }
    public Cell[,] Cells { get { return _cells; } }

    private void Awake()
    {
        _cells = new Cell[_columns, _rows];

        InitalizeMatrix();

        foreach (Cell cells in _cells)
        {
            cells.ChangeColorDependingOnState();
        }
    }
    private void InitalizeMatrix()
    {
        for (int ri = 0; ri < Rows; ri++)
        {
            for (int ci = 0; ci < Columns; ci++)
            {
                Cell cell = Instantiate(cellPrefab, new Vector3(ci, -ri, 0), Quaternion.identity, cellHolder.transform).GetComponent<Cell>();
                cell.transform.name = ("Cell: Column - " + ci + ", Row - " + ri);
                _cells[ci, ri] = cell;
            }
        }
    }
}
