namespace AppModel;

public class Config
{
	private bool _autoHint = true;
	private bool _botOpponent = true;
	private bool _timer = true;

	public Config(bool autoHint, bool botOpponent, bool timer)
	{
		_timer = timer;
		_autoHint = autoHint;
		_botOpponent = botOpponent;
	}
	public bool IsBot()
	{
		return _botOpponent;
	}

	public bool IsHint()
	{
		return _autoHint;
	}

	public bool IsTimer()
	{
		return _timer;
	}
}