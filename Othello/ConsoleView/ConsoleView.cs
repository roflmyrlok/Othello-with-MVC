using Application;
using Model;

namespace ConsoleView;

public class ConsoleView : IViewApp
{
	public void ShowChange(GameBoard gameBoard, Player currentPlayer)
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
						row += " _ ";
						break;
					case CellState.White:
						row += " 1 ";
						break;
					case CellState.Black:
						row += " 2 ";
						break;
				}
			}
			row += "=";
			Console.WriteLine(row);
			
			
		}
		Console.WriteLine("==========================");
		Console.WriteLine(currentPlayer.PlayerCellState + "'s move");
	}

	public void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask,  Player currentPlayer)
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
						rowString += " _ ";
						break;
					case CellState.White:
						rowString += " 1 ";
						break;
					case CellState.Black:
						rowString += " 2 ";
						break;
				}
			}
			rowString += "|";
			Console.WriteLine(rowString);
			
			
		}
		Console.WriteLine(" ==========================");
		Console.WriteLine(currentPlayer.PlayerCellState + "'s move");
	}

	public void ShowCellOccupied(Player occupiedBy)
	{
		Console.WriteLine("Cell occupied by " + occupiedBy.PlayerCellState);
	}

	public void ShowOnWin(Player winner)
	{
		Console.WriteLine("Game won by " + winner.PlayerCellState);
	}

	public void ShowTimerMoveComing(int row, string column)
	{
		Console.WriteLine("Time out! Auto-move coming:  " + row + " " + column);
	}

	public void ShowMoveMade(Player madeBy, int row, string column)
	{
		Console.WriteLine( madeBy.PlayerCellState  + " made move:" + row + " " + column);
	}

	public void ShowMoveMadeAttempt(Player madeBy, int row, string column)
	{
		Console.WriteLine( madeBy.PlayerCellState  + " entered move: " + row + " " + column);
	}
}