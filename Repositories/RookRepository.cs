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
			ThreadCheckRepository threadCheckRepository = new ThreadCheckRepository();
			List<Move> possibleMoves = new List<Move>();
			Move move;
			byte[,] matrix = board.BoardMatrix;
			if (threadCheckRepository.IsMovable(matrix, row, column, isWhite ? board.WhiteKing.Row : board.BlackKing.Row, isWhite ? board.WhiteKing.Col : board.BlackKing.Col, isWhite))
			{
				// sağ, sol, yukarı ve aşağı kontrol edilecek
				if (isWhite)
				{
					var message = "";
					if(row == 7 && column == 7 && !board.IsWhiteShortRookMoved) // kısa rok kalesini ilk hamlesi olucak
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
			}
			return possibleMoves;
		}
	}
}
