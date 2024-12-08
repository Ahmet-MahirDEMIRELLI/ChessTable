using ChessTable.Classes;
using ChessTable.Interfaces;
using System.Collections.Generic;

namespace ChessTable.Repositories
{
	public class KnightRepository : IKnightDal
	{
		public int GetPoint()
		{
			return 3;
		}

		public List<Move> GetPossibleMoves(Board board, int row, int column, bool isWhite)
		{
			ThreadCheckRepository threadCheckRepository = new ThreadCheckRepository();
			List<Move> possibleMoves = new List<Move>();
			Move move;
			byte[,] matrix = board.BoardMatrix;
			int[,] squares = {{row-2, column + 1}, {row-1, column + 2}, {row+1, column + 2}, {row+2, column +1}, {row-2, column -1}, {row-1, column -2}, {row+1, column -2}, {row+2, column -1}};
			// maximum 8 kare var
			if(threadCheckRepository.IsMovable(matrix, row, column, isWhite ? board.WhiteKing.Row : board.BlackKing.Row, isWhite ? board.WhiteKing.Col : board.BlackKing.Col, isWhite))
			{
				if (isWhite)
				{
					for (int i = 0; i < 8; i++)
					{
						if (CheckSquare(squares[i, 0], squares[i, 1]) && (matrix[squares[i, 0], squares[i, 1]] >= 8 || matrix[squares[i, 0], squares[i, 1]] == 0))
						{
							move = new Move()
							{
								Column = squares[i, 1],
								Row = squares[i, 0],
								Message = "",
							};
							possibleMoves.Add(move);
						}
					}
				}
				else
				{
					for (int i = 0; i < 8; i++)
					{
						if (CheckSquare(squares[i, 0], squares[i, 1]) && matrix[squares[i, 0], squares[i, 1]] <= 7)
						{
							move = new Move()
							{
								Column = squares[i, 1],
								Row = squares[i, 0],
								Message = "",
							};
							possibleMoves.Add(move);
						}
					}
				}
			}
			return possibleMoves;
		}

		private bool CheckSquare(int row, int col)
		{
			if(row >= 0 && col >= 0 && row <= 7 && col <= 7)
			{
				return true;
			}
			return false;
		}
	}
}
