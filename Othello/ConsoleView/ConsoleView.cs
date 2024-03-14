using AppModel;
using Model;

namespace ConsoleView;

public class ConsoleView : IViewApp
{
	public void ShowChange(GameBoard gameBoard, CellState currentPlayer)
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
		Console.WriteLine(currentPlayer + "'s move");
	}

	public void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask,  CellState currentPlayer)
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
		Console.WriteLine(currentPlayer + "'s move");
	}

	public void ShowEventCellOccupied( CellState currentPlayer)
	{
		Console.WriteLine("occupied by:" + currentPlayer.ToString());
	}

	public void ShowEventAiMoveComing(int row, string column)
	{
		Console.WriteLine("Ai move: " + row + column );
	}

	public void ShowEventTimerMoveComing(int row, string column)
	{
		Console.WriteLine("TIME OUT! RANDOM MOVE " + row + " " + column + " WAS MADE");
	}

	public void ShowEventCancel()
	{
		Console.WriteLine("Move canceled, restoring game");
	}

	public void ShowEventWin(CellState cellState)
	{
		Console.WriteLine("Winner:" + cellState);
	}

	public void ShowEventCannotRestoreMove()
	{
		Console.WriteLine("Cannot restore move");
	}

	public void ShowEventInputDuringBotMove()
	{
		Console.WriteLine("Cannot move while bot is thinking");
	}
}