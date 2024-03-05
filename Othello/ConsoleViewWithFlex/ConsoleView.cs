using Model;

namespace ConsoleViewWithFlex;

public class ConsoleView : IObserver
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
					case CellState.Available:
						row += " a ";
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
}