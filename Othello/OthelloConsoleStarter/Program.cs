// See https://aka.ms/new-console-template for more information

using AiOthelloModel;
using AppModel;
using ConsoleController;
using ConsoleView;

Console.WriteLine("pudge");

var app = new AppFlow(new ConsoleView.ConsoleView(), new Ai(), new BoardCoordinatesInternalTranslator());
var c = new ConsoleController.ConsoleController(app);
c.Start();


