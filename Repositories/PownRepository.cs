using ChessTable.Classes;
using ChessTable.Interfaces;
using System.Collections.Generic;

namespace ChessTable.Repositories
{
	public class PownRepository : IPownDal
	{
		public int GetPoint()
		{
			return 1;
		}

		public List<Move> GetPossibleMoves(Board board, int row, int column, bool isWhite)
		{
			ThreadCheckRepository threadCheckRepository = new ThreadCheckRepository();
			List<Move> possibleMoves = new List<Move>();
			Move move;
			byte[,] matrix = board.BoardMatrix;
			if(threadCheckRepository.IsMovable(board.BoardMatrix, row, column, isWhite ? board.WhiteKing.Row : board.BlackKing.Row, isWhite ? board.WhiteKing.Col : board.BlackKing.Col, isWhite))
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
