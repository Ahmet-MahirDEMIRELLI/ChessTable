using ChessTable.Classes;
using ChessTable.Interfaces;
using System.Collections.Generic;

namespace ChessTable.Repositories
{
	public class RookRepository : IRookDal
	{
		public int GetPoint()
		{
			return 5;
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
			if (rowDiff == 0 || colDiff == 0) // şah çeken kalenin kolunda
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
			int type = threadCheckRepository.IsMovable(matrix, row, column, isWhite ? board.WhiteKing.Row : board.BlackKing.Row, isWhite ? board.WhiteKing.Col : board.BlackKing.Col, isWhite);
			switch (type)
			{
				case 0:
					// sağ, sol, yukarı ve aşağı kontrol edilecek
					if (isWhite)
					{
						var message = "";
						if (row == 7 && column == 7 && !board.IsWhiteShortRookMoved) // kısa rok kalesini ilk hamlesi olucak
						{
							message = "Whites Short Rooks First Move";
						}
						else if (row == 7 && column == 0 && !board.IsWhiteLongRookMoved)
						{
							message = "Whites Long Rooks First Move";
						}
						int i = row, j = column - 1;
						while (j >= 0)            // sol
						{
							if (matrix[i, j] == 0)      // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = message,
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] >= 8) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = message,
								};
								possibleMoves.Add(move);
								j = -2;   // taşa çarptıysak gerisine bakmaya gerek yok
							}
							else if (matrix[i, j] <= 7)  // kendi taşı var
							{
								j = -2;
							}
							j--;
						}

						i = row;
						j = column + 1;
						while (j <= 7)            // sağ
						{
							if (matrix[i, j] == 0)      // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = message,
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] >= 8) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = message,
								};
								possibleMoves.Add(move);
								j = 9;
							}
							else if (matrix[i, j] <= 7)  // kendi taşı var
							{
								j = 9;
							}
							j++;
						}

						i = row - 1;
						j = column;
						while (i >= 0)            // yukarı
						{
							if (matrix[i, j] == 0)      // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = message,
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] >= 8) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = message,
								};
								possibleMoves.Add(move);
								i = -2;
							}
							else if (matrix[i, j] <= 7)  // kendi taşı var
							{
								i = -2;
							}
							i--;
						}

						i = row + 1;
						j = column;
						while (i <= 7)            // aşağı
						{
							if (matrix[i, j] == 0)      // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = message,
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] >= 8) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = message,
								};
								possibleMoves.Add(move);
								i = 9;
							}
							else if (matrix[i, j] <= 7)  // kendi taşı var
							{
								i = 9;
							}
							i++;
						}
					}
					else
					{
						var message = "";
						if (row == 0 && column == 7 && !board.IsBlackShortRookMoved) // kısa rok kalesini ilk hamlesi olucak
						{
							message = "Blacks Short Rooks First Move";
						}
						else if (row == 0 && column == 0 && !board.IsBlackLongRookMoved)
						{
							message = "Blacks Long Rooks First Move";
						}
						int i = row, j = column - 1;
						while (j >= 0)            // sol
						{
							if (matrix[i, j] == 0)      // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = message,
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] <= 7) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = message,
								};
								possibleMoves.Add(move);
								j = -2;   // taşa çarptıysak gerisine bakmaya gerek yok
							}
							else if (matrix[i, j] >= 8)  // kendi taşı var
							{
								j = -2;
							}
							j--;
						}

						i = row;
						j = column + 1;
						while (j <= 7)            // sağ
						{
							if (matrix[i, j] == 0)      // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = message,
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] <= 7) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = message,
								};
								possibleMoves.Add(move);
								j = 9;
							}
							else if (matrix[i, j] >= 8)  // kendi taşı var
							{
								j = 9;
							}
							j++;
						}

						i = row - 1;
						j = column;
						while (i >= 0)            // yukarı
						{
							if (matrix[i, j] == 0)      // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = message,
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] <= 7) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = message,
								};
								possibleMoves.Add(move);
								i = -2;
							}
							else if (matrix[i, j] >= 8)  // kendi taşı var
							{
								i = -2;
							}
							i--;
						}

						i = row + 1;
						j = column;
						while (i <= 7)            // aşağı
						{
							if (matrix[i, j] == 0)      // boş kare
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = message,
								};
								possibleMoves.Add(move);
							}
							else if (matrix[i, j] <= 7) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = message,
								};
								possibleMoves.Add(move);
								i = 9;
							}
							else if (matrix[i, j] >= 8)  // kendi taşı var
							{
								i = 9;
							}
							i++;
						}
					}
					break;
				case 1:
					if (isWhite)
					{
						int i = row, j = column - 1;
						while (j >= 0)            // sol
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
								j = -2;   // taşa çarptıysak gerisine bakmaya gerek yok
							}
							else if (matrix[i, j] <= 7)  // kendi taşı var
							{
								j = -2;
							}
							j--;
						}
					}
					else
					{
						int i = row, j = column - 1;
						while (j >= 0)            // sol
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
							else if (matrix[i, j] <= 7) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								j = -2;   // taşa çarptıysak gerisine bakmaya gerek yok
							}
							else if (matrix[i, j] >= 8)  // kendi taşı var
							{
								j = -2;
							}
							j--;
						}
					}
					break;
				case 2:
					if (isWhite)
					{
						int i = row;
						int j = column + 1;
						while (j <= 7)            // sağ
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
								j = 9;
							}
							else if (matrix[i, j] <= 7)  // kendi taşı var
							{
								j = 9;
							}
							j++;
						}
					}
					else
					{
						int i = row;
						int j = column + 1;
						while (j <= 7)            // sağ
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
							else if (matrix[i, j] <= 7) // rakip taş var 
							{
								move = new Move()
								{
									Column = j,
									Row = i,
									Message = "",
								};
								possibleMoves.Add(move);
								j = 9;
							}
							else if (matrix[i, j] >= 8)  // kendi taşı var
							{
								j = 9;
							}
							j++;
						}
					}
					break;
				case 3:
					if (isWhite)
					{
						int i = row - 1;
						int j = column;
						while (i >= 0)            // yukarı
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
						}
					}
					else
					{
						int i = row - 1;
						int j = column;
						while (i >= 0)            // yukarı
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
							else if (matrix[i, j] <= 7) // rakip taş var 
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
						}
					}
					break;
				case 4:
					if(isWhite) {
						int i = row + 1;
						int j = column;
						while (i <= 7)            // aşağı
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
						}
					}
					else
					{
						int i = row + 1;
						int j = column;
						while (i <= 7)            // aşağı
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
							else if (matrix[i, j] <= 7) // rakip taş var 
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
						}
					}
					break;
				case 5:
				case 6:
				case 7:
				case 8:
					break;
			}
			return possibleMoves;
		}
	}
}
