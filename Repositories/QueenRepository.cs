using ChessTable.Classes;
using ChessTable.Interfaces;
using System.Collections.Generic;

namespace ChessTable.Repositories
{
	public class QueenRepository : IQueenDal
	{
		public int GetPoint()
		{
			return 9;
		}
		public List<Move> GetPossibleMoves(Board board, int row, int column, bool isWhite)
		{
			ThreadCheckRepository threadCheckRepository = new ThreadCheckRepository();
			List<Move> possibleMoves = new List<Move>();
			List<Move> rookMoves;
			byte[,] matrix = board.BoardMatrix;
			int type = threadCheckRepository.IsMovable(matrix, row, column, isWhite ? board.WhiteKing.Row : board.BlackKing.Row, isWhite ? board.WhiteKing.Col : board.BlackKing.Col, isWhite);
			BishopRepository bishopRepository = new BishopRepository();
			RookRepository rookRepository = new RookRepository();
			switch (type)
			{
				case 0:
					possibleMoves = bishopRepository.GetPossibleMoves(board, row, column, isWhite);
					rookMoves = rookRepository.GetPossibleMoves(board, row, column, isWhite);
					foreach (Move m in rookMoves)
					{
						possibleMoves.Add(m);
					}
					break;
				case 1:
				case 2:
				case 3:
				case 4:
					possibleMoves = rookRepository.GetPossibleMoves(board, row, column, isWhite);
					break;
				case 5:
				case 6:
				case 7:
				case 8:
					possibleMoves = bishopRepository.GetPossibleMoves(board, row, column, isWhite);
					break;
			}
			return possibleMoves;
		}
	}
}
