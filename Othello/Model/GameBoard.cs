namespace Model;

public class GameBoard
{
	//column{row}
	public List<List<Cell>> Board;
	public int Columns { get; }
	public int Rows { get; }

	public GameBoard(int columns, int rows)
	{
		Board = new List<List<Cell>>();
		Rows = rows;
		Columns = columns;
		for (int i = 0; i < columns; i++)
		{
			Board.Add(new List<Cell>());
			for (int j = 0; j < rows; j++)
			{
				Board[i].Add(new Cell(i,j, CellState.Empty));
			}
		}
	}
	
}