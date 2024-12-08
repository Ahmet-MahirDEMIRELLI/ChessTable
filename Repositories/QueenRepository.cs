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
			List<Move> bishopMoves;
			byte[,] matrix = board.BoardMatrix;
			if (threadCheckRepository.IsMovable(matrix, row, column, isWhite ? board.WhiteKing.Row : board.BlackKing.Row, isWhite ? board.WhiteKing.Col : board.BlackKing.Col, isWhite))
			{
				BishopRepository bishopRepository = new BishopRepository();
				RookRepository rookRepository = new RookRepository();
				bishopMoves = bishopRepository.GetPossibleMoves(board, row, column, isWhite);
				rookMoves = rookRepository.GetPossibleMoves(board,row , column, isWhite);
				possibleMoves = bishopMoves;
				foreach (Move m in rookMoves)
				{
					possibleMoves.Add(m);
				}
			}
			return possibleMoves;
		}
	}
}
