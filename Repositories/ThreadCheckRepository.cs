using ChessTable.Classes;
using ChessTable.Interfaces;

namespace ChessTable.Repositories
{
	public class ThreadCheckRepository : IThreadCheckDal
	{
		public Checker CheckBishop(byte[,] m, int row, int col, bool checkWhitePieces)              // Eğer kareyi tehdit etmiyorsa true döndürür
		{
			Checker checker = new Checker();
			checker.IsCheck = true;
			checker.Row = -1;
			checker.Col = -1;
			if (checkWhitePieces)
			{
				int i = row - 1, j = col - 1;
				while (i >= 0 && j >= 0)            // sol üst çapraz
				{
					if (m[i, j] == 4)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)      // arayı kesen taş var
					{
						break;
					}
					i--;
					j--;
				}

				i = row - 1;
				j = col + 1;
				while (i >= 0 && j <= 7)            // sağ üst çapraz
				{
					if (m[i, j] == 4)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)      // arayı kesen taş var
					{
						break;
					}
					i--;
					j++;
				}

				i = row + 1;
				j = col - 1;
				while (i <= 7 && j >= 0)            // sol alt çapraz
				{
					if (m[i, j]  == 4)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)		// arayı kesen taş var
					{
						break;
					}
					i++;
					j--;
				}

				i = row + 1;
				j = col + 1;
				while (i <= 7 && j <= 7)            // sağ alt çapraz
				{
					if (m[i, j]  == 4)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)		// arayı kesen taş var
					{
						break;
					}
					i++;
					j++;
				}
			}
			else
			{
				int i = row - 1, j = col - 1;
				while (i >= 0 && j >= 0)            // sol üst çapraz
				{
					if (m[i, j] == 11)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)      // arayı kesen taş var
					{
						break;
					}
					i--;
					j--;
				}

				i = row - 1;
				j = col + 1;
				while (i >= 0 && j <= 7)            // sağ üst çapraz
				{
					if (m[i, j] == 11)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)      // arayı kesen taş var
					{
						break;
					}
					i--;
					j++;
				}

				i = row + 1;
				j = col - 1;
				while (i <= 7 && j >= 0)            // sol alt çapraz
				{
					if (m[i, j] == 11)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)      // arayı kesen taş var
					{
						break;
					}
					i++;
					j--;
				}

				i = row + 1;
				j = col + 1;
				while (i <= 7 && j <= 7)            // sağ alt çapraz
				{
					if (m[i, j] == 11)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)      // arayı kesen taş var
					{
						break;
					}
					i++;
					j++;
				}
			}
			checker.IsCheck = false;
			return checker;
		}

		public bool CheckKing(byte[,] m, int row, int col, bool checkWhitePieces)
		{
			int[,] squares = { { row - 1, col - 1 }, { row - 1, col }, { row - 1, col + 1 }, { row, col - 1 }, { row + 1, col - 1 }, { row + 1, col }, { row + 1, col + 1 }, { row, col + 1 } };
			// maximum 8 kare var
			if (checkWhitePieces)
			{
				for (int i = 0; i < 8; i++)
				{
					if (CheckSquare(squares[i, 0], squares[i, 1]) && m[squares[i, 0], squares[i, 1]] == 7)
					{
						return false;
					}
				}
			}
			else
			{
				for (int i = 0; i < 8; i++)
				{
					if (CheckSquare(squares[i, 0], squares[i, 1]) && m[squares[i, 0], squares[i, 1]] == 14)
					{
						return false;
					}
				}
			}
			return true;
		}

		public Checker CheckKnight(byte[,] m, int row, int col, bool checkWhitePieces)
		{
			Checker checker = new Checker();
			checker.IsCheck = true;
			checker.Row = -1;
			checker.Col = -1;
			int[,] squares = { { row - 2, col + 1 }, { row - 1, col + 2 }, { row + 1, col + 2 }, { row + 2, col + 1 }, { row - 2, col - 1 }, { row - 1, col - 2 }, { row + 1, col - 2 }, { row + 2, col - 1 } };
			// maximum 8 kare var
			if (checkWhitePieces)
			{
				for (int i = 0; i < 8; i++)
				{
					if (CheckSquare(squares[i, 0], squares[i, 1]) && m[squares[i, 0], squares[i, 1]] == 3)
					{
						checker.Row = squares[i, 0];
						checker.Col = squares[i, 1];
						return checker;
					}
				}
			}
			else
			{
				for (int i = 0; i < 8; i++)
				{
					if (CheckSquare(squares[i, 0], squares[i, 1]) && m[squares[i, 0], squares[i, 1]] == 10)
					{
						checker.Row = squares[i, 0];
						checker.Col = squares[i, 1];
						return checker;
					}
				}
			}
			checker.IsCheck = false;
			return checker;
		}

		public Checker CheckPawn(byte[,] m, int row, int col, bool checkWhitePieces)
		{
			Checker checker = new Checker();
			checker.IsCheck = true;
			checker.Row = -1;
			checker.Col = -1;
			if (checkWhitePieces)
			{
				// sol alt çapraz
				if (row + 1 <= 7 && col - 1 >= 0 && (m[row + 1, col - 1] == 1 || m[row + 1, col - 1] == 2))       // beyaz piyon 1  veya 2 değeri alabilir
				{
					checker.Row = row + 1;
					checker.Col = col - 1;
					return checker;
				}
				// sağ alt çapraz
				if (row + 1 <= 7 && col + 1 <= 7 && (m[row + 1, col + 1] == 1 || m[row + 1, col + 1] == 2))       // beyaz piyon 1  veya 2 değeri alabilir
				{
					checker.Row = row + 1;
					checker.Col = col + 1;
					return checker;
				}
			}
			else
			{
				// sol üst çapraz
				if (row - 1 >= 0 && col - 1 >= 0 && (m[row - 1, col - 1] == 8 || m[row - 1, col - 1] == 9))       // siyah piyon 8  veya 9 değeri alabilir
				{
					checker.Row = row - 1;
					checker.Col = col - 1;
					return checker;
				}
				// sağ üst çapraz
				if (row - 1 >= 0 && col + 1 <= 7 && (m[row - 1, col + 1] == 8 || m[row - 1, col + 1] == 9))       // beyaz piyon 8  veya 9 değeri alabilir
				{
					checker.Row = row - 1;
					checker.Col = col + 1;
					return checker;
				}
			}
			checker.IsCheck = false;
			return checker;
		}

		public Checker CheckQueen(byte[,] m, int row, int col, bool checkWhitePieces)
		{
			Checker checker = new Checker();
			checker.IsCheck = true;
			checker.Row = -1;
			checker.Col = -1;
			if (checkWhitePieces)
			{
				// fil özelliğini kontrol et
				int i = row - 1, j = col - 1;
				while (i >= 0 && j >= 0)            // sol üst çapraz
				{
					if (m[i, j] == 6)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)  // arayı kesen taş var
					{
						break;
					}
					i--;
					j--;
				}

				i = row - 1;
				j = col + 1;
				while (i >= 0 && j <= 7)            // sağ üst çapraz
				{
					if (m[i, j] == 6)      // arayı kesen siyah taş var
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)  // arayı kesen taş var
					{
						break;
					}
					i--;
					j++;
				}

				i = row + 1;
				j = col - 1;
				while (i <= 7 && j >= 0)            // sol alt çapraz
				{
					if (m[i, j] == 6)      // arayı kesen siyah taş var
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)  // arayı kesen taş var
					{
						break;
					}
					i++;
					j--;
				}

				i = row + 1;
				j = col + 1;
				while (i <= 7 && j <= 7)            // sağ alt çapraz
				{
					if (m[i, j] == 6)      // arayı kesen siyah taş var
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)  // arayı kesen taş var
					{
						break;
					}
					i++;
					j++;
				}
				// kale özelliğini kontrol et
				i = row;
				j = col - 1;
				while (j >= 0)            // sol
				{
					if (m[i, j] == 6)      // arayı kesen siyah taş var
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)  // arayı kesen taş var
					{
						break;
					}
					j--;
				}

				i = row;
				j = col + 1;
				while (j <= 7)            // sağ
				{
					if (m[i, j] == 6)      // arayı kesen siyah taş var
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)  // arayı kesen taş var
					{
						break;
					}
					j++;
				}

				i = row - 1;
				j = col;
				while (i >= 0)            // yukarı
				{
					if (m[i, j] == 6)      // arayı kesen siyah taş var
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)  // arayı kesen taş var
					{
						break;
					}
					i--;
				}

				i = row + 1;
				j = col;
				while (i <= 7)            // aşağı
				{
					if (m[i, j] == 6)      // arayı kesen siyah taş var
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)  // arayı kesen taş var
					{
						break;
					}
					i++;
				}
			}
			else
			{
				// fil özelliğini kontrol et
				int i = row - 1, j = col - 1;
				while (i >= 0 && j >= 0)            // sol üst çapraz
				{
					if (m[i, j] == 13)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)       // arayı kesen taş var
					{
						break;
					}
					i--;
					j--;
				}

				i = row - 1;
				j = col + 1;
				while (i >= 0 && j <= 7)            // sağ üst çapraz
				{
					if (m[i, j] == 13)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)       // arayı kesen taş var
					{
						break;
					}
					i--;
					j++;
				}

				i = row + 1;
				j = col - 1;
				while (i <= 7 && j >= 0)            // sol alt çapraz
				{
					if (m[i, j] == 13)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)       // arayı kesen taş var
					{
						break;
					}
					i++;
					j--;
				}

				i = row + 1;
				j = col + 1;
				while (i <= 7 && j <= 7)            // sağ alt çapraz
				{
					if (m[i, j] == 13)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)       // arayı kesen taş var
					{
						break;
					}
					i++;
					j++;
				}
				// kale özelliğini kontrol et
				i = row;
				j = col - 1;
				while (j >= 0)            // sol
				{
					if (m[i, j] == 13)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)       // arayı kesen taş var
					{
						break;
					}
					j--;
				}

				i = row;
				j = col + 1;
				while (j <= 7)            // sağ
				{
					if (m[i, j] == 13)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)       // arayı kesen taş var
					{
						break;
					}
					j++;
				}

				i = row - 1;
				j = col;
				while (i >= 0)            // yukarı
				{
					if (m[i, j] == 13)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)       // arayı kesen taş var
					{
						break;
					}
					i--;
				}

				i = row + 1;
				j = col;
				while (i <= 7)            // aşağı
				{
					if (m[i, j] == 13)
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0)       // arayı kesen taş var
					{
						break;
					}
					i++;
				}
			}
			checker.IsCheck = false;
			return checker;
		}

		public Checker CheckRook(byte[,] m, int row, int col, bool checkWhitePieces)
		{
			Checker checker = new Checker();
			checker.IsCheck = true;
			checker.Row = -1;
			checker.Col = -1;
			if (checkWhitePieces)
			{
				int i = row, j = col - 1;
				while (j >= 0)            // sol
				{
					if (m[i, j] == 5)      // arayı kesen siyah taş var
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0) // arayı kesen taş var
					{
						break;
					}
					j--;
				}

				i = row;
				j = col + 1;
				while (j <= 7)            // sağ
				{
					if (m[i, j] == 5)      // arayı kesen siyah taş var
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0) // arayı kesen taş var
					{
						break;
					}
					j++;
				}

				i = row - 1;
				j = col;
				while (i >= 0)            // yukarı
				{
					if (m[i, j] == 5)      // arayı kesen siyah taş var
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0) // arayı kesen taş var
					{
						break;
					}
					i--;
				}

				i = row + 1;
				j = col;
				while (i <= 7)            // aşağı
				{
					if (m[i, j] == 5)      // arayı kesen siyah taş var
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0) // arayı kesen taş var
					{
						break;
					}
					i++;
				}
			}
			else
			{
				int i = row, j = col - 1;
				while (j >= 0)            // sol
				{
					if (m[i, j] == 12)      // arayı kesen siyah taş var
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0) // arayı kesen taş var
					{
						break;
					}
					j--;
				}

				i = row;
				j = col + 1;
				while (j <= 7)            // sağ
				{
					if (m[i, j] == 12)      // arayı kesen siyah taş var
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0) // arayı kesen taş var
					{
						break;
					}
					j++;
				}

				i = row - 1;
				j = col;
				while (i >= 0)            // yukarı
				{
					if (m[i, j] == 12)      // arayı kesen siyah taş var
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0) // arayı kesen taş var
					{
						break;
					}
					i--;
				}

				i = row + 1;
				j = col;
				while (i <= 7)            // aşağı
				{
					if (m[i, j] == 12)      // arayı kesen siyah taş var
					{
						checker.Row = i;
						checker.Col = j;
						return checker;
					}
					else if (m[i, j] != 0) // arayı kesen taş var
					{
						break;
					}
					i++;
				}
			}
			checker.IsCheck = false;
			return checker;
		}

		public bool CheckSquare(int row, int col)
		{
			if (row >= 0 && col >= 0 && row <= 7 && col <= 7)
			{
				return true;
			}
			return false;
		}

		/*
		 Returns:
			0 -> Hareket ettirilebilir
			1 -> Soldan açmazda
			2 -> Sağdan açmazda
			3 -> Üstten açmazda
			4 -> Alttan açmazda
			5 -> Sağ alttan açmazda
			6 -> Sağ üstten açmazda
			7 -> Sol üstten açmazda
			8 -> Sol alttan açmazda
		*/
		public int IsMovable(byte[,] m, int pieceRow, int pieceColumn, int kingRow, int kingCol, bool isWhite)
		{
			if (isWhite)
			{
				int type = TraceToKing(m, pieceRow, pieceColumn, kingRow, kingCol, isWhite);
				switch (type)
				{
					case 0:
						return 0;
					case 1:
						pieceColumn--;
						while (pieceColumn >= 0)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // solunda siyah taş var
							{
								if (m[pieceRow, pieceColumn] == 12 || m[pieceRow, pieceColumn] == 13) // kale veya vezir var
								{
									return 1;
								}
								return 0;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda kendi taşı var
							{
								return 0;
							}
							pieceColumn--;
						}
						return 0; // bir şeye çarpamdan çıktık
					case 2:
						pieceColumn++;
						while (pieceColumn <= 7)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sağında siyah taş var
							{
								if (m[pieceRow, pieceColumn] == 12 || m[pieceRow, pieceColumn] == 13) // kale veya vezir var
								{
									return 2;
								}
								return 0;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // sağında kendi taşı var
							{
								return 0;
							}
							pieceColumn++;
						}
						return 0;
					case 3:
						pieceRow--;
						while (pieceRow >= 0)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // üstünde siyah taş var
							{
								if (m[pieceRow, pieceColumn] == 12 || m[pieceRow, pieceColumn] == 13) // kale veya vezir var
								{
									return 3;
								}
								return 0;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // üstünde kendi taşı var
							{
								return 0;
							}
							pieceRow--;
						}
						return 0;
					case 4:
						pieceRow++;
						while (pieceRow <= 7)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // altında siyah taş var
							{
								if (m[pieceRow, pieceColumn] == 12 || m[pieceRow, pieceColumn] == 13) // kale veya vezir var
								{
									return 4;
								}
								return 0;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // altında kendi taşı var
							{
								return 0;
							}
							pieceRow++;
						}
						return 0;
					case 5:
						pieceRow++;
						pieceColumn++;
						while (pieceRow <= 7 && pieceColumn <= 7)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sağ alt çaprazda siyah taş var
							{
								if (m[pieceRow, pieceColumn] == 11 || m[pieceRow, pieceColumn] == 13) // fil veya vezir var
								{
									return 5;
								}
								return 0;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // sağ alt çaprazda kendi taşı var
							{
								return 0;
							}
							pieceRow++;
							pieceColumn++;
						}
						return 0;
					case 6:
						pieceRow--;
						pieceColumn++;
						while (pieceRow >= 0 && pieceColumn <= 7)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sağ üst çaprazda siyah taş var
							{
								if (m[pieceRow, pieceColumn] == 11 || m[pieceRow, pieceColumn] == 13) // fil veya vezir var
								{
									return 6;
								}
								return 0;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // sağ üst çaprazda kendi taşı var
							{
								return 0;
							}
							pieceRow--;
							pieceColumn++;
						}
						return 0;
					case 7:
						pieceRow--;
						pieceColumn--;
						while (pieceRow >= 0 && pieceColumn >= 0)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sol üst çaprazda siyah taş var
							{
								if (m[pieceRow, pieceColumn] == 11 || m[pieceRow, pieceColumn] == 13) // fil veya vezir var
								{
									return 7;
								}
								return 0;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // sağ üst çaprazda kendi taşı var
							{
								return 0;
							}
							pieceRow--;
							pieceColumn--;
						}
						return 0;
					case 8:
						pieceRow++;
						pieceColumn--;
						while (pieceRow <= 7 && pieceColumn >= 0)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sol alt çaprazda siyah taş var
							{
								if (m[pieceRow, pieceColumn] == 11 || m[pieceRow, pieceColumn] == 13) // fil veya vezir var
								{
									return 8;
								}
								return 0;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // sağ üst çaprazda kendi taşı var
							{
								return 0;
							}
							pieceRow++;
							pieceColumn--;
						}
						return 0;
				}
			}
			else
			{
				int type = TraceToKing(m, pieceRow, pieceColumn, kingRow, kingCol, isWhite);
				switch (type)
				{
					case 0:
						return 0;
					case 1:
						pieceColumn--;
						while(pieceColumn >= 0)
						{
							if(m[pieceRow, pieceColumn] >= 8)  // solunda kendi taşı taş var
							{
								return 0;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda beyaz taş var
							{
								if (m[pieceRow, pieceColumn] == 5 || m[pieceRow, pieceColumn] == 6) // kale veya vezir var
								{
									return 1;
								}
								return 0;
							}
							pieceColumn--;
						}
						return 0; // bir şeye çarpamdan çıktık
					case 2:
						pieceColumn++;
						while (pieceColumn <= 7)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sağında kendi taşı var
							{
								return 0;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda beyaz taş var
							{
								if (m[pieceRow, pieceColumn] == 5 || m[pieceRow, pieceColumn] == 6) // kale veya vezir var
								{
									return 2;
								}
								return 0;
							}
							pieceColumn++;
						}
						return 0;
					case 3:
						pieceRow--;
						while (pieceRow >= 0)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // üstünde kendi taşı var
							{
								return 0;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda beyaz taş var
							{
								if (m[pieceRow, pieceColumn] == 5 || m[pieceRow, pieceColumn] == 6) // kale veya vezir var
								{
									return 3;
								}
								return 0;
							}
							pieceRow--;
						}
						return 0;
					case 4:
						pieceRow++;
						while (pieceRow <= 7)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // altında kendi taşı var
							{
								return 0;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda beyaz taş var
							{
								if (m[pieceRow, pieceColumn] == 5 || m[pieceRow, pieceColumn] == 6) // kale veya vezir var
								{
									return 4;
								}
								return 0;
							}
							pieceRow++;
						}
						return 0;
					case 5:
						pieceRow++;
						pieceColumn++;
						while (pieceRow <= 7 && pieceColumn <= 7)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sağ alt çaprazda kendi taşı var
							{
								return 0;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda beyaz taş var
							{
								if (m[pieceRow, pieceColumn] == 4 || m[pieceRow, pieceColumn] == 6) // fil veya vezir var
								{
									return 5;
								}
								return 0;
							}
							pieceRow++;
							pieceColumn++;
						}
						return 0;
					case 6:
						pieceRow--;
						pieceColumn++;
						while (pieceRow >= 0 && pieceColumn <= 7)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sağ üst çaprazda kendi taşı var
							{
								return 0;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda beyaz taş var
							{
								if (m[pieceRow, pieceColumn] == 4 || m[pieceRow, pieceColumn] == 6) // fil veya vezir var
								{
									return 6;
								}
								return 0;
							}
							pieceRow--;
							pieceColumn++;
						}
						return 0;
					case 7:
						pieceRow--;
						pieceColumn--;
						while (pieceRow >= 0 && pieceColumn >= 0)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sol üst çaprazda kendi taşı var
							{
								return 0;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda beyaz taş var
							{
								if (m[pieceRow, pieceColumn] == 4 || m[pieceRow, pieceColumn] == 6) // fil veya vezir var
								{
									return 7;
								}
								return 0;
							}
							pieceRow--;
							pieceColumn--;
						}
						return 0;
					case 8:
						pieceRow++;
						pieceColumn--;
						while (pieceRow <= 7 && pieceColumn >= 0)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sol alt çaprazda kendi taşı var
							{
								return 0;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda beyaz taş var
							{
								if (m[pieceRow, pieceColumn] == 4 || m[pieceRow, pieceColumn] == 6) // fil veya vezir var
								{
									return 8;
								}
								return 0;
							}
							pieceRow++;
							pieceColumn--;
						}
						return 0;
				}
			}
			return 0;  // Buraya hiç bir durumda düşmemesi lazım ama compiler bilmediği için hata veriyor
		}

		/*
		 Returns:
			0 -> Şahtan aranan taşa yol yok
			1 -> Taş şahın solunda
			2 -> Taş şahın sağında
			3 -> Taş şahın üstünde
			4 -> Taş şahın altında
			5 -> Taş şahın sağ alt çaprazında
			6 -> Taş şahın sağ üst çaprazında
			7 -> Taş şahın sol üst çaprazında
			8 -> Taş şahın sol alt çaprazında
		*/
		public int TraceToKing(byte[,] m, int pieceRow, int pieceColumn, int kingRow, int kingCol, bool isWhite)
		{
			int rowDiff = pieceRow - kingRow;
			int columnDiff = pieceColumn - kingCol;
			bool isDiffEqual = rowDiff == columnDiff; // fark eşitse true
			if (rowDiff == -1 * columnDiff)  // mutlak olarak eşitse de true olmalı
			{   
				isDiffEqual = true;
			}

			if (pieceRow == kingRow && pieceColumn < kingCol)  // şahın solunda
			{
				pieceColumn++;
				while (pieceColumn <= kingCol)
				{
					if (pieceColumn == kingCol)
					{
						return 1;
					}
					else if (m[pieceRow, pieceColumn] != 0)
					{
						return 0;
					}
					pieceColumn++;
				}
			}
			else if (pieceRow == kingRow && pieceColumn > kingCol)  // şahın sağında
			{
				pieceColumn--;
				while (pieceColumn >= kingCol)
				{
					if (pieceColumn == kingCol)
					{
						return 2;
					}
					else if (m[pieceRow, pieceColumn] != 0)
					{
						return 0;
					}
					pieceColumn--;
				}
			}
			else if (pieceRow < kingRow && pieceColumn == kingCol)  // şahın üstünde
			{
				pieceRow++;
				while (pieceRow <= kingRow)
				{
					if (pieceRow == kingRow)
					{
						return 3;
					}
					else if (m[pieceRow, pieceColumn] != 0)
					{
						return 0;
					}
					pieceRow++;
				}
			}
			else if (pieceRow > kingRow && pieceColumn == kingCol)  // şahın altında
			{
				pieceRow--;
				while (pieceRow >= kingRow)
				{
					if (pieceRow == kingRow)
					{
						return 4;
					}
					else if (m[pieceRow, pieceColumn] != 0)
					{
						return 0;
					}
					pieceRow--;
				}
			}
			else if (isDiffEqual)   // şahın çaprazında
			{
				if (rowDiff > 0 && columnDiff > 0)   // sağ alt çaprazda
				{
					pieceRow--;
					pieceColumn--;
					while (pieceRow >= kingRow)
					{
						if (pieceRow == kingRow)
						{
							return 5;
						}
						else if (m[pieceRow, pieceColumn] != 0)
						{
							return 0;
						}
						pieceRow--;
						pieceColumn--;
					}
				}
				else if (rowDiff < 0 && columnDiff > 0) // sağ üst çaprazda
				{
					pieceRow++;
					pieceColumn--;
					while (pieceRow <= kingRow)
					{
						if (pieceRow == kingRow)
						{
							return 6;
						}
						else if (m[pieceRow, pieceColumn] != 0)
						{
							return 0;
						}
						pieceRow++;
						pieceColumn--;
					}
				}
				else if (rowDiff < 0 && columnDiff < 0)  // sol üst çaprazda
				{
					pieceRow++;
					pieceColumn++;
					while (pieceRow <= kingRow)
					{
						if (pieceRow == kingRow)
						{
							return 7;
						}
						else if (m[pieceRow, pieceColumn] != 0)
						{
							return 0;
						}
						pieceRow++;
						pieceColumn++;
					}
				}
				else if (rowDiff > 0 && columnDiff < 0)  // sol alt çarpazda
				{
					pieceRow--;
					pieceColumn++;
					while (pieceRow >= kingRow)
					{
						if (pieceRow == kingRow)
						{
							return 8;
						}
						else if (m[pieceRow, pieceColumn] != 0)
						{
							return 0;
						}
						pieceRow--;
						pieceColumn++;
					}
				}
			}
			return 0;
		}
	}
}
