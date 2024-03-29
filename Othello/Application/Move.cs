namespace Application;

public class Move(Player performedBy, int internalRow, int internalColumn, DateTime moveMadeAt)
{
	public Player PerformedBy = performedBy;
	public int InternalRow = internalRow;
	public int InternalColumn = internalColumn;
	public DateTime MoveMadeAt = moveMadeAt;
	public bool MoveByTimer = false;
}