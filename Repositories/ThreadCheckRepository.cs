using ChessTable.Interfaces;

namespace ChessTable.Repositories
{
	public class ThreadCheckRepository : IThreadCheckDal
	{
		public bool CheckBishop(byte[,] m, int row, int col, bool checkWhitePieces)              // Eğer kareyi tehdit etmiyorsa true döndürür
		{
			if (checkWhitePieces)
			{
				int i = row - 1, j = col - 1;
				while (i >= 0 && j >= 0)            // sol üst çapraz
				{
					if (m[i, j] >= 8)      // arayı kesen siyah taş var
					{
						break; // arayı kesen taş varsa bu çapraza bakmaya gerek yok
					}
					else if (m[i, j] == 4 || m[i, j] == 6)  // beyaz fil veya vezir var
					{
						return false;
					}
					i--;
					j--;
				}

				i = row - 1;
				j = col + 1;
				while (i >= 0 && j <= 7)            // sağ üst çapraz
				{
					if (m[i, j] >= 8)      // arayı kesen siyah taş var
					{
						break;
					}
					else if (m[i, j] == 4 || m[i, j] == 6)
					{
						return false;
					}
					i--;
					j++;
				}

				i = row + 1;
				j = col - 1;
				while (i <= 7 && j >= 0)            // sol alt çapraz
				{
					if (m[i, j] >= 8)      // arayı kesen siyah taş var
					{
						break;
					}
					else if (m[i, j] == 4 || m[i, j] == 6)
					{
						return false;
					}
					i++;
					j--;
				}

				i = row + 1;
				j = col + 1;
				while (i <= 7 && j <= 7)            // sağ alt çapraz
				{
					if (m[i, j] >= 8)      // arayı kesen siyah taş var
					{
						break;
					}
					else if (m[i, j] == 4 || m[i, j] == 6)
					{
						return false;
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
					if (m[i, j] != 0 && m[i, j] <= 7)      // arayı kesen beyaz taş var
					{
						break; // arayı kesen taş varsa bu çapraza bakmaya gerek yok
					}
					else if (m[i, j] == 11 || m[i, j] == 13)  // siyah fil veya vezir var
					{
						return false;
					}
					i--;
					j--;
				}

				i = row - 1;
				j = col + 1;
				while (i >= 0 && j <= 7)            // sağ üst çapraz
				{
					if (m[i, j] != 0 && m[i, j] <= 7)      // arayı kesen beyaz taş var
					{
						break;
					}
					else if (m[i, j] == 11 || m[i, j] == 13)
					{
						return false;
					}
					i--;
					j++;
				}

				i = row + 1;
				j = col - 1;
				while (i <= 7 && j >= 0)            // sol alt çapraz
				{
					if (m[i, j] != 0 && m[i, j] <= 7)      // arayı kesen beyaz taş var
					{
						break;
					}
					else if (m[i, j] == 11 || m[i, j] == 13)
					{
						return false;
					}
					i++;
					j--;
				}

				i = row + 1;
				j = col + 1;
				while (i <= 7 && j <= 7)            // sağ alt çapraz
				{
					if (m[i, j] != 0 && m[i, j] <= 7)      // arayı kesen beyaz taş var
					{
						break;
					}
					else if (m[i, j] == 11 || m[i, j] == 13)
					{
						return false;
					}
					i++;
					j++;
				}
			}
			return true;
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

		public bool CheckKnight(byte[,] m, int row, int col, bool checkWhitePieces)
		{
			int[,] squares = { { row - 2, col + 1 }, { row - 1, col + 2 }, { row + 1, col + 2 }, { row + 2, col + 1 }, { row - 2, col - 1 }, { row - 1, col - 2 }, { row + 1, col - 2 }, { row + 2, col - 1 } };
			// maximum 8 kare var
			if (checkWhitePieces)
			{
				for (int i = 0; i < 8; i++)
				{
					if (CheckSquare(squares[i, 0], squares[i, 1]) && m[squares[i, 0], squares[i, 1]] == 3)
					{
						return false;
					}
				}
			}
			else
			{
				for (int i = 0; i < 8; i++)
				{
					if (CheckSquare(squares[i, 0], squares[i, 1]) && m[squares[i, 0], squares[i, 1]] == 10)
					{
						return false;
					}
				}
			}
			return true;
		}

		public bool CheckPawn(byte[,] m, int row, int col, bool checkWhitePieces)
		{
			if (checkWhitePieces)
			{
				// sol alt çapraz
				if (row + 1 <= 7 && col - 1 >= 0 && (m[row + 1, col - 1] == 1 || m[row + 1, col - 1] == 2))       // beyaz piyon 1  veya 2 değeri alabilir
				{
					return false;
				}
				// sağ alt çapraz
				if (row + 1 <= 7 && col + 1 <= 7 && (m[row + 1, col + 1] == 1 || m[row + 1, col + 1] == 2))       // beyaz piyon 1  veya 2 değeri alabilir
				{
					return false;
				}
			}
			else
			{
				// sol üst çapraz
				if (row - 1 >= 0 && col - 1 >= 0 && (m[row - 1, col - 1] == 8 || m[row - 1, col - 1] == 9))       // siyah piyon 8  veya 9 değeri alabilir
				{
					return false;
				}
				// sağ üst çapraz
				if (row - 1 >= 0 && col + 1 <= 7 && (m[row - 1, col + 1] == 8 || m[row - 1, col + 1] == 9))       // beyaz piyon 8  veya 9 değeri alabilir
				{
					return false;
				}
			}
			return true;
		}

		public bool CheckQueen(byte[,] m, int row, int col, bool checkWhitePieces)
		{
			return CheckBishop(m, row, col, checkWhitePieces) && CheckRook(m, row, col, checkWhitePieces);
		}

		public bool CheckRook(byte[,] m, int row, int col, bool checkWhitePieces)
		{
			if (checkWhitePieces)
			{
				int i = row, j = col - 1;
				while (j >= 0)            // sol
				{
					if (m[i, j] >= 8)      // arayı kesen siyah taş var
					{
						break;
					}
					else if (m[i, j] == 5 || m[i, j] == 6) // beyaz kale veya vezir var
					{
						return false;
					}
					j--;
				}

				i = row;
				j = col + 1;
				while (j <= 7)            // sağ
				{
					if (m[i, j] >= 8)      // arayı kesen siyah taş var
					{
						break;
					}
					else if (m[i, j] == 5 || m[i, j] == 6)
					{
						return false;
					}
					j++;
				}

				i = row - 1;
				j = col;
				while (i >= 0)            // yukarı
				{
					if (m[i, j] >= 8)      // arayı kesen siyah taş var
					{
						break;
					}
					else if (m[i, j] == 5 || m[i, j] == 6)
					{
						return false;
					}
					i--;
				}

				i = row + 1;
				j = col;
				while (i <= 7)            // aşağı
				{
					if (m[i, j] >= 8)      // arayı kesen siyah taş var
					{
						break;
					}
					else if (m[i, j] == 5 || m[i, j] == 6)
					{
						return false;
					}
					i++;
				}
			}
			else
			{
				int i = row, j = col - 1;
				while (j >= 0)            // sol
				{
					if (m[i, j] != 0 && m[i, j] <= 7)      // arayı kesen beyaz taş var
					{
						break;
					}
					else if (m[i, j] == 12 || m[i, j] == 13) // siyah kale veya vezir var
					{
						return false;
					}
					j--;
				}

				i = row;
				j = col + 1;
				while (j <= 7)            // sağ
				{
					if (m[i, j] != 0 && m[i, j] <= 7)      // arayı kesen beyaz taş var
					{
						break;
					}
					else if (m[i, j] == 12 || m[i, j] == 13)
					{
						return false;
					}
					j++;
				}

				i = row - 1;
				j = col;
				while (i >= 0)            // yukarı
				{
					if (m[i, j] != 0 && m[i, j] <= 7)      // arayı kesen beyaz taş var
					{
						break;
					}
					else if (m[i, j] == 12 || m[i, j] == 13)
					{
						return false;
					}
					i--;
				}

				i = row + 1;
				j = col;
				while (i <= 7)            // aşağı
				{
					if (m[i, j] != 0 && m[i, j] <= 7)      // arayı kesen beyaz taş var
					{
						break;
					}
					else if (m[i, j] == 12 || m[i, j] == 13)
					{
						return false;
					}
					i++;
				}
			}
			return true;
		}

		public bool CheckSquare(int row, int col)
		{
			if (row >= 0 && col >= 0 && row <= 7 && col <= 7)
			{
				return true;
			}
			return false;
		}

		public bool IsUnderCheck(byte[,] m, bool checkWhiteKing)        // şah çekiliyorsa true döndürür
		{
			int i, j;
			int row = 0, col = 0;
			if (checkWhiteKing)
			{
				i = 7;
				while (i >= 0)
				{
					j = 0;
					while (j <= 7)
					{
						if (m[i, j] == 7)
						{
							row = i;    // beyaz şahı bulduk
							col = j;
							j = 9;
							i = -2;
						}
						j++;
					}
					i--;
				}
			}
			else
			{
				i = 0;
				while (i <= 7)
				{
					j = 0;
					while (j <= 7)
					{
						if (m[i, j] == 14)
						{
							row = i;    // siyah şahı bulduk
							col = j;
							j = 9;
							i = 9;
						}
						j++;
					}
					i++;
				}
			}
			ThreadCheckRepository threadCheckRepository = new ThreadCheckRepository();
			if (threadCheckRepository.CheckPawn(m, row, col, !checkWhiteKing) && threadCheckRepository.CheckKnight(m, row, col, !checkWhiteKing)
				&& threadCheckRepository.CheckBishop(m, row, col, !checkWhiteKing) && threadCheckRepository.CheckRook(m, row, col, !checkWhiteKing)
				&& threadCheckRepository.CheckQueen(m, row, col, !checkWhiteKing))
			{
				return false;
			}
			return true;
		}
		public bool IsMovable(byte[,] m, int pieceRow, int pieceColumn, int kingRow, int kingCol, bool isWhite)
		{
			if (isWhite)
			{
				int type = TraceToKing(m, pieceRow, pieceColumn, kingRow, kingCol, isWhite);
				switch (type)
				{
					case 0:
						return true;
					case 1:
						pieceColumn--;
						while (pieceColumn >= 0)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // solunda siyah taş var
							{
								if (m[pieceRow, pieceColumn] == 12 || m[pieceRow, pieceColumn] == 13) // kale veya vezir var
								{
									return false;
								}
								return true;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda kendi taşı var
							{
								return true;
							}
							pieceColumn--;
						}
						return true; // bir şeye çarpamdan çıktık
					case 2:
						pieceColumn++;
						while (pieceColumn <= 7)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sağında siyah taş var
							{
								if (m[pieceRow, pieceColumn] == 12 || m[pieceRow, pieceColumn] == 13) // kale veya vezir var
								{
									return false;
								}
								return true;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // sağında kendi taşı var
							{
								return true;
							}
							pieceColumn++;
						}
						return true;
					case 3:
						pieceRow--;
						while (pieceRow >= 0)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // üstünde siyah taş var
							{
								if (m[pieceRow, pieceColumn] == 12 || m[pieceRow, pieceColumn] == 13) // kale veya vezir var
								{
									return false;
								}
								return true;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // üstünde kendi taşı var
							{
								return true;
							}
							pieceRow--;
						}
						return true;
					case 4:
						pieceRow++;
						while (pieceRow <= 7)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // altında siyah taş var
							{
								if (m[pieceRow, pieceColumn] == 12 || m[pieceRow, pieceColumn] == 13) // kale veya vezir var
								{
									return false;
								}
								return true;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // altında kendi taşı var
							{
								return true;
							}
							pieceRow++;
						}
						return true;
					case 5:
						pieceRow++;
						pieceColumn++;
						while (pieceRow <= 7 && pieceColumn <= 7)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sağ alt çaprazda siyah taş var
							{
								if (m[pieceRow, pieceColumn] == 11 || m[pieceRow, pieceColumn] == 13) // fil veya vezir var
								{
									return false;
								}
								return true;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // sağ alt çaprazda kendi taşı var
							{
								return true;
							}
							pieceRow++;
							pieceColumn++;
						}
						return true;
					case 6:
						pieceRow--;
						pieceColumn++;
						while (pieceRow >= 0 && pieceColumn <= 7)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sağ üst çaprazda siyah taş var
							{
								if (m[pieceRow, pieceColumn] == 11 || m[pieceRow, pieceColumn] == 13) // fil veya vezir var
								{
									return false;
								}
								return true;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // sağ üst çaprazda kendi taşı var
							{
								return true;
							}
							pieceRow--;
							pieceColumn++;
						}
						return true;
					case 7:
						pieceRow--;
						pieceColumn--;
						while (pieceRow >= 0 && pieceColumn >= 0)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sağ üst çaprazda siyah taş var
							{
								if (m[pieceRow, pieceColumn] == 11 || m[pieceRow, pieceColumn] == 13) // fil veya vezir var
								{
									return false;
								}
								return true;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // sağ üst çaprazda kendi taşı var
							{
								return true;
							}
							pieceRow--;
							pieceColumn--;
						}
						return true;
					case 8:
						pieceRow++;
						pieceColumn--;
						while (pieceRow <= 7 && pieceColumn >= 0)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sağ üst çaprazda siyah taş var
							{
								if (m[pieceRow, pieceColumn] == 11 || m[pieceRow, pieceColumn] == 13) // fil veya vezir var
								{
									return false;
								}
								return true;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // sağ üst çaprazda kendi taşı var
							{
								return true;
							}
							pieceRow++;
							pieceColumn--;
						}
						return true;
				}
			}
			else
			{
				int type = TraceToKing(m, pieceRow, pieceColumn, kingRow, kingCol, isWhite);
				switch (type)
				{
					case 0:
						return true;
					case 1:
						pieceColumn--;
						while(pieceColumn >= 0)
						{
							if(m[pieceRow, pieceColumn] >= 8)  // solunda kendi taşı taş var
							{
								return true;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda beyaz taş var
							{
								if (m[pieceRow, pieceColumn] == 5 || m[pieceRow, pieceColumn] == 6) // kale veya vezir var
								{
									return false;
								}
								return true;
							}
							pieceColumn--;
						}
						return true; // bir şeye çarpamdan çıktık
					case 2:
						pieceColumn++;
						while (pieceColumn <= 7)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sağında kendi taşı var
							{
								return true;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda beyaz taş var
							{
								if (m[pieceRow, pieceColumn] == 5 || m[pieceRow, pieceColumn] == 6) // kale veya vezir var
								{
									return false;
								}
								return true;
							}
							pieceColumn++;
						}
						return true;
					case 3:
						pieceRow--;
						while (pieceRow >= 0)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // üstünde kendi taşı var
							{
								return true;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda beyaz taş var
							{
								if (m[pieceRow, pieceColumn] == 5 || m[pieceRow, pieceColumn] == 6) // kale veya vezir var
								{
									return false;
								}
								return true;
							}
							pieceRow--;
						}
						return true;
					case 4:
						pieceRow++;
						while (pieceRow <= 7)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // altında kendi taşı var
							{
								return true;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda beyaz taş var
							{
								if (m[pieceRow, pieceColumn] == 5 || m[pieceRow, pieceColumn] == 6) // kale veya vezir var
								{
									return false;
								}
								return true;
							}
							pieceRow++;
						}
						return true;
					case 5:
						pieceRow++;
						pieceColumn++;
						while (pieceRow <= 7 && pieceColumn <= 7)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sağ alt çaprazda kendi taşı var
							{
								return true;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda beyaz taş var
							{
								if (m[pieceRow, pieceColumn] == 5 || m[pieceRow, pieceColumn] == 6) // kale veya vezir var
								{
									return false;
								}
								return true;
							}
							pieceRow++;
							pieceColumn++;
						}
						return true;
					case 6:
						pieceRow--;
						pieceColumn++;
						while (pieceRow >= 0 && pieceColumn <= 7)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sağ üst çaprazda kendi taşı var
							{
								return true;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda beyaz taş var
							{
								if (m[pieceRow, pieceColumn] == 5 || m[pieceRow, pieceColumn] == 6) // kale veya vezir var
								{
									return false;
								}
								return true;
							}
							pieceRow--;
							pieceColumn++;
						}
						return true;
					case 7:
						pieceRow--;
						pieceColumn--;
						while (pieceRow >= 0 && pieceColumn >= 0)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sağ üst çaprazda kendi taşı var
							{
								return true;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda beyaz taş var
							{
								if (m[pieceRow, pieceColumn] == 5 || m[pieceRow, pieceColumn] == 6) // kale veya vezir var
								{
									return false;
								}
								return true;
							}
							pieceRow--;
							pieceColumn--;
						}
						return true;
					case 8:
						pieceRow++;
						pieceColumn--;
						while (pieceRow <= 7 && pieceColumn >= 0)
						{
							if (m[pieceRow, pieceColumn] >= 8)  // sağ üst çaprazda kendi taşı var
							{
								return true;
							}
							else if (m[pieceRow, pieceColumn] != 0 && m[pieceRow, pieceColumn] <= 7)  // solunda beyaz taş var
							{
								if (m[pieceRow, pieceColumn] == 5 || m[pieceRow, pieceColumn] == 6) // kale veya vezir var
								{
									return false;
								}
								return true;
							}
							pieceRow++;
							pieceColumn--;
						}
						return true;
				}
			}
			return true;  // Buraya hiç bir durumda düşmemesi lazım ama compiler bilmediği için hata veriyor
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
