using Model;

namespace AppModel;

public class Config
{
	public bool AutoHint;
	public bool BotOpponent;
	public bool Timer;
	public List<(int, int)> CurrentGameMoves { get; set; } = new List<(int, int)>();
	public bool Win = false;
	public CellState WCellState;

	public Config(bool autoHint = true, bool botOpponent = false, bool timer = false)
	{
		Timer = timer;
		AutoHint = autoHint;
		BotOpponent = botOpponent;
	}
}