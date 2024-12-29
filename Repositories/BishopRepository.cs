using ChessTable.Classes;
using ChessTable.Interfaces;
using System;
using System.Collections.Generic;

namespace ChessTable.Repositories
{
	public class BishopRepository : IBishopDal
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
						if(eatingMove != null)
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
			foreach(Move normalMove in normalMoves)
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
					foreach(Move move in normalMoves)
					{
						if(move.Row == checker.Row && move.Column > checker.Col)
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
			if (rowDiff == colDiff || rowDiff == -colDiff) // şah çeken taş çaprazda 
			{
				var moves = GetNormalMoves(board, row, col, isWhite);
				foreach(Move move in moves)
				{
					if(move.Row == checker.Row && move.Column == checker.Col)
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
			int type = threadCheckRepository.IsMovable(board.BoardMatrix, row, column, isWhite ? board.WhiteKing.Row : board.BlackKing.Row, isWhite ? board.WhiteKing.Col : board.BlackKing.Col, isWhite);
			switch (type)
			{
				case 0:
					// 4 çapraz kontrol edilecek
					if (isWhite)
					{
						int i = row - 1, j = column - 1;
						while (i >= 0 && j >= 0)            // sol üst çapraz
						{
							if (matrix[i, j] == 0)      // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] >= 8) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								i = -2;   // taşa çarptıysak gerisine bakamya gerek yok
							}
							else if (matrix[i, j] <= 7)  // kendi taşı var
							{
								i = -2;
							}
							i--;
							j--;
						}

						i = row - 1;
						j = column + 1;
						while (i >= 0 && j <= 7)            // sağ üst çapraz
						{
							if (matrix[i, j] == 0)      // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] >= 8) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								i = -2;
							}
							else if (matrix[i, j] <= 7)  // kendi taşı var
							{
								i = -2;
							}
							i--;
							j++;
						}

						i = row + 1;
						j = column - 1;
						while (i <= 7 && j >= 0)            // sol alt çapraz
						{
							if (matrix[i, j] == 0)      // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] >= 8) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								i = 9;
							}
							else if (matrix[i, j] <= 7)  // kendi taşı var
							{
								i = 9;
							}
							i++;
							j--;
						}

						i = row + 1;
						j = column + 1;
						while (i <= 7 && j <= 7)            // sağ alt çapraz
						{
							if (matrix[i, j] == 0)      // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] >= 8) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								i = 9;
							}
							else if (matrix[i, j] <= 7)  // kendi taşı var
							{
								i = 9;
							}
							i++;
							j++;
						}
					}
					else
					{
						int i = row - 1, j = column - 1;
						while (i >= 0 && j >= 0)            // sol üst çapraz
						{
							if (matrix[i, j] == 0)          // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] <= 7)     // rakip taş var
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								i = -2;
							}
							else if (matrix[i, j] >= 8)  // kendi taşı var
							{
								i = -2;
							}
							i--;
							j--;
						}

						i = row - 1;
						j = column + 1;
						while (i >= 0 && j <= 7)            // sağ üst çapraz
						{
							if (matrix[i, j] == 0)          // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] <= 7)     // rakip taş var
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								i = -2;
							}
							else if (matrix[i, j] >= 8)  // kendi taşı var
							{
								i = -2;
							}
							i--;
							j++;
						}

						i = row + 1;
						j = column - 1;
						while (i <= 7 && j >= 0)            // sol alt çapraz
						{
							if (matrix[i, j] == 0)          // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] <= 7)     // taş var
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								i = 9;
							}
							else if (matrix[i, j] >= 8)  // kendi taşı var
							{
								i = 9;
							}
							i++;
							j--;
						}

						i = row + 1;
						j = column + 1;
						while (i <= 7 && j <= 7)            // sağ alt çapraz
						{
							if (matrix[i, j] == 0)          // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] <= 7)     // taş var
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								i = 9;
							}
							else if (matrix[i, j] >= 8)  // kendi taşı var
							{
								i = 9;
							}
							i++;
							j++;
						}
					}
					break;
				case 1:
				case 2:
				case 3:
				case 4:
					break;
				case 5:
					if (isWhite)
					{
						int i = row + 1;
						int j = column + 1;
						while (i <= 7 && j <= 7)            // sağ alt çapraz
						{
							if (matrix[i, j] == 0)      // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] >= 8) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								i = 9;
							}
							else if (matrix[i, j] <= 7)  // kendi taşı var
							{
								i = 9;
							}
							i++;
							j++;
						}
					}
					else
					{
						int i = row + 1;
						int j = column + 1;
						while (i <= 7 && j <= 7)            // sağ alt çapraz
						{
							if (matrix[i, j] == 0)          // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] <= 7)     // taş var
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								i = 9;
							}
							else if (matrix[i, j] >= 8)  // kendi taşı var
							{
								i = 9;
							}
							i++;
							j++;
						}
					}
					break;
				case 6:
					if (isWhite)
					{
						int i = row - 1;
						int j = column + 1;
						while (i >= 0 && j <= 7)            // sağ üst çapraz
						{
							if (matrix[i, j] == 0)      // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] >= 8) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								i = -2;
							}
							else if (matrix[i, j] <= 7)  // kendi taşı var
							{
								i = -2;
							}
							i--;
							j++;
						}
					}
					else
					{
						int i = row - 1;
						int j = column + 1;
						while (i >= 0 && j <= 7)            // sağ üst çapraz
						{
							if (matrix[i, j] == 0)          // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] <= 7)     // rakip taş var
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								i = -2;
							}
							else if (matrix[i, j] >= 8)  // kendi taşı var
							{
								i = -2;
							}
							i--;
							j++;
						}
					}
					break;
				case 7:
					if (isWhite)
					{
						int i = row - 1, j = column - 1;
						while (i >= 0 && j >= 0)            // sol üst çapraz
						{
							if (matrix[i, j] == 0)      // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] >= 8) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								i = -2;   // taşa çarptıysak gerisine bakamya gerek yok
							}
							else if (matrix[i, j] <= 7)  // kendi taşı var
							{
								i = -2;
							}
							i--;
							j--;
						}
					}
					else
					{
						int i = row - 1, j = column - 1;
						while (i >= 0 && j >= 0)            // sol üst çapraz
						{
							if (matrix[i, j] == 0)          // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] <= 7)     // rakip taş var
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								i = -2;
							}
							else if (matrix[i, j] >= 8)  // kendi taşı var
							{
								i = -2;
							}
							i--;
							j--;
						}
					}
					break;
				case 8:
					if (isWhite)
					{
						int i = row + 1;
						int j = column - 1;
						while (i <= 7 && j >= 0)            // sol alt çapraz
						{
							if (matrix[i, j] == 0)      // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] >= 8) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								i = 9;
							}
							else if (matrix[i, j] <= 7)  // kendi taşı var
							{
								i = 9;
							}
							i++;
							j--;
						}
					}
					else
					{
						int i = row + 1;
						int j = column - 1;
						while (i <= 7 && j >= 0)            // sol alt çapraz
						{
							if (matrix[i, j] == 0)          // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] <= 7)     // taş var
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								i = 9;
							}
							else if (matrix[i, j] >= 8)  // kendi taşı var
							{
								i = 9;
							}
							i++;
							j--;
						}
					}
					break;
			}
			return possibleMoves;
		}
	}
}
