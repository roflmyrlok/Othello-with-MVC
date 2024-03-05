using Model;

namespace ConsoleView;

public class ConsoleErrorNotifier : IInputErrorNotifier
{
	public void ShowInputError(string inputError)
	{
		Console.WriteLine(inputError);
	}
}