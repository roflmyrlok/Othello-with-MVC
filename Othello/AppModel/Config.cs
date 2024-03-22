using Model;

namespace AppModel;

public class Config
{
	public bool AutoHint;
	public bool Timer;
	public List<(int, int)> CurrentGameMoves { get; set; } = new List<(int, int)>();
	public bool Win = false;
	public CellState WCellState;

	public Config(bool autoHint = true, bool timer = false)
	{
		Timer = timer;
		AutoHint = autoHint;
	}
}