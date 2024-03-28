namespace Application;

public interface ISetup
{
	public bool Setup(bool autoHint, bool timer, Player playerWhite,
		Player playerBlack);
}