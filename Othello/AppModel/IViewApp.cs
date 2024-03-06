using Model;

namespace AppModel;

public interface IViewApp
{
	void ShowChange(GameBoard gameBoard);
	void ShowAvailableMoves(GameBoard gameBoard, List<List<bool>> movesMask);
	void ShowEventCellOccupied();
}