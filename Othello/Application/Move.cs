namespace Application;

public class Move(Player performedBy, int internalRow, int internalColumn, DateTime moveMadeAt)
{
	internal Player PerformedBy = performedBy;
	internal int InternalRow = internalRow;
	internal int InternalColumn = internalColumn;
	internal DateTime MoveMadeAt = moveMadeAt;
	internal bool MoveByTimer = false;
}