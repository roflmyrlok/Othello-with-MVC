using Model;

namespace AppModel;

public class LocalGameView : IView
{
	private IViewApp _viewApp;

	public LocalGameView(IViewApp viewApp)
	{
		_viewApp = viewApp;
	}
	public void ShowChange(GameBoard gameBoard)
	{
		_viewApp.ShowChange(gameBoard);
	}

	public void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask)
	{
		_viewApp.ShowAvailableMoves(gameBoard, movesMask);
	}

	public void ShowEventCellOccupied()
	{
		_viewApp.ShowEventCellOccupied();
	}
}