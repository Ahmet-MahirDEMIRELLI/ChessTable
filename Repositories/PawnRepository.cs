using ChessTable.Classes;
using ChessTable.Interfaces;
using System.Collections.Generic;

namespace ChessTable.Repositories
{
	public class PawnRepository : IPownDal
	{
		public int GetPoint()
		{
			return 1;
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
			if ((rowDiff == 1 || rowDiff == -1) && (colDiff == 1 || colDiff == -1)) // şah çeken piyon kolunda
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
			if (threadCheckRepository.IsMovable(board.BoardMatrix, row, column, isWhite ? board.WhiteKing.Row : board.BlackKing.Row, isWhite ? board.WhiteKing.Col : board.BlackKing.Col, isWhite))
			{
				if (isWhite)
				{
					switch (row)        // özel durumlar
					{
						case 6:         // 2 kare ilerleme ihtimali var
							if (matrix[row - 2, column] == 0 && matrix[row - 1, column] == 0)   // 2 gidebilir mi?
							{
								move = new Move()
								{
									Column = column,
									Row = row - 2,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							break;
						case 3:         // geçerken al yapma ihtimali var
							if (column > 0 && column < 7)   // kenar piyonu değil ise doğru koluna girer
							{
								if (matrix[row, column - 1] == 9)  // solda 2 sürülmüş siyah piyon
								{
									move = new Move()
									{
										Column = column - 1,
										Row = row - 1,
										Message = $"Eats {row},{column - 1}"
									};
									possibleMoves.Add(move);
								}
								else if (matrix[row, column + 1] == 9)  // sağda 2 sürülmüş siyah piyon
								{
									move = new Move()
									{
										Column = column + 1,
										Row = row - 1,
										Message = $"Eats {row},{column + 1}"
									};
									possibleMoves.Add(move);
								}
							}
							else if (column == 7 && matrix[row, column - 1] == 9) // solda 2 sürülmüş siyah piyon
							{
								move = new Move()
								{
									Column = column - 1,
									Row = row - 1,
									Message = $"Eats {row},{column - 1}"
								};
								possibleMoves.Add(move);
							}
							else if (column == 0 && matrix[row, column + 1] == 9)  // sağda 2 sürülmüş siyah piyon
							{
								move = new Move()
								{
									Column = column + 1,
									Row = row - 1,
									Message = $"Eats {row},{column + 1}"
								};
								possibleMoves.Add(move);
							}
							break;
						case 1:     // Son yataya ulaşma ihtimali var
							if (matrix[row - 1, column] == 0)
							{
								move = new Move()
								{
									Column = column,
									Row = row - 1,
									Message = "Upgrade"
								};
								possibleMoves.Add(move);
							}
							WhiteEatingMoves(possibleMoves, matrix, row, column, true);
							break;
					}
					if (row != 1)       // bu işlemler row=1 için switch içinde yapıldı
					{
						if (matrix[row - 1, column] == 0)       // 1 kare ilerleme
						{
							move = new Move()
							{
								Column = column,
								Row = row - 1,
								Message = ""
							};
							possibleMoves.Add(move);
						}
						WhiteEatingMoves(possibleMoves, matrix, row, column, false);
					}
				}
				else
				{
					switch (row)        // özel durumlar
					{
						case 1:         // 2 kare ilerleme ihtimali var
							if (matrix[row + 2, column] == 0 && matrix[row + 1, column] == 0)   // 2 gidebilir mi?
							{
								move = new Move()
								{
									Column = column,
									Row = row + 2,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							break;
						case 4:         // geçerken al yapma ihtimali var
							if (column > 0 && column < 7)   // kenar piyonu değil ise doğru koluna girer
							{
								if (matrix[row, column - 1] == 2)  // solda 2 sürülmüş beyaz piyon (beyaz gözünden sol)
								{
									move = new Move()
									{
										Column = column - 1,
										Row = row + 1,
										Message = $"Eats {row},{column - 1}"
									};
									possibleMoves.Add(move);
								}
								else if (matrix[row, column + 1] == 2)  // sağda 2 sürülmüş beyaz piyon
								{
									move = new Move()
									{
										Column = column + 1,
										Row = row + 1,
										Message = $"Eats {row},{column + 1}"
									};
									possibleMoves.Add(move);
								}
							}
							else if (column == 7 && matrix[row, column - 1] == 2) // solda 2 sürülmüş beyaz piyon
							{
								move = new Move()
								{
									Column = column - 1,
									Row = row + 1,
									Message = $"Eats {row},{column - 1}"
								};
								possibleMoves.Add(move);
							}
							else if (column == 0 && matrix[row, column + 1] == 2)  // sağda 2 sürülmüş beyazsiyah piyon
							{
								move = new Move()
								{
									Column = column + 1,
									Row = row + 1,
									Message = $"Eats {row},{column + 1}"
								};
								possibleMoves.Add(move);
							}
							break;
						case 6:     // Son yataya ulaşma ihtimali var
							if (matrix[row + 1, column] == 0)
							{
								move = new Move()
								{
									Column = column,
									Row = row + 1,
									Message = "Upgrade"
								};
								possibleMoves.Add(move);
							}
							BlackEatingMoves(possibleMoves, matrix, row, column, true);
							break;
					}
					if (row != 6)       // bu işlemler row=6 için switch içinde yapıldı
					{
						if (matrix[row + 1, column] == 0)       // 1 kare ilerleme
						{
							move = new Move()
							{
								Column = column,
								Row = row + 1,
								Message = ""
							};
							possibleMoves.Add(move);
						}
						BlackEatingMoves(possibleMoves, matrix, row, column, false);
					}
				}
			}
			return possibleMoves;
		}
		private void WhiteEatingMoves(List<Move> moveList, byte[,] matrix, int row, int column, bool isUpgrade)
		{
			Move move;
			if (column > 0 && column < 7)   // kenar piyonu değil ise doğru koluna girer
			{
				if (matrix[row - 1, column - 1] >= 8)  // sol çaprazda siyah taş var
				{
					move = new Move()
					{
						Column = column - 1,
						Row = row - 1,
						Message = isUpgrade ? "Upgrade" : "",
					};
					moveList.Add(move);
				}
				if (matrix[row - 1, column + 1] >= 8)  // sağ çaprazda taş var
				{
					move = new Move()
					{
						Column = column + 1,
						Row = row - 1,
						Message = isUpgrade ? "Upgrade&" : "",
					};
					moveList.Add(move);
				}
			}
			else if (column == 7 && matrix[row - 1, column - 1] >= 8) // sol çaprazda taş var
			{
				move = new Move()
				{
					Column = column - 1,
					Row = row - 1,
					Message = isUpgrade ? "Upgrade" : "",
				};
				moveList.Add(move);
			}
			else if (column == 0 && matrix[row - 1, column + 1] >= 8)  // sağ çaprazda taş var
			{
				move = new Move()
				{
					Column = column + 1,
					Row = row - 1,
					Message = isUpgrade ? "Upgrade" : "",
				};
				moveList.Add(move);
			}
		}

		private void BlackEatingMoves(List<Move> moveList, byte[,] matrix, int row, int column, bool isUpgrade)
		{
			Move move;
			if (column > 0 && column < 7)   // kenar piyonu değil ise doğru koluna girer
			{
				if (matrix[row + 1, column - 1] != 0 && matrix[row + 1, column - 1] <= 7)  // sağ çaprazda beyaz taş var (beyaz gözünden sağ)
				{
					move = new Move()
					{
						Column = column - 1,
						Row = row + 1,
						Message = isUpgrade ? "Upgrade" : "",
					};
					moveList.Add(move);
				}
				if (matrix[row + 1, column + 1] != 0 && matrix[row + 1, column + 1] <= 7)  // sağ çaprazda taş var
				{
					move = new Move()
					{
						Column = column + 1,
						Row = row + 1,
						Message = isUpgrade ? "Upgrade" : "",
					};
					moveList.Add(move);
				}
			}
			else if (column == 7 && matrix[row + 1, column - 1] != 0 && matrix[row + 1, column - 1] <= 7) // sol çaprazda taş var
			{
				move = new Move()
				{
					Column = column - 1,
					Row = row + 1,
					Message = isUpgrade ? "Upgrade" : "",
				};
				moveList.Add(move);
			}
			else if (column == 0 && matrix[row + 1, column + 1] != 0 && matrix[row + 1, column + 1] <= 7)  // sağ çaprazda taş var
			{
				move = new Move()
				{
					Column = column + 1,
					Row = row + 1,
					Message = isUpgrade ? "Upgrade" : "",
				};
				moveList.Add(move);
			}
		}
	}
}
