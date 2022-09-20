using UnityEngine;
public interface IInstantiateCellMatrix
{
    void Instantiate(int Columns, int Rows, GameObject cellPrefab, Transform cellParent);
}