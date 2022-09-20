public interface IManagerCellMatrix
{
    void InstantiateNewCellMatrix();
    bool IsContinousGeneration();
    bool IsPaused();
    void InitializeCells(Cell[,] cells);
    void UpdateCurrentMatix(Cell[,] cells);
    void UpdateCell(Cell cellToUpdate);
    void IncrementAliveCells(int amount);
}