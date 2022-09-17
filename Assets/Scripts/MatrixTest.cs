using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixTest : MonoBehaviour
{
    private SpriteRenderer[,] matrixSR = new SpriteRenderer[5,5];//



    [SerializeField] private int Rows;//
    [SerializeField] private int Columns;//
    [SerializeField] private GameObject cellPrefab;//
    [SerializeField] private GameObject cellHolder;//
    [SerializeField] private Camera sceneCamera;

    bool constantPlay = false;//

    private CellClass[,] currentCells = new CellClass[5,5];//
    private CellClass[,] nextCells = new CellClass[5,5];//

    private void Awake()
    {
        matrixSR = new SpriteRenderer[Columns, Rows];//
        currentCells = new CellClass[Columns, Rows];//
        nextCells = new CellClass[Columns, Rows];//
        for (int ri = 0; ri < Rows; ri++)//
        {
            for (int ci = 0; ci < Columns; ci++)//
            {
                currentCells[ci, ri] = new CellClass(false);//
                nextCells[ci, ri] = new CellClass(false);//
            }
        }
    }
    void Start()
    {
        //Instantiate Graphics
        InitalizeMatrix();//

        //Refresh Graphics according to currentCells States
        RefreshUI();//
    }
    private void InitalizeMatrix()
    {
        sceneCamera.orthographicSize = (float)Rows / 2;
        sceneCamera.transform.position = new Vector3(((float)Columns / 2)-1, ((float)-Rows / 2)+0.5f, -10);
        for (int ri = 0; ri < Rows; ri++)
        {
            for (int ci = 0; ci < Columns; ci++)
            {
                SpriteRenderer cellGO = Instantiate(cellPrefab, new Vector3(ci, -ri, 0), Quaternion.identity, cellHolder.transform).GetComponent<SpriteRenderer>();
                matrixSR[ci, ri] = cellGO;
                cellGO.transform.name = ("Cell: Column - " + ci + ", Row - " + ri);
            }
        }
    }//
    private void GoNextState()//
    {
        for (int ri = 0; ri < Rows; ri++)
        {
            for (int ci = 0; ci < Columns; ci++)
            {
                if (currentCells[ci, ri].Alive)
                {
                    if(CountAliveNeighbors(currentCells, ci, ri) < 2 || (CountAliveNeighbors(currentCells, ci, ri) > 3))
                    {
                        nextCells[ci, ri].Alive = false;
                    }
                    else
                    {
                        nextCells[ci, ri].Alive = true;
                    }
                }
                else
                {
                    if (CountAliveNeighbors(currentCells, ci, ri) == 3)
                    {
                        nextCells[ci, ri].Alive = true;
                    }
                    else
                    {
                        nextCells[ci, ri].Alive = false;
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

        //currentCells = nextCells;

        for (int ri = 0; ri < Rows; ri++)
        {
            for (int ci = 0; ci < Columns; ci++)
            {
                currentCells[ci, ri].Alive = nextCells[ci, ri].Alive;
            }
        }
        RefreshUI();
    }

    private float CountAliveNeighbors(CellClass[,] cellMatrix, int cols, int rows)//
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
    
    private void RefreshUI()//
    {
        for (int ri = 0; ri < Rows; ri++)
        {
            for (int ci = 0; ci < Columns; ci++)
            {
                if (currentCells[ci, ri].Alive)
                {
                    matrixSR[ci,ri].color = Color.white;
                }
                else
                {
                    matrixSR[ci, ri].color = Color.black;

                }
            }
        }
    }
    private bool ContainsCell(int colOffset, int rowOffset)//
    {
        for (int ri = 0; ri < Rows; ri++)
        {
            for (int ci = 0; ci < Columns; ci++)
            {
                if(colOffset < 0 || colOffset > (Columns - 1) || rowOffset < 0 || rowOffset > (Rows - 1))
                {
                    return false;
                }
                
                if (currentCells[ci, ri] == currentCells[colOffset, rowOffset])
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()//
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GoNextState();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (constantPlay)
            {
                StopCoroutine(Play());
                constantPlay = false;
            }
            else
            {
                StartCoroutine(Play());
                constantPlay = true;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            for (int ri = 0; ri < Rows; ri++)
            {
                for (int ci = 0; ci < Columns; ci++)
                {
                    if (Vector2.Distance(matrixSR[ci, ri].transform.position, mousePos) <= 0.5f)
                    {
                        if(currentCells[ci, ri].Alive)
                        {
                            currentCells[ci, ri].Alive = false;
                            RefreshUI();
                        }
                        else
                        {
                            currentCells[ci, ri].Alive = true;
                            RefreshUI();
                        }
                    }
                }
            }
        }
    }

    IEnumerator Play()
    {
        GoNextState();
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Play());
    }
}
