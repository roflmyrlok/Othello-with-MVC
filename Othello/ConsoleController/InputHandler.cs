namespace ConsoleController;

public static class InputHandler
{

	public static (int, string) ParseInput(string newInput)
	{
		try
		{
			string column = "";
			int row = 0;
			var splittedInput = newInput.Split(" ");
			var l = splittedInput.Length;
			switch (l)
			{
				case 2:
					foreach (var spl in splittedInput)
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
					return (row, column);
					break;
			}
		}
		catch (Exception e)
		{
			//Console.WriteLine(e);
			throw new Exception("cannot process the input");
		}

		return (0, "");
	}
}