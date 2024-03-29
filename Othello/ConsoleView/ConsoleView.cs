using Application;
using Model;

namespace ConsoleView;

public class ConsoleView : IViewApp
{
	public ConsoleView()
	{
		Console.WriteLine("Hello Pudge!");
		Console.WriteLine("To create new game enter: " + "ng tForHint tForBot tForTimer");
		Console.WriteLine("For example: " + "ng t f f");
		Console.WriteLine("Will create game with auto hints enabled, bot and timer disabled");
		Console.WriteLine("Note that: " + "ng t t t");
		Console.WriteLine("Will create game were 2 bots are playing and it is the only way to do so");
		Console.WriteLine("Timer will make a random move after 20 seconds of afk");
		Console.WriteLine("To make move use: w/b row column");
		Console.WriteLine("Where 'w 3 c' will try to make move 3 c for white");
		Console.WriteLine("To get hint just use: w/b hint");
		Console.WriteLine("To undo move use: w/b u");
		Console.WriteLine("Note that u have 3 seconds to undo move or before enemy makes his move");
		Console.WriteLine("If u losing use: q");
		Console.WriteLine("Good luck!");
	}
	public void ShowChange(GameBoard gameBoard, Player currentPlayer)
	{
		Console.WriteLine("==========================");
		foreach (var column in gameBoard.Board)
		{
			var row = "=";
			foreach (var cell in column)
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

	public void ShowUndoMade()
	{
		Console.WriteLine("Undo made");
	}

	public void ShowFailedToPerformRequest()
	{
		Console.WriteLine("Can't made this action");
	}
}