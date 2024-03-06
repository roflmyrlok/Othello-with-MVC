namespace Model;

public class Cell
{
	public int Row;
	public int Column;
	public CellState CellState;

	public Cell(int row, int column, CellState cellState = CellState.Empty)
	{
		Row = row;
		Column = column;
		CellState = cellState;
	}
	
}