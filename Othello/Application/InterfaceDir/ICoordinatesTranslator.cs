namespace Application;

public interface ICoordinatesTranslator
{
	public string ConvertNumberToLetter(int number);
	public int ConvertLetterToNumber(string letter);
}