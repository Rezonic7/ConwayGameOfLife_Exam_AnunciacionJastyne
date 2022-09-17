using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoResizeCamera : MonoBehaviour
{
    private InstantiateCells instantiatedCells;

    private void Awake()
    {
        instantiatedCells = GetComponent<InstantiateCells>();
        Camera.main.orthographicSize = (float)instantiatedCells.Rows / 2;
        Camera.main.transform.position = new Vector3(((float)instantiatedCells.Columns / 2) - 1, ((float)-instantiatedCells.Rows / 2) + 0.5f, -10);
    }
}
