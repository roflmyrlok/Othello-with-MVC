// See https://aka.ms/new-console-template for more information

using AiOthelloModel;
using Application;
using ConsoleController;
using ConsoleView;

Console.WriteLine("Hello Pudge!");
Console.WriteLine("To create new game enter: " + "ng tForHint tForBot tForTimer");
Console.WriteLine("For example: " + "ng t f t");
Console.WriteLine("will create game with auto hints enabled, bot and timer disabled");
Console.WriteLine("Timer will make a random move after 20 seconds of afk");
Console.WriteLine("To get hint just use: hint");
Console.WriteLine("To undo move use: u");
Console.WriteLine("Note that u have 3 seconds to undo move or before enemy makes his move");
Console.WriteLine("If u losing use: q");
Console.WriteLine("Good luck!");
Console.WriteLine("p.s. exception => bug V I forgot to remove it while testing wrong input");

var view = new ConsoleView.ConsoleView();
var c = new ConsoleController.ConsoleController(view);


