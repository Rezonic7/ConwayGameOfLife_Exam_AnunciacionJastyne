using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerCellMatrix : MonoBehaviour, IManagerCellMatrix, IInteractable, IChangeableSettingsCellMatrix
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform cellParent;

    private int rows;
    private int columns;
    private float generationSpeed;

    private int totalCellGenerations;
    private int aliveCells;

    private Color aliveColor;
    private Color deadColor;

    private bool continousNextGeneration;
    public bool isPaused;

    private Cell[,] currentGenerationCells;

    private CellClass[,] nextGenerationCells;

    private IInstantiateCellMatrix iInstantiateCellMatrix;
    private IBehaviourCellMatrix iBehaviourCellMatrix;
    private IResizeCamera iResizeCamera;
    private IUICellMatrix iUICellMatrix;

    private void Awake()
    {
        iInstantiateCellMatrix = GetComponent<IInstantiateCellMatrix>();
        iBehaviourCellMatrix = GetComponent<IBehaviourCellMatrix>();
        iUICellMatrix = GetComponent<IUICellMatrix>();

        foreach (GameObject gameObjects in FindObjectsOfType<GameObject>())
        {
            if (gameObjects.GetComponent<IResizeCamera>() != null)
                iResizeCamera = gameObjects.GetComponent<IResizeCamera>();
        }
    }

    public void SetVariables(Color AliveColor, Color DeadColor, int Columns, int Rows, float GenerationSpeed)
    {
        columns = Columns;
        rows = Rows;

        aliveColor = AliveColor;
        deadColor = DeadColor;

        generationSpeed = GenerationSpeed;

        InstantiateNewCellMatrix();
    }

    public void InstantiateNewCellMatrix()
    {
        currentGenerationCells = new Cell[columns, rows];
        nextGenerationCells = new CellClass[columns, rows];

        totalCellGenerations = 0;
        aliveCells = 0;
        iUICellMatrix.UpdateCurrentGenerationText(totalCellGenerations);

        continousNextGeneration = false;

        for (int i = 0; i < cellParent.transform.childCount; i++)
        {
            Destroy(cellParent.transform.GetChild(i).gameObject);
        }

        iInstantiateCellMatrix.Instantiate(columns, rows, cellPrefab, cellParent);

        foreach (Cell cells in currentGenerationCells)
        {
            cells.ChangeStateColors(aliveColor, deadColor);
            cells.ChangeColorDependingOnState();
        }

        iResizeCamera.ResizeCamera(columns, rows);
    }

    public void Interaction(string interaction)
    {
        if(interaction == "Interact1" && !continousNextGeneration)
        {
            MoveGenerationOnce();
        }
        else if (interaction == "Interact2")
        {
            MoveGenerationContinous();
        }
        else if (interaction == "Interact3")
        {
            Restart();
        }
    }
    private void MoveGenerationOnce()
    {
        iBehaviourCellMatrix.Behaviour(columns, rows, currentGenerationCells, nextGenerationCells);
        totalCellGenerations += 1;
        iUICellMatrix.UpdateCurrentGenerationText(totalCellGenerations);
    }
    public void InitializeCells(Cell[,] currentCells)
    {
        this.currentGenerationCells = currentCells;

        for (int ri = 0; ri < rows; ri++)
        {
            for (int ci = 0; ci < columns; ci++)
            {
                nextGenerationCells[ci, ri] = new CellClass(false);
            }
        }
    }

    public void UpdateCell(Cell cellToUpdate)
    {
        for (int ri = 0; ri < rows; ri++)
        {
            for (int ci = 0; ci < columns; ci++)
            {
                if (currentGenerationCells[ci, ri] == cellToUpdate)
                {
                    currentGenerationCells[ci, ri].CellClass.Alive = cellToUpdate.CellClass.Alive;
                }
            }

        }
        #region Debug
        //string beforeDebug = "";
        //string afterDebug = "";
        //for (int dri = 0; dri < rows; dri++)
        //{
        //    beforeDebug += "\n";
        //    afterDebug += "\n";
        //    for (int dci = 0; dci < columns; dci++)
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
    }

    private void Restart()
    {
        foreach(Cell cells in currentGenerationCells)
        {
            cells.CellClass.Alive = false;
            cells.ChangeColorDependingOnState();

            aliveCells = 0;
            totalCellGenerations = 0;
            iUICellMatrix.UpdateCurrentAliveCellsText(aliveCells);
            iUICellMatrix.UpdateCurrentGenerationText(totalCellGenerations);
        }
    }

    public void UpdateCurrentMatix(Cell[,] cells)
    {
        currentGenerationCells = cells;

        #region Debug
        //string beforeDebug = "";
        //string afterDebug = "";
        //for (int dri = 0; dri < rows; dri++)
        //{
        //    beforeDebug += "\n";
        //    afterDebug += "\n";
        //    for (int dci = 0; dci < columns; dci++)
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
        aliveCells = 0;
        foreach (Cell cell in currentGenerationCells)
        {
            cell.ChangeColorDependingOnState();
            if(cell.CellClass.Alive)
            {
                aliveCells += 1;
            }
        }
        iUICellMatrix.UpdateCurrentAliveCellsText(aliveCells);
    }

    public void IncrementAliveCells(int amount)
    {
        aliveCells += amount;
        iUICellMatrix.UpdateCurrentAliveCellsText(aliveCells);
    }

    private void MoveGenerationContinous()
    {
        if (continousNextGeneration)
        {
            StopAllCoroutines();
            continousNextGeneration = false;
        }
        else
        {
            StartCoroutine(PlayContinousGeneration());
            continousNextGeneration = true;
        }
    }
    IEnumerator PlayContinousGeneration()
    {
        yield return new WaitForSeconds(generationSpeed);
        MoveGenerationOnce();
        StartCoroutine(PlayContinousGeneration());
    }

    public bool IsContinousGeneration()
    {
        return continousNextGeneration;
    }
    public bool IsPaused()
    {
        return isPaused;
    }
    public void Pause()
    {
        if (isPaused)
        {
            isPaused = false;
        }
        else
        {
            isPaused = true;
        }
    }
}
