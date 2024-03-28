using Model;

namespace Application;

public class Config
{
	public bool AutoHint;
	public bool Timer;

	public Config(bool autoHint = true, bool timer = false)
	{
		Timer = timer;
		AutoHint = autoHint;
	}
}