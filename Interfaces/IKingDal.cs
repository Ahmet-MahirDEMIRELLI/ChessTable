using ChessTable.Classes;
using System.Collections.Generic;

namespace ChessTable.Interfaces
{
	public interface IKingDal
	{
		int GetPoint();
		List<Move> GetPossibleMoves(Board board, int row, int column, bool isWhite);
	}
}
