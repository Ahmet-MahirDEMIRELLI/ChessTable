using ChessTable.Classes;
using ChessTable.Interfaces;
using System.Collections.Generic;

namespace ChessTable.Repositories
{
	public class KingRepository : IKingDal
	{
		public int GetPoint()
		{
			return 0;
		}

		public List<Move> GetPossibleMoves(Board board, int row, int column, bool isWhite)
		{
			List<Move> possibleMoves = new List<Move>();
			List<Square> squaresToCheck = new List<Square>();
			Square square;
			Move move;
			byte[,] matrix = board.BoardMatrix;
			if (isWhite)
			{
				// özel durum
				if (row == 7 && column == 4 && !board.IsChecked && !board.IsWhiteKingMoved )		// şah başlangıç konumunda, hiç oynanmamış ve tehdit altında değil
				{
					squaresToCheck = new List<Square>();
					if (matrix[7,7] == 5 && !board.IsWhiteShortRookMoved && matrix[7, 6] == 0 && matrix[7, 5] == 0)       // kısa rok kalesi başlangıç konumunda, oynanmamış ve aradaki kareler boş
					{
						square = new Square()
						{
							Row = 7,
							Col = 6,
						};
						squaresToCheck.Add(square);
						square = new Square()
						{
							Row = 7,
							Col = 5,
						};
						squaresToCheck.Add(square);
						if (!IsUnderThread(matrix, squaresToCheck, isWhite)) // aradaki kareler tehdit altında değil
						{
							move = new Move()
							{
								Column = 6,
								Row = 7,
								Message = "Short Castle",
							};
							possibleMoves.Add(move);
						}
					}
					if (matrix[7, 0] == 5 && !board.IsWhiteLongRookMoved && matrix[7, 3] == 0 && matrix[7, 2] == 0 && matrix[7, 1] == 0)      // uzun rok kalesi başlangıç konumunda, oynanmamış  ve aradaki kareler boş
					{
						square = new Square()
						{
							Row = 7,
							Col = 1,
						};
						squaresToCheck.Add(square);
						square = new Square()
						{
							Row = 7,
							Col = 2,
						};
						squaresToCheck.Add(square);
						square = new Square()
						{
							Row = 7,
							Col = 3,
						};
						squaresToCheck.Add(square);
						if (!IsUnderThread(matrix, squaresToCheck, isWhite))
						{
							move = new Move()
							{
								Column = 2,
								Row = 7,
								Message = "Long Castle",
							};
							possibleMoves.Add(move);
						}
					}
				}
				// genel durum (max 8 kare)
				// sağ
				if (column + 1 <= 7 && (matrix[row, column+1] == 0 || matrix[row, column+1] >= 8)) // sağ boş veya rakip taş var
				{
					square = new Square()
					{
						Row = row,
						Col = column+1,
					};
					squaresToCheck.Add(square);
					if(!IsUnderThread(matrix, squaresToCheck, isWhite))  // kare korunmada değil
					{
						move = new Move()
						{
							Column = column+1,
							Row = row,
							Message = "White King Moved",
						};
						possibleMoves.Add(move);
					}
				}
				// sol
				if (column - 1 >= 0 && (matrix[row, column-1] == 0 || matrix[row, column-1] >= 8)) // sol boş veya rakip taş var
				{
					square = new Square()
					{
						Row = row,
						Col = column - 1,
					};
					squaresToCheck.Add(square);
					if (!IsUnderThread(matrix, squaresToCheck, isWhite))  // kare korunmada değil
					{
						move = new Move()
						{
							Column = column - 1,
							Row = row,
							Message = "White King Moved",
						};
						possibleMoves.Add(move);
					}
				}
				// yukarı
				if (row - 1 >= 0 && (matrix[row-1, column] == 0 || matrix[row-1, column] >= 8)) // sağ boş veya rakip taş var
				{
					square = new Square()
					{
						Row = row-1,
						Col = column,
					};
					squaresToCheck.Add(square);
					if (!IsUnderThread(matrix, squaresToCheck, isWhite))  // kare korunmada değil
					{
						move = new Move()
						{
							Column = column,
							Row = row-1,
							Message = "White King Moved",
						};
						possibleMoves.Add(move);
					}
				}
				// aşağı
				if (row + 1 <= 7 && (matrix[row+1, column] == 0 || matrix[row+1, column] >= 8)) // sağ boş veya rakip taş var
				{
					square = new Square()
					{
						Row = row+1,
						Col = column,
					};
					squaresToCheck.Add(square);
					if (!IsUnderThread(matrix, squaresToCheck, isWhite))  // kare korunmada değil
					{
						move = new Move()
						{
							Column = column,
							Row = row+1,
							Message = "White King Moved",
						};
						possibleMoves.Add(move);
					}
				}
				// sol üst çapraz
				if (row - 1 >= 0 && column -1 >= 0 && (matrix[row-1, column-1] == 0 || matrix[row-1, column-1] >= 8)) // sağ boş veya rakip taş var
				{
					square = new Square()
					{
						Row = row-1,
						Col = column - 1,
					};
					squaresToCheck.Add(square);
					if (!IsUnderThread(matrix, squaresToCheck, isWhite))  // kare korunmada değil
					{
						move = new Move()
						{
							Column = column - 1,
							Row = row -1,
							Message = "White King Moved",
						};
						possibleMoves.Add(move);
					}
				}
				// sol alt çapraz
				if (row + 1 <= 7 && column - 1 >= 0 && (matrix[row + 1, column - 1] == 0 || matrix[row + 1, column - 1] >= 8)) // sağ boş veya rakip taş var
				{
					square = new Square()
					{
						Row = row + 1,
						Col = column - 1,
					};
					squaresToCheck.Add(square);
					if (!IsUnderThread(matrix, squaresToCheck, isWhite))  // kare korunmada değil
					{
						move = new Move()
						{
							Column = column - 1,
							Row = row + 1,
							Message = "White King Moved",
						};
						possibleMoves.Add(move);
					}
				}
				// sağ alt çapraz
				if (row + 1 <= 7 && column + 1 <= 7 && (matrix[row + 1, column + 1] == 0 || matrix[row + 1, column + 1] >= 8)) // sağ boş veya rakip taş var
				{
					square = new Square()
					{
						Row = row + 1,
						Col = column + 1,
					};
					squaresToCheck.Add(square);
					if (!IsUnderThread(matrix, squaresToCheck, isWhite))  // kare korunmada değil
					{
						move = new Move()
						{
							Column = column + 1,
							Row = row + 1,
							Message = "White King Moved",
						};
						possibleMoves.Add(move);
					}
				}
				// sağ üst çapraz
				if (row - 1 >= 0 && column + 1 <= 7 && (matrix[row - 1, column + 1] == 0 || matrix[row - 1, column + 1] >= 8)) // sağ boş veya rakip taş var
				{
					square = new Square()
					{
						Row = row - 1,
						Col = column + 1,
					};
					squaresToCheck.Add(square);
					if (!IsUnderThread(matrix, squaresToCheck, isWhite))  // kare korunmada değil
					{
						move = new Move()
						{
							Column = column + 1,
							Row = row - 1,
							Message = "White King Moved",
						};
						possibleMoves.Add(move);
					}
				}
			}
			else
			{
				// özel durum
				if (row == 0 && column == 4 && !board.IsChecked && !board.IsBlackKingMoved)        // şah başlangıç konumunda, hiç oynanmamış ve tehdit altında değil
				{
					if (matrix[0, 7] == 12 && !board.IsBlackShortRookMoved && matrix[0, 6] == 0 && matrix[0, 5] == 0)      // kısa rok kalesi başlangıç konumunda, oynanmamış ve aradaki kareler boş
					{
						square = new Square()
						{
							Row = 0,
							Col = 6,
						};
						squaresToCheck.Add(square);
						square = new Square()
						{
							Row = 0,
							Col = 5,
						};
						squaresToCheck.Add(square);
						if (!IsUnderThread(matrix, squaresToCheck, isWhite)) // aradaki kareler tehdit altında değil
						{
							move = new Move()
							{
								Column = 6,
								Row = 0,
								Message = "Short Castle",
							};
							possibleMoves.Add(move);
						}
					}
					if (matrix[0, 0] == 12 && !board.IsBlackLongRookMoved && matrix[0, 3] == 0 && matrix[0, 2] == 0 && matrix[0, 1] == 0)      // uzun rok kalesi başlangıç konumunda, oynanmamış ve aradaki kareler boş
					{
						square = new Square()
						{
							Row = 0,
							Col = 1,
						};
						squaresToCheck.Add(square);
						square = new Square()
						{
							Row = 0,
							Col = 2,
						};
						squaresToCheck.Add(square);
						square = new Square()
						{
							Row = 0,
							Col = 3,
						};
						squaresToCheck.Add(square);
						if (!IsUnderThread(matrix, squaresToCheck, isWhite))
						{
							move = new Move()
							{
								Column = 2,
								Row = 0,
								Message = "Long Castle",
							};
							possibleMoves.Add(move);
						}
					}
				}
				// genel durum (max 8 kare)
				// sağ
				if (column + 1 <= 7 && matrix[row, column + 1] <= 7) // sağ boş veya rakip taş var
				{
					square = new Square()
					{
						Row = row,
						Col = column + 1,
					};
					squaresToCheck.Add(square);
					if (!IsUnderThread(matrix, squaresToCheck, isWhite))  // kare korunmada değil
					{
						move = new Move()
						{
							Column = column + 1,
							Row = row,
							Message = "Black King Moved",
						};
						possibleMoves.Add(move);
					}
				}
				// sol
				if (column - 1 >= 0 && matrix[row, column - 1] <= 7) // sol boş veya rakip taş var
				{
					square = new Square()
					{
						Row = row,
						Col = column - 1,
					};
					squaresToCheck.Add(square);
					if (!IsUnderThread(matrix, squaresToCheck, isWhite))  // kare korunmada değil
					{
						move = new Move()
						{
							Column = column - 1,
							Row = row,
							Message = "Black King Moved",
						};
						possibleMoves.Add(move);
					}
				}
				// yukarı
				if (row - 1 >= 0 && matrix[row - 1, column] <= 7) // sağ boş veya rakip taş var
				{
					square = new Square()
					{
						Row = row - 1,
						Col = column,
					};
					squaresToCheck.Add(square);
					if (!IsUnderThread(matrix, squaresToCheck, isWhite))  // kare korunmada değil
					{
						move = new Move()
						{
							Column = column,
							Row = row - 1,
							Message = "Black King Moved",
						};
						possibleMoves.Add(move);
					}
				}
				// aşağı
				if (row + 1 <= 7 && matrix[row + 1, column] <= 7) // sağ boş veya rakip taş var
				{
					square = new Square()
					{
						Row = row + 1,
						Col = column,
					};
					squaresToCheck.Add(square);
					if (!IsUnderThread(matrix, squaresToCheck, isWhite))  // kare korunmada değil
					{
						move = new Move()
						{
							Column = column,
							Row = row + 1,
							Message = "Black King Moved",
						};
						possibleMoves.Add(move);
					}
				}
				// sol üst çapraz
				if (row - 1 >= 0 && column - 1 >= 0 && matrix[row - 1, column - 1] <= 7) // sağ boş veya rakip taş var
				{
					square = new Square()
					{
						Row = row - 1,
						Col = column - 1,
					};
					squaresToCheck.Add(square);
					if (!IsUnderThread(matrix, squaresToCheck, isWhite))  // kare korunmada değil
					{
						move = new Move()
						{
							Column = column - 1,
							Row = row - 1,
							Message = "Black King Moved",
						};
						possibleMoves.Add(move);
					}
				}
				// sol alt çapraz
				if (row + 1 <= 7 && column - 1 >= 0 && matrix[row + 1, column - 1] <= 7) // sağ boş veya rakip taş var
				{
					square = new Square()
					{
						Row = row + 1,
						Col = column - 1,
					};
					squaresToCheck.Add(square);
					if (!IsUnderThread(matrix, squaresToCheck, isWhite))  // kare korunmada değil
					{
						move = new Move()
						{
							Column = column - 1,
							Row = row + 1,
							Message = "Black King Moved",
						};
						possibleMoves.Add(move);
					}
				}
				// sağ alt çapraz
				if (row + 1 <= 7 && column + 1 <= 7 && matrix[row + 1, column + 1] <= 7) // sağ boş veya rakip taş var
				{
					square = new Square()
					{
						Row = row + 1,
						Col = column + 1,
					};
					squaresToCheck.Add(square);
					if (!IsUnderThread(matrix, squaresToCheck, isWhite))  // kare korunmada değil
					{
						move = new Move()
						{
							Column = column + 1,
							Row = row + 1,
							Message = "Black King Moved",
						};
						possibleMoves.Add(move);
					}
				}
				// sağ üst çapraz
				if (row - 1 >= 0 && column + 1 <= 7 && matrix[row - 1, column + 1] <= 7) // sağ boş veya rakip taş var
				{
					square = new Square()
					{
						Row = row - 1,
						Col = column + 1,
					};
					squaresToCheck.Add(square);
					if (!IsUnderThread(matrix, squaresToCheck, isWhite))  // kare korunmada değil
					{
						move = new Move()
						{
							Column = column + 1,
							Row = row - 1,
							Message = "Black King Moved",
						};
						possibleMoves.Add(move);
					}
				}
			}

			return possibleMoves;
		}
		
		private static bool IsUnderThread(byte[,] m,  List<Square> squares, bool isWhite)
		{
			ThreadCheckRepository threadCheckRepository = new ThreadCheckRepository();
			int counter = 0;
			foreach (Square square in squares)
			{
				if (!threadCheckRepository.CheckPawn(m, square.Row, square.Col, !isWhite).IsCheck && !threadCheckRepository.CheckKnight(m, square.Row, square.Col, !isWhite).IsCheck
				&& !threadCheckRepository.CheckBishop(m, square.Row, square.Col, !isWhite).IsCheck && !threadCheckRepository.CheckRook(m, square.Row, square.Col, !isWhite).IsCheck
				&& !threadCheckRepository.CheckQueen(m, square.Row, square.Col, !isWhite).IsCheck && threadCheckRepository.CheckKing(m, square.Row, square.Col, !isWhite))
				{
					counter++;
				}
			}
			if(counter == squares.Count)     // her eleman için koşulları geçtiyse tüm kareler güvende
			{
				squares = new List<Square>();
				return false;
			}
			squares = new List<Square>();
			return true;      
		}
	}
}
