using System.Security.Cryptography;
using AppModel;
using Model;

namespace AiOthelloModel;


public class Ai : IMoveProvider
{
	public (int, int) DetermineBestMove(GameBoard gameBoard, List<List<bool>> am)
	{
		var upperLimit = 0;
		foreach (var liBo in am)
		{
			foreach (var bo in liBo)
			{
				if (bo)
				{
					upperLimit += 1;
				}
			}
		}
		var move = new Random(DateTime.Now.Millisecond).Next(1, upperLimit);
		var raw = 0;
		foreach (var liBo in am)
		{
			var col = 0;
			foreach (var bo in liBo)
			{
				if (bo)
				{
					move -= 1;
				}

				if (move == 0)
				{
					return (raw, col);
				}
				col += 1;
			}
			raw += 1;
		}

		throw new Exception("no moves");
	}
}