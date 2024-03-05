namespace InputParser;

public class Input
{
	private List<string> _parsedInput;
	private List<int> _returnInp;

	public Input(string newInput)
	{
		_parsedInput = new List<string>();
		_returnInp = new List<int>();
		try
		{
			var splittedInput = newInput.Split(" ");
			var l = splittedInput.Length;
			switch (l)
			{
				case 2:
					foreach (var spl in splittedInput)
					{
						int integer;
						int.TryParse(spl, out integer);
						_returnInp.Add(integer);
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

	public List<int> GetParsedInput()
	{
		return _returnInp;
	}
}