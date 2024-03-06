using Model;

namespace ConsoleView;

public class ConsoleView : IView
{
	public void ShowChange(GameBoard gameBoard)
	{
		Console.WriteLine("==========================");
		foreach (var collumn in gameBoard.Board)
		{
			var row = "=";
			foreach (var cell in collumn)
			{
				switch (cell.CellState)
				{
					case CellState.Empty:
						row += " 0 ";
						break;
					case CellState.Player1:
						row += " 1 ";
						break;
					case CellState.Player2:
						row += " 2 ";
						break;
				}
			}
			row += "=";
			Console.WriteLine(row);
			
			
		}
		Console.WriteLine("==========================");
	}

	public void ShowInputError(string inputError)
	{
		Console.WriteLine(inputError);
	}

	public void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask)
	{
		Console.WriteLine(" |=a==b==c==d==e==f==g==h=|");
		var rowN = 1;
		foreach (var row in gameBoard.Board)
		{
			var rowString = rowN.ToString() + "|";
			rowN++;
			foreach (var cell in row)
			{
				switch (cell.CellState)
				{
					case CellState.Empty:
						if (movesMask[cell.Row][cell.Column])
						{
							rowString += " a ";
							break;
						}
						rowString += " 0 ";
						break;
					case CellState.Player1:
						rowString += " 1 ";
						break;
					case CellState.Player2:
						rowString += " 2 ";
						break;
				}
			}
			rowString += "|";
			Console.WriteLine(rowString);
			
			
		}
		Console.WriteLine(" ==========================");
	}
}