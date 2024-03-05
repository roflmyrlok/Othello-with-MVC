// See https://aka.ms/new-console-template for more information

using ConsoleController;
using ConsoleView;

Console.WriteLine("pudge");

var o = new ConsoleView.ConsoleView();
var en = new ConsoleErrorNotifier();
var c = new ConsoleController.ConsoleController(o, en);
while (true)
{
	(int, int) arg; ;
	try
	{
		var input = Console.ReadLine();
		var parser = new Input(input);
		arg = parser.GetParsedInput();
	}
	catch (Exception e)
	{
		Console.WriteLine(e);
		throw;
	}
	
	
	c.Act(arg);
}


