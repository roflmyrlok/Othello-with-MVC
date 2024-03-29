namespace Application;

public class Configuration
{
	public bool AutoHint { get; }
	public bool Timer { get; }

	public Configuration(bool autoHint = true, bool timer = false)
	{
		Timer = timer;
		AutoHint = autoHint;
	}
}