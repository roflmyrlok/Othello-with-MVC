namespace ConsoleController;

public static class BoardCoordinatesInternalTranslator
{ 
	public static string ConvertNumberToLetter(int number)
	{
		List<string> alphabet = new List<string> {"a","b","c","d","e","f","g","h","i","g","k"};
		if (number <= alphabet.Count - 1)
		{
			return alphabet[number - 1];
		}
		throw new Exception("number cringe");
	}
	public static int ConvertLetterToNumber(string letter)
	{
		List<string> alphabet = new List<string> {"a","b","c","d","e","f","g","h","i","g","k"};
		var i = 1;
		foreach (var number in alphabet)
		{
			if (number == letter)
			{
				return i;
			}
			i++;
		}
		throw new Exception("number cringe");
	}

}