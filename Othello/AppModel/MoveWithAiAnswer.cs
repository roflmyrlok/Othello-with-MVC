using System.Timers;
using Model;
using Timer = System.Timers.Timer;

namespace AppModel;

public class MoveWithAiAnswer : Move
{
	private readonly IMoveProvider _moveProvider;
	private readonly IViewApp _viewApp;
	private readonly ICoordinatesTranslator _coordinatesTranslator;
	private Timer _botMoveOnTimer;
	private Game _currentGame;

	public MoveWithAiAnswer(IMoveProvider moveProvider, IViewApp viewApp, ICoordinatesTranslator coordinatesTranslator, Game currentGame, Timer botMoveOnTimer)
	{
		_botMoveOnTimer = botMoveOnTimer;
		_moveProvider = moveProvider;
		_viewApp = viewApp;
		_coordinatesTranslator = coordinatesTranslator;
		_currentGame = currentGame;
	}
	public override void MakeMove(int row, int column)
	{
		_currentGame.MakeMove(row, column);
		_botMoveOnTimer.Start();
		_botMoveOnTimer.Elapsed += BotMoveOnTimerResponse;
	}

	private void BotMoveOnTimerResponse(object? sender, ElapsedEventArgs e)
	{
		var responseMove =
			_moveProvider.DetermineBestMove(_currentGame.GetGameBoardData(), _currentGame.GetAvailableMovesData());
		_viewApp.ShowEventAiMoveComing(responseMove.Item1 + 1, _coordinatesTranslator.ConvertNumberToLetter(responseMove.Item2 + 1));
		Thread.Sleep(500);
		_currentGame.MakeMove(responseMove.Item1, responseMove.Item2);
		_botMoveOnTimer.Dispose();
	}
}