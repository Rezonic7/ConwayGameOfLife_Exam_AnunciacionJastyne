using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour, IClickable, IHoverable
{
    private ICallableCellMatrix callCellMatrix;

    private CellClass _cellClass = new CellClass(false);
    private SpriteRenderer _cellSprite;

    public CellClass CellClass { get { return _cellClass; } set { _cellClass = value; } }
    public SpriteRenderer CellSprite { get { return _cellSprite; } }

    private void Awake()
    {
        _cellSprite = GetComponent<SpriteRenderer>();

        foreach (GameObject gameObjects in FindObjectsOfType<GameObject>())
        {
            if (gameObjects.GetComponent<ICallableCellMatrix>() != null)
                callCellMatrix = gameObjects.GetComponent<ICallableCellMatrix>();
        }
    }

    public void SwitchState(Cell cellToSwitch)
    {
        if (cellToSwitch._cellClass.Alive)
        {
            cellToSwitch._cellClass.Alive = false;
        }
        else
        {
            cellToSwitch._cellClass.Alive = true;
        }
        ChangeColorDependingOnState();

        callCellMatrix.ChangeState(cellToSwitch);
    }

    public void ChangeColorDependingOnState()
    {
        if (_cellClass.Alive)
        {
            ChangeColor(Color.white);
        }
        else
        {
            ChangeColor(Color.black);
        }
    }

    public void ChangeColor(Color changedColor)
    {
        CellSprite.color = changedColor;
    }

    public void Click()
    {
        SwitchState(this);
    }

    public void Hover()
    {
        ChangeColor(Color.gray);
    }

    public void Unhover()
    {
        ChangeColorDependingOnState();
    }
}



