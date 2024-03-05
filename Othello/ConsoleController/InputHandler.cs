namespace ConsoleController;

public class Input
{
	private List<string> _alphabet = new List<string>
	{
		"a","b","c","d","e","f","g","h","i","g","k"
	};

	private int column = 0;
	private int row = 0;

	public Input(string newInput)
	{
		try
		{
			var splittedInput = newInput.Split(" ");
			var l = splittedInput.Length;
			switch (l)
			{
				case 2:
					foreach (var spl in splittedInput)
					{
						var lrow = 0;
						if (!int.TryParse(spl,out lrow))
						{
							column = 0;
							while (true)
							{
								if (_alphabet[column] == spl)
								{
									break;
								}
								if (column >= _alphabet.Count - 1)
								{
									throw new Exception("aplphabet");
								}
								column += 1;
							}
							column += 1;
						}
						else
						{
							row = lrow;
						}
					}
					break;
			}
		}
		catch (Exception e)
		{
			//Console.WriteLine(e);
			throw new Exception("cannot process the input");
		}
		
	}

	public (int,int) GetParsedInput()
	{
		if (row == 0 || column == 0)
		{
			throw new Exception("cannot be 0");
		}
		return (column, row);
	}
}