namespace Application;

public class Configuration
{
	public bool AutoHint;
	public bool Timer;

	public Configuration(bool autoHint = true, bool timer = false)
	{
		Timer = timer;
		AutoHint = autoHint;
	}
}