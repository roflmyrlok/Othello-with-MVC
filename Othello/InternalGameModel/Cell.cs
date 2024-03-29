namespace Model;

public class Cell
{
	public readonly int Row;
	public readonly int Column;
	public CellState CellState;

	public Cell(int row, int column, CellState cellState = CellState.Empty)
	{
		Row = row;
		Column = column;
		CellState = cellState;
	}
	
}