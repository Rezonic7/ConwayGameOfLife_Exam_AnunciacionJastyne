using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour, IClickable, IHoverable
{
    private IManagerCellMatrix iCellMatrixManager;

    private CellClass _cellClass = new CellClass(false);
    private SpriteRenderer _cellSprite;

    private Color changeableAliveColor;
    private Color changeableDeadColor;

    public CellClass CellClass { get { return _cellClass; } }
    public SpriteRenderer CellSprite { get { return _cellSprite; } }

    

    private void Awake()
    {
        _cellSprite = GetComponent<SpriteRenderer>();

        foreach (GameObject gameObjects in FindObjectsOfType<GameObject>())
        {
            if (gameObjects.GetComponent<IManagerCellMatrix>() != null)
                if(gameObjects.GetComponent<ManagerCellMatrix>() != null)
                    iCellMatrixManager = gameObjects.GetComponent<IManagerCellMatrix>();
        }
    }

    public void ChangeStateColors(Color aliveColor, Color deadColor)
    {
        changeableAliveColor = aliveColor;
        changeableDeadColor = deadColor;
    }

    public void SwitchState(Cell cellToSwitch)
    {
        if (cellToSwitch._cellClass.Alive)
        {
            cellToSwitch._cellClass.Alive = false;
            iCellMatrixManager.IncrementAliveCells(-1);
        }
        else
        {
            cellToSwitch._cellClass.Alive = true;
            iCellMatrixManager.IncrementAliveCells(1);
        }
        ChangeColorDependingOnState();

        iCellMatrixManager.UpdateCell(this);
    }

    public void ChangeColorDependingOnState()
    {
        
        if (_cellClass.Alive)
        {
            SetColor(changeableAliveColor);
        }
        else
        {
            SetColor(changeableDeadColor);
        }
    }

    public void SetColor(Color changedColor)
    {
        if (this.gameObject != null)
        {
            CellSprite.color = changedColor;
        }
    }

    public void Click()
    {
        if(!iCellMatrixManager.IsPaused())
        {
            if (!iCellMatrixManager.IsContinousGeneration())
            {
                SwitchState(this);
            }
        }
    }

    public void Hover()
    {
        if (!iCellMatrixManager.IsPaused())
        {
            if (!iCellMatrixManager.IsContinousGeneration() || !iCellMatrixManager.IsPaused())
            {
                SetColor(Color.gray);
            }
        }
    }

    public void Unhover()
    {
        ChangeColorDependingOnState();
    }
}



