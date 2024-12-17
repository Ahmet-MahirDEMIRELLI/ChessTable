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
			List<Move> possibleMoves = new List<Move>();
			if (isWhite)
			{
				if (board.IsChecked)
				{
					if (board.BlacksCheckers[1].Row == -1) // çifte şah yok
					{
						Move eatingMove = GetEatingMoves(board, row, column, board.BlacksCheckers[0], isWhite);
						if (eatingMove != null)
						{
							possibleMoves.Add(eatingMove);
						}
						List<Move> blockingMoves = GetBlockingMoves(board, row, column, board.BlacksCheckers[0], isWhite);
						foreach (Move blockingMove in blockingMoves)
						{
							possibleMoves.Add(blockingMove);
						}
					}
					return possibleMoves;
				}
			}
			else
			{
				if (board.IsChecked)
				{
					if (board.WhitesCheckers[1].Row == -1) // çifte şah yok
					{
						Move eatingMove = GetEatingMoves(board, row, column, board.WhitesCheckers[0], isWhite);
						if (eatingMove != null)
						{
							possibleMoves.Add(eatingMove);
						}
						List<Move> blockingMoves = GetBlockingMoves(board, row, column, board.WhitesCheckers[0], isWhite);
						foreach (Move blockingMove in blockingMoves)
						{
							possibleMoves.Add(blockingMove);
						}
					}
					return possibleMoves;
				}
			}

			List<Move> normalMoves = GetNormalMoves(board, row, column, isWhite);
			foreach (Move normalMove in normalMoves)
			{
				possibleMoves.Add(normalMove);
			}

			return possibleMoves;
		}

		private List<Move> GetBlockingMoves(Board board, int row, int column, Square checker, bool isWhite)
		{
			ThreadCheckRepository threadCheckRepository = new ThreadCheckRepository();
			List<Move> possibleMoves = new List<Move>();
			int type = threadCheckRepository.TraceToKing(board.BoardMatrix, checker.Row, checker.Col, isWhite ? board.WhiteKing.Row : board.BlackKing.Row, isWhite ? board.WhiteKing.Col : board.BlackKing.Col, isWhite);
			List<Move> normalMoves = GetNormalMoves(board, row, column, isWhite);
			switch (type)
			{
				case 1: // soldan şah çekiliyor
					foreach (Move move in normalMoves)
					{
						if (move.Row == checker.Row && move.Column > checker.Col)
						{
							possibleMoves.Add(move);
						}
					}
					break;
				case 2: // sağdan şah çekiliyor
					foreach (Move move in normalMoves)
					{
						if (move.Row == checker.Row && move.Column < checker.Col)
						{
							possibleMoves.Add(move);
						}
					}
					break;
				case 3: // üstten şah çekiliyor
					foreach (Move move in normalMoves)
					{
						if (move.Row > checker.Row && move.Column == checker.Col)
						{
							possibleMoves.Add(move);
						}
					}
					break;
				case 4: // alttan şah çekiliyor
					foreach (Move move in normalMoves)
					{
						if (move.Row < checker.Row && move.Column == checker.Col)
						{
							possibleMoves.Add(move);
						}
					}
					break;
				case 5: // sağ alttan şah çekiliyor
					foreach (Move move in normalMoves)
					{
						int rowDiff = move.Row - checker.Row;
						int colDiff = move.Column - checker.Col;
						if ((rowDiff == colDiff || rowDiff == -colDiff) && rowDiff < 0 && colDiff < 0)
						{
							possibleMoves.Add(move);
						}
					}
					break;
				case 6: // sağ üstten şah çekiliyor
					foreach (Move move in normalMoves)
					{
						int rowDiff = move.Row - checker.Row;
						int colDiff = move.Column - checker.Col;
						if ((rowDiff == colDiff || rowDiff == -colDiff) && rowDiff > 0 && colDiff < 0)
						{
							possibleMoves.Add(move);
						}
					}
					break;
				case 7: // sol üstten şah çekiliyor
					foreach (Move move in normalMoves)
					{
						int rowDiff = move.Row - checker.Row;
						int colDiff = move.Column - checker.Col;
						if ((rowDiff == colDiff || rowDiff == -colDiff) && rowDiff > 0 && colDiff > 0)
						{
							possibleMoves.Add(move);
						}
					}
					break;
				case 8: // sol alttan şah çekiliyor
					foreach (Move move in normalMoves)
					{
						int rowDiff = move.Row - checker.Row;
						int colDiff = move.Column - checker.Col;
						if ((rowDiff == colDiff || rowDiff == -colDiff) && rowDiff < 0 && colDiff > 0)
						{
							possibleMoves.Add(move);
						}
					}
					break;
			}
			return possibleMoves;
		}

		private Move GetEatingMoves(Board board, int row, int col, Square checker, bool isWhite)
		{
			int rowDiff = row - checker.Row;
			int colDiff = col - checker.Col;
			if ((rowDiff == 2 || rowDiff == -2) && (colDiff == 1 || colDiff == -1) || (rowDiff == 1 || rowDiff == -1) && (colDiff == 2 || colDiff == -2)) // şah çeken atın kolunda
			{
				var moves = GetNormalMoves(board, row, col, isWhite);
				foreach (Move move in moves)
				{
					if (move.Row == checker.Row && move.Column == checker.Col)
					{
						return move;
					}
				}
			}
			return null;
		}

		private List<Move> GetNormalMoves(Board board, int row, int column, bool isWhite)
		{
			ThreadCheckRepository threadCheckRepository = new ThreadCheckRepository();
			List<Move> possibleMoves = new List<Move>();
			Move move;
			byte[,] matrix = board.BoardMatrix;
			int[,] squares = { { row - 2, column + 1 }, { row - 1, column + 2 }, { row + 1, column + 2 }, { row + 2, column + 1 }, { row - 2, column - 1 }, { row - 1, column - 2 }, { row + 1, column - 2 }, { row + 2, column - 1 } };
			// maximum 8 kare var
			if (threadCheckRepository.IsMovable(matrix, row, column, isWhite ? board.WhiteKing.Row : board.BlackKing.Row, isWhite ? board.WhiteKing.Col : board.BlackKing.Col, isWhite))
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
