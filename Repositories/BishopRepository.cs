using ChessTable.Classes;
using ChessTable.Interfaces;
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
			ThreadCheckRepository threadCheckRepository = new ThreadCheckRepository();
			List<Move> possibleMoves = new List<Move>();
			Move move;
			byte[,] matrix = board.BoardMatrix;
			if(threadCheckRepository.IsMovable(matrix, row, column, isWhite ? board.WhiteKing.Row : board.BlackKing.Row, isWhite ? board.WhiteKing.Col : board.BlackKing.Col, isWhite))
			{
				// 4 çapraz kontrol edilecek
				if (isWhite)
				{
					int i = row-1, j = column-1;
					while(i >= 0 && j >= 0)			// sol üst çapraz
					{
						if(matrix[i, j] == 0)		// boş kare
						{
							move = new Move()
							{
								Column = j,
								Row = i,
								Message = "",
							};
							possibleMoves.Add(move); 
						}
						else if(matrix[i, j] >= 8)	// rakip taş var 
						{
							move = new Move() { 
								Column = j,
								Row = i,
								Message = "",
							};
							possibleMoves.Add(move);
							i = -2;   // taşa çarptıysak gerisine bakamya gerek yok
						}
						else if(matrix[i, j] <= 7)  // kendi taşı var
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
						if (matrix[i, j] == 0)			// boş kare
						{
							move = new Move()
							{
								Column = j,
								Row = i,
								Message = "",
							};
							possibleMoves.Add(move);
						}
						else if(matrix[i, j] <= 7)		// rakip taş var
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
			}
			return possibleMoves;
		}
	}
}
