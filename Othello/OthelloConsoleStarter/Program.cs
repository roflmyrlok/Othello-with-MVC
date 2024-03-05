// See https://aka.ms/new-console-template for more information

using Model;

Console.WriteLine("pudge");

var o = new ConsoleViewWithFlex.ConsoleView();
var c = new ConsoleControllerWithFlex.ConsoleController(o);
while (true)
{
	string input = Console.ReadLine();
	var i = new InputParser.Input(input);
	c.Act(i.GetParsedInput());
}


