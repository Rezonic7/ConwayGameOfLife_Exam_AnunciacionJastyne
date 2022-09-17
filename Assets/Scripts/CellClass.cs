using UnityEngine;
public class CellClass
{
    public bool Alive;

    public CellClass()
    {
        int random = Random.Range(0, 2);
        if (random == 1)
        {
            Alive = true;
        }
        else
        {
            Alive = false;
        }
    }

    public CellClass(bool _alive)
    {
        Alive = _alive;
    }

}
