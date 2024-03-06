using System.Security.Cryptography;
using Model;

namespace AiOthelloModel;


public static class Ai
{
	public static (int, int) DetermineBestMove((GameBoard, List<List<bool>>) tuple)
	{
		var am = tuple.Item2;
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
		var move = new Random().Next(1, upperLimit);
		var col = 0;
		foreach (var liBo in am)
		{
			col += 1;
			var raw = 0;
			foreach (var bo in liBo)
			{
				raw += 1;
				if (bo)
				{
					move -= 1;
				}

				if (move == 0)
				{
					return (col, raw);
				}
			}
		}

		throw new Exception("no moves");
	}
}