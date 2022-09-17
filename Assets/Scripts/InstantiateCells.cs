using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCells : MonoBehaviour
{
    private CellGraphics cellGraphics;

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject cellHolder;

    [SerializeField] private int _rows;
    [SerializeField] private int _columns;
    
    private SpriteRenderer[,] _matrixSR;

    private CellClass[,] _currentCells;
    private CellClass[,] _nextCells;
    public int Rows { get { return _rows; } }
    public int Columns { get { return _columns; } }
    public SpriteRenderer[,] MatrixSR { get { return _matrixSR; } }
    public CellClass[,] CurrentCells { get { return _currentCells; } }
    public CellClass[,] NextCells { get { return _nextCells; } }

    private void Awake()
    {
        _matrixSR = new SpriteRenderer[Columns, Rows];
        _currentCells = new CellClass[Columns, Rows];
        _nextCells = new CellClass[Columns, Rows];
        for (int ri = 0; ri < Rows; ri++)
        {
            for (int ci = 0; ci < Columns; ci++)
            {
                _currentCells[ci, ri] = new CellClass(false);
                _nextCells[ci, ri] = new CellClass(false);
            }
        }
        Debug.Log("test");
        cellGraphics = GetComponent<CellGraphics>();

        InitalizeMatrix();
        cellGraphics.RefreshGraphics();
    }
    private void InitalizeMatrix()
    {
        for (int ri = 0; ri < Rows; ri++)
        {
            for (int ci = 0; ci < Columns; ci++)
            {
                SpriteRenderer cellSR = Instantiate(cellPrefab, new Vector3(ci, -ri, 0), Quaternion.identity, cellHolder.transform).GetComponent<SpriteRenderer>();
                _matrixSR[ci, ri] = cellSR;
                cellSR.transform.name = ("Cell: Column - " + ci + ", Row - " + ri);
            }
        }
    }
}
