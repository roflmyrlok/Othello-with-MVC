using AiOthelloModel;
using AppModel;
using Model;

namespace ConsoleController;

public class HumanPlayerNotifyable : GenericPlayerNotifyable
{
	public HumanPlayerNotifyable(CellState currentPlayerCellState, AppFlow currentGame) : base (currentPlayerCellState, currentGame)
	{
	}
	public override void OpponentMoveMaid(CellState receiver)
	{
		if (receiver != this._currentPlayerCellState)
		{
			return;
		}
		PlayerRequestAction();
	}

	public override void OpponentMoveCanceled(CellState receiver)
	{
		if (receiver != this._currentPlayerCellState)
		{
			return;
		}
		PlayerRequestAction();
	}

	protected override void PlayerRequestAction()
	{
		try
		{
			Console.WriteLine("Provide input for move of " + this._currentPlayerCellState);
			Console.WriteLine("Format: player number + space +, row and column in any order separated by space");
			var input = Console.ReadLine();

			var splitInput = input.Split(" ");
			if (splitInput[0] == 1.ToString() || splitInput[0] == 2.ToString())
			{
				splitInput = splitInput[1..];
			}
			else
			{
				PlayerRequestAction();
			}
		
			int row = -1;
			string column = "";
		
			foreach (var spl in splitInput)
			{
				var lrow = 0;
				if (!int.TryParse(spl, out lrow))
				{
					column = spl;
				}
				else
				{
					row = lrow;
				}
			}

			if (!CurrentGame.TryMakeMoveInCurrentGame(row, column, this._currentPlayerCellState))
			{
				PlayerRequestAction();
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
		
	}
}