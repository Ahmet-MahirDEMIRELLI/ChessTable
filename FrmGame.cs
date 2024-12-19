using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ChessTable.Classes;
using ChessTable.Helper;
using ChessTable.Helpers;
using ChessTable.Repositories;

namespace ChessTable
{
	public partial class FrmGame : Form
	{
		DataTable notationTable = new DataTable();
		private static Panel pieceToMovePanel = null; // Seçilen taşın paneli
		private static Image pieceToMoveImage = null; // Seçilen taşın görseli
		private static Game game = new Game();
		private static Move move = new Move();
		private static bool isWhitesMove;
		private static List<Move> validMoves;
		private static int pieceToMoveRow;
		private static int pieceToMoveCol;
		private static int[,] whites2List = { {-1, -1}};
		private static int[,] blacks9List = { { -1, -1 } };
		private static int whitePawn = 1;
		private static int white2Pawn = 2;
		private static int blackPawn = 8;
		private static int black2Pawn = 9;

		public FrmGame()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			InitializeChessBoard(tableLayoutPanel1, this);
			InitializeChessBoard(tableLayoutPanel2, this);
			InitializePieces.PlacePieces(tableLayoutPanel1);
			InitializeBoardMatrix();
			MatrixToPanel();
			dataGridView1.Width = 233;
			dataGridView1.DataSource = notationTable;
		}

		private static void OnCellClick(object sender, EventArgs e, TableLayoutPanel tableLayoutPanel, FrmGame formInstance)
		{
			Panel clickedPanel = (Panel)sender;

			TableLayoutPanelCellPosition position = tableLayoutPanel.GetCellPosition(clickedPanel);
			int col = position.Column;
			int row = position.Row;

			// Taş seçildikten sonra başak bir kareye tıklanınca true
			if (pieceToMovePanel != null)
			{
				if(clickedPanel != pieceToMovePanel)   // aynı kareye tıklanmadığından emin ol
				{
					bool isPlayed = false;
					position = tableLayoutPanel.GetCellPosition(clickedPanel);   // oynanmak istenen kare
					col = position.Column;
					row = position.Row;

					if ((isWhitesMove && game.GameBoard.BoardMatrix[row,col] <= 7 && game.GameBoard.BoardMatrix[row, col] != 0) || (!isWhitesMove && game.GameBoard.BoardMatrix[row, col] >= 8)) // beyaz/siyah başka taş seçti
					{
						pieceToMovePanel = clickedPanel;
						pieceToMoveImage = clickedPanel.BackgroundImage;
						Highlighter.ResetHighlightedSquares(tableLayoutPanel, validMoves, pieceToMoveRow, pieceToMoveCol);
						validMoves = null;
						pieceToMoveRow = row;
						pieceToMoveCol = col;
						HiglightPossibleMoves(game.GameBoard, row, col, tableLayoutPanel);
					}
					else
					{
						move = IsMoveValid(row, col);
						if (move != null)
						{
							// şah çekilme durumunu temizle
							game.GameBoard.IsChecked = false;
							if(move.Message != "") // Mesaj = "Upgrade", "Enpassant row,col", "ShortCastle", "Long Castle", "White King Moved", "Black King Moved" olabilir
							{
								if(move.Message == "Upgrade")
								{
									Panel upgradePanel = (Panel)tableLayoutPanel.GetControlFromPosition(col, row);
									HandleUpgrade(row, col, pieceToMoveRow, pieceToMoveCol, upgradePanel, pieceToMovePanel);
								}
								else if (move.Message.Contains("Enpassant"))
								{
									var parts = move.Message.Split(' ');    // parts[0] = Enpassant part[1] = row,col
									var coordinates = parts[1].Split(',');
									int eatenRow = int.Parse(coordinates[0]);
									int eatenCol = int.Parse(coordinates[1]);
									Panel eatenPanel = (Panel)tableLayoutPanel.GetControlFromPosition(eatenCol, eatenRow);
									HandleEnpassant(eatenRow, eatenCol, eatenPanel);
								}
								else if (move.Message.Contains("Castle"))
								{
									HandleCastle(move.Message, tableLayoutPanel);
								}
								else if(move.Message.Contains("King Moved"))
								{
									HandleKingMoved(row, col);
								}
								else if(move.Message.Contains("Rooks First Move"))
								{
									HandleRooksFirstMove(move.Message);
								}
							}

							if(game.GameBoard.BoardMatrix[row, col] != 0)   // taş yenecek
							{
								HandlePieceEating(row, col);
							}

							if (game.GameBoard.BoardMatrix[pieceToMoveRow, pieceToMoveCol] == 2)  // 2 sürülmüş beyaz piyon ile oynanacak
							{
								game.GameBoard.BoardMatrix[pieceToMoveRow, pieceToMoveCol]--;
								whites2List[0, 0] = -1;
								whites2List[0, 1] = -1;
							}
							else if (game.GameBoard.BoardMatrix[pieceToMoveRow, pieceToMoveCol] == 9)
							{
								game.GameBoard.BoardMatrix[pieceToMoveRow, pieceToMoveCol]--;
								blacks9List[0, 0] = -1;
								blacks9List[0, 1] = -1;
							}

							if(move.Message != "Upgrade") // Upgrade fonksiyonu hamle yapma işini kendi hallediyor
							{
								MakeTheMove(clickedPanel, pieceToMoveImage, pieceToMovePanel, row, col);
							}
							isPlayed = true;
						}
					}

					if (isPlayed)
					{
						// önceden 2 yapılmış piyon varsa temizle
						if (whites2List[0, 0] != -1)
						{
							game.GameBoard.BoardMatrix[whites2List[0, 0], whites2List[0, 1]]--;
							whites2List[0, 0] = -1;
							whites2List[0, 1] = -1;
						}
						// önceden 9 yapılmış piyon varsa temizle
						if (blacks9List[0, 0] != -1)
						{
							game.GameBoard.BoardMatrix[blacks9List[0, 0], blacks9List[0, 1]]--;
							blacks9List[0, 0] = -1;
							blacks9List[0, 1] = -1;
						}
						if (isWhitesMove)
						{
							// beyaz piyon 2 kare oynandıysa değeri 1 değil 2 olmalı
							if (game.GameBoard.BoardMatrix[row, col] == whitePawn && row == pieceToMoveRow - 2 && col == pieceToMoveCol)
							{
								game.GameBoard.BoardMatrix[row, col]++;
								whites2List[0, 0] = row;
								whites2List[0, 1] = col;
							}
						}
						else
						{
							// siyah piyon 2 kare oynandıysa değeri 8 değil 9 olmalı
							if (game.GameBoard.BoardMatrix[row, col] == blackPawn && row == pieceToMoveRow + 2 && col == pieceToMoveCol)
							{
								game.GameBoard.BoardMatrix[row, col]++;
								blacks9List[0, 0] = row;
								blacks9List[0, 1] = col;
							}
						}
						
						Highlighter.ResetHighlightedSquares(tableLayoutPanel, validMoves, pieceToMoveRow, pieceToMoveCol);
						// Seçimi sıfırla
						pieceToMovePanel = null;
						pieceToMoveImage = null;
						// validMoves temizle
						validMoves = null;
						// şah çekenleri temizle
						game.GameBoard.WhitesCheckers[0].Row = -1;
						game.GameBoard.WhitesCheckers[0].Col = -1;
						game.GameBoard.WhitesCheckers[1].Row = -1;
						game.GameBoard.WhitesCheckers[1].Col = -1;
						game.GameBoard.BlacksCheckers[0].Row = -1;
						game.GameBoard.BlacksCheckers[0].Col = -1;
						game.GameBoard.BlacksCheckers[1].Row = -1;
						game.GameBoard.BlacksCheckers[1].Col = -1;
						// şah çekildi mi kontrolü
						HandleIsCheckedControl();
						// hamle sırasını değiştir
						isWhitesMove = !isWhitesMove;
						formInstance.MatrixToPanel();
					}
				}
				
			}
			else if (clickedPanel.BackgroundImage != null && ((isWhitesMove && game.GameBoard.BoardMatrix[row,col] <= 7) || (!isWhitesMove && game.GameBoard.BoardMatrix[row,col] >= 8)))
			{
				// Hamle sırası olan rengin taşı seçildi
				pieceToMovePanel = clickedPanel;
				pieceToMoveImage = clickedPanel.BackgroundImage;
				pieceToMoveRow = row;
				pieceToMoveCol = col;
				// Seçilen taşın gidebileceği kareleri işaretleme
				HiglightPossibleMoves(game.GameBoard, row, col, tableLayoutPanel);
			}
		}

		private static void HandleIsCheckedControl()
		{
			ThreadCheckRepository threadCheckRepository = new ThreadCheckRepository();
			if (isWhitesMove)
			{
				Checker checkerPawn = threadCheckRepository.CheckPawn(game.GameBoard.BoardMatrix, game.GameBoard.BlackKing.Row, game.GameBoard.BlackKing.Col, isWhitesMove);
				Checker checkerKnight = threadCheckRepository.CheckKnight(game.GameBoard.BoardMatrix, game.GameBoard.BlackKing.Row, game.GameBoard.BlackKing.Col, isWhitesMove);
				Checker checkerBishop = threadCheckRepository.CheckBishop(game.GameBoard.BoardMatrix, game.GameBoard.BlackKing.Row, game.GameBoard.BlackKing.Col, isWhitesMove);
				Checker checkerRook = threadCheckRepository.CheckRook(game.GameBoard.BoardMatrix, game.GameBoard.BlackKing.Row, game.GameBoard.BlackKing.Col, isWhitesMove);
				Checker checkerQueen = threadCheckRepository.CheckQueen(game.GameBoard.BoardMatrix, game.GameBoard.BlackKing.Row, game.GameBoard.BlackKing.Col, isWhitesMove);
				bool isFirstFound = false;
				bool isSecondFound = false;
				if (checkerQueen.IsCheck)
				{
					game.GameBoard.IsChecked = true;
					game.GameBoard.WhitesCheckers[0].Row = checkerQueen.Row;
					game.GameBoard.WhitesCheckers[0].Col = checkerQueen.Col;
					isFirstFound = true;
				}

				if (checkerKnight.IsCheck)
				{
					game.GameBoard.IsChecked = true;
					if (!isFirstFound)
					{
						game.GameBoard.WhitesCheckers[0].Row = checkerKnight.Row;
						game.GameBoard.WhitesCheckers[0].Col = checkerKnight.Col;
						isFirstFound = true;
					}
					else
					{
						game.GameBoard.WhitesCheckers[1].Row = checkerKnight.Row;
						game.GameBoard.WhitesCheckers[1].Col = checkerKnight.Col;
						isSecondFound = true;
					}
				}

				if (!isSecondFound && checkerBishop.IsCheck)
				{
					game.GameBoard.IsChecked = true;
					if (!isFirstFound)
					{
						game.GameBoard.WhitesCheckers[0].Row = checkerBishop.Row;
						game.GameBoard.WhitesCheckers[0].Col = checkerBishop.Col;
						isFirstFound = true;
					}
					else
					{
						game.GameBoard.WhitesCheckers[1].Row = checkerBishop.Row;
						game.GameBoard.WhitesCheckers[1].Col = checkerBishop.Col;
						isSecondFound = true;
					}
				}

				if (!isSecondFound && checkerRook.IsCheck)
				{
					game.GameBoard.IsChecked = true;
					if (!isFirstFound)
					{
						game.GameBoard.WhitesCheckers[0].Row = checkerRook.Row;
						game.GameBoard.WhitesCheckers[0].Col = checkerRook.Col;
						isFirstFound = true;
					}
					else
					{
						game.GameBoard.WhitesCheckers[1].Row = checkerRook.Row;
						game.GameBoard.WhitesCheckers[1].Col = checkerRook.Col;
						isSecondFound = true;
					}
				}

				if (!isSecondFound && checkerPawn.IsCheck)
				{
					game.GameBoard.IsChecked = true;
					if (!isFirstFound)
					{
						game.GameBoard.WhitesCheckers[0].Row = checkerPawn.Row;
						game.GameBoard.WhitesCheckers[0].Col = checkerPawn.Col;
					}
					else
					{
						game.GameBoard.WhitesCheckers[1].Row = checkerPawn.Row;
						game.GameBoard.WhitesCheckers[1].Col = checkerPawn.Col;
					}
				}
			}
			else
			{
				Checker checkerPawn = threadCheckRepository.CheckPawn(game.GameBoard.BoardMatrix, game.GameBoard.WhiteKing.Row, game.GameBoard.WhiteKing.Col, isWhitesMove);
				Checker checkerKnight = threadCheckRepository.CheckKnight(game.GameBoard.BoardMatrix, game.GameBoard.WhiteKing.Row, game.GameBoard.WhiteKing.Col, isWhitesMove);
				Checker checkerBishop = threadCheckRepository.CheckBishop(game.GameBoard.BoardMatrix, game.GameBoard.WhiteKing.Row, game.GameBoard.WhiteKing.Col, isWhitesMove);
				Checker checkerRook = threadCheckRepository.CheckRook(game.GameBoard.BoardMatrix, game.GameBoard.WhiteKing.Row, game.GameBoard.WhiteKing.Col, isWhitesMove);
				Checker checkerQueen = threadCheckRepository.CheckQueen(game.GameBoard.BoardMatrix, game.GameBoard.WhiteKing.Row, game.GameBoard.WhiteKing.Col, isWhitesMove);
				bool isFirstFound = false;
				bool isSecondFound = false;
				if (checkerQueen.IsCheck)
				{
					game.GameBoard.IsChecked = true;
					game.GameBoard.BlacksCheckers[0].Row = checkerQueen.Row;
					game.GameBoard.BlacksCheckers[0].Col = checkerQueen.Col;
					isFirstFound = true;
				}

				if (checkerKnight.IsCheck)
				{
					game.GameBoard.IsChecked = true;
					if (!isFirstFound)
					{
						game.GameBoard.BlacksCheckers[0].Row = checkerKnight.Row;
						game.GameBoard.BlacksCheckers[0].Col = checkerKnight.Col;
						isFirstFound = true;
					}
					else
					{
						game.GameBoard.BlacksCheckers[1].Row = checkerKnight.Row;
						game.GameBoard.BlacksCheckers[1].Col = checkerKnight.Col;
						isSecondFound = true;
					}
				}

				if (!isSecondFound && checkerBishop.IsCheck)
				{
					game.GameBoard.IsChecked = true;
					if (!isFirstFound)
					{
						game.GameBoard.BlacksCheckers[0].Row = checkerBishop.Row;
						game.GameBoard.BlacksCheckers[0].Col = checkerBishop.Col;
						isFirstFound = true;
					}
					else
					{
						game.GameBoard.BlacksCheckers[1].Row = checkerBishop.Row;
						game.GameBoard.BlacksCheckers[1].Col = checkerBishop.Col;
						isSecondFound = true;
					}
				}

				if (!isSecondFound && checkerRook.IsCheck)
				{
					game.GameBoard.IsChecked = true;
					if (!isFirstFound)
					{
						game.GameBoard.BlacksCheckers[0].Row = checkerRook.Row;
						game.GameBoard.BlacksCheckers[0].Col = checkerRook.Col;
						isFirstFound = true;
					}
					else
					{
						game.GameBoard.BlacksCheckers[1].Row = checkerRook.Row;
						game.GameBoard.BlacksCheckers[1].Col = checkerRook.Col;
						isSecondFound = true;
					}
				}

				if (!isSecondFound && checkerPawn.IsCheck)
				{
					game.GameBoard.IsChecked = true;
					if (!isFirstFound)
					{
						game.GameBoard.BlacksCheckers[0].Row = checkerPawn.Row;
						game.GameBoard.BlacksCheckers[0].Col = checkerPawn.Col;
					}
					else
					{
						game.GameBoard.BlacksCheckers[1].Row = checkerPawn.Row;
						game.GameBoard.BlacksCheckers[1].Col = checkerPawn.Col;
					}
				}
			}
		}

		private static void HandleCastle(string message, TableLayoutPanel tableLayoutPanel)
		{
			if (message.Contains("Short"))        // kısa rok
			{
				if (isWhitesMove)           // beyaz kısa rok
				{
					Panel rookOld = (Panel)tableLayoutPanel.GetControlFromPosition(7, 7);
					Panel rookNew = (Panel)tableLayoutPanel.GetControlFromPosition(5, 7); // panel alırken col,row olarak alınır
					if (rookOld != null && rookNew != null)
					{
						// kale görselini taşı ve eskisini kaldır
						rookNew.BackgroundImage = rookOld.BackgroundImage;
						rookOld.BackgroundImage = null;
					}
					game.GameBoard.IsWhiteKingMoved = true;
					game.GameBoard.IsWhiteShortRookMoved = true;
					game.GameBoard.BoardMatrix[7, 5] = 5;
					game.GameBoard.BoardMatrix[7, 7] = 0;
					// Şahın yerini güncelle
					game.GameBoard.WhiteKing.Row = 7;
					game.GameBoard.WhiteKing.Col = 6;
				}
				else
				{
					Panel rookOld = (Panel)tableLayoutPanel.GetControlFromPosition(7, 0);  // panel alırken col,row olarak alınır
					Panel rookNew = (Panel)tableLayoutPanel.GetControlFromPosition(5, 0);
					if (rookOld != null && rookNew != null)
					{
						// kale görselini taşı ve eskisini kaldır
						rookNew.BackgroundImage = rookOld.BackgroundImage;
						rookOld.BackgroundImage = null;
					}
					game.GameBoard.IsBlackKingMoved = true;
					game.GameBoard.IsBlackShortRookMoved = true;
					game.GameBoard.BoardMatrix[0, 5] = 12;
					game.GameBoard.BoardMatrix[0, 7] = 0;
					// Şahın yerini güncelle
					game.GameBoard.BlackKing.Row = 0;
					game.GameBoard.BlackKing.Col = 6;
				}
			}
			else
			{
				if (isWhitesMove)           // beyaz uzun rok
				{
					Panel rookOld = (Panel)tableLayoutPanel.GetControlFromPosition(0, 7);  // panel alırken col,row olarak alınır
					Panel rookNew = (Panel)tableLayoutPanel.GetControlFromPosition(3, 7);
					if (rookOld != null && rookNew != null)
					{
						// kale görselini taşı ve eskisini kaldır
						rookNew.BackgroundImage = rookOld.BackgroundImage;
						rookOld.BackgroundImage = null;
					}
					game.GameBoard.IsWhiteKingMoved = true;
					game.GameBoard.IsWhiteLongRookMoved = true;
					game.GameBoard.BoardMatrix[7, 3] = 5;
					game.GameBoard.BoardMatrix[7, 0] = 0;
					// Şahın yerini güncelle
					game.GameBoard.WhiteKing.Row = 7;
					game.GameBoard.WhiteKing.Col = 2;
				}
				else
				{
					Panel rookOld = (Panel)tableLayoutPanel.GetControlFromPosition(0, 0);
					Panel rookNew = (Panel)tableLayoutPanel.GetControlFromPosition(3, 0);
					if (rookOld != null && rookNew != null)
					{
						// kale görselini taşı ve eskisini kaldır
						rookNew.BackgroundImage = rookOld.BackgroundImage;
						rookOld.BackgroundImage = null;
					}
					game.GameBoard.IsBlackKingMoved = true;
					game.GameBoard.IsBlackLongRookMoved = true;
					game.GameBoard.BoardMatrix[0, 3] = 12;
					game.GameBoard.BoardMatrix[0, 0] = 0;
					// Şahın yerini güncelle
					game.GameBoard.BlackKing.Row = 0;
					game.GameBoard.BlackKing.Col = 2;
				}
			}
		}

		private static void MakeTheMove(Panel clickedPanel, Image pieceToMoveImage, Panel pieceToMovePanel, int row, int col)
		{
			// Taşı yeni pozisyona yerleştir
			clickedPanel.BackgroundImage = pieceToMoveImage;
			pieceToMovePanel.BackgroundImage = null; // Eski paneli temizle
			// hamleyi matriste oyna
			game.GameBoard.BoardMatrix[row, col] = game.GameBoard.BoardMatrix[pieceToMoveRow, pieceToMoveCol];
			game.GameBoard.BoardMatrix[pieceToMoveRow, pieceToMoveCol] = 0;
		}

		private static void HandlePieceEating(int row, int col)
		{
			// Yenilecek olan taşın puanını düş
			if (isWhitesMove)
			{
				if (game.GameBoard.BoardMatrix[row, col] <= 9)  // Siyah piyon
				{
					game.GameBoard.BlacksPoints--;
				}
				else if (game.GameBoard.BoardMatrix[row, col] <= 11) // at veya fil
				{
					game.GameBoard.BlacksPoints -= 3;
				}
				else if (game.GameBoard.BoardMatrix[row, col] == 12) // kale
				{
					game.GameBoard.BlacksPoints -= 5;
				}
				else
				{
					game.GameBoard.BlacksPoints -= 9;
				}
			}
			else
			{
				if (game.GameBoard.BoardMatrix[row, col] <= 2)  // Beyaz piyon
				{
					game.GameBoard.WhitesPoints--;
				}
				else if (game.GameBoard.BoardMatrix[row, col] <= 4) // at veya fil
				{
					game.GameBoard.WhitesPoints -= 3;
				}
				else if (game.GameBoard.BoardMatrix[row, col] == 5) // kale
				{
					game.GameBoard.WhitesPoints -= 5;
				}
				else
				{
					game.GameBoard.WhitesPoints -= 9;
				}
			}

			if (game.GameBoard.BoardMatrix[row, col] == white2Pawn)         // beyazın 2 sürülmüş piyonu yenecek
			{
				whites2List[0, 0] = -1;
				whites2List[0, 1] = -1;
			}
			else if (game.GameBoard.BoardMatrix[row, col] == black2Pawn)
			{
				blacks9List[0, 0] = -1;
				blacks9List[0, 1] = -1;
			}
		}

		private static void HandleRooksFirstMove(string message)
		{
			if (move.Message.Contains("Short"))
			{
				if (!game.GameBoard.IsWhiteShortRookMoved && move.Message.Contains("White"))  // daha önce oynanmamış ise
				{
					game.GameBoard.IsWhiteShortRookMoved = true;
				}
				else if (!game.GameBoard.IsBlackShortRookMoved && move.Message.Contains("Black"))
				{
					game.GameBoard.IsBlackShortRookMoved = true;
				}
			}
			else
			{
				if (!game.GameBoard.IsWhiteLongRookMoved && move.Message.Contains("White"))  // daha önce oynanmamış ise
				{
					game.GameBoard.IsWhiteLongRookMoved = true;
				}
				else if (!game.GameBoard.IsBlackLongRookMoved && move.Message.Contains("Black"))
				{
					game.GameBoard.IsBlackLongRookMoved = true;
				}
			}
		}

		private static void HandleKingMoved(int row, int col)
		{
			// şahın yerini güncelle
			if (isWhitesMove)
			{
				game.GameBoard.WhiteKing.Row = row;
				game.GameBoard.WhiteKing.Col = col;
			}
			else
			{
				game.GameBoard.BlackKing.Row = row;
				game.GameBoard.BlackKing.Col = col;
			}
			if (!game.GameBoard.IsWhiteKingMoved && move.Message.Contains("White"))  // daha önce oynanmamış ise
			{
				game.GameBoard.IsWhiteKingMoved = true;
			}
			else if (!game.GameBoard.IsBlackKingMoved && move.Message.Contains("Black"))
			{
				game.GameBoard.IsBlackKingMoved = true;
			}
		}

		private static void HandleEnpassant(int eatenRow, int eatenCol, Panel eatenPanel)
		{
			// Yenilen taşı matristen temizle
			if (game.GameBoard.BoardMatrix[eatenRow, eatenCol] == white2Pawn)  // beyazın 2 kare oynanmış piyonu yendi
			{
				whites2List[0, 0] = -1;
				whites2List[0, 1] = -1;
				game.GameBoard.WhitesPoints--;  // Beyazın puanından piyonu düş
			}
			else if (game.GameBoard.BoardMatrix[eatenRow, eatenCol] == black2Pawn)
			{
				blacks9List[0, 0] = -1;
				blacks9List[0, 1] = -1;
				game.GameBoard.BlacksPoints--;  // Siyahın puanından piyonu düş
			}
			game.GameBoard.BoardMatrix[eatenRow, eatenCol] = 0;
			// Yenilen taşı tahtadan temizle
			if (eatenPanel != null)
			{
				eatenPanel.BackgroundImage = null;
			}
		}

		public static void HiglightPossibleMoves(Board board, int row, int col, TableLayoutPanel panel)
		{
			List<Move> moves = new List<Move>();
			PawnRepository pownRepo = new PawnRepository();
			KnightRepository knightRepo = new KnightRepository();
			BishopRepository bishopRepo = new BishopRepository();
			RookRepository rookRepo = new RookRepository();
			QueenRepository queenRepo = new QueenRepository();
			KingRepository kingRepo = new KingRepository();
			switch (game.GameBoard.BoardMatrix[row, col])
			{
				case 1:
				case 2:
					moves = pownRepo.GetPossibleMoves(game.GameBoard, row, col, true);
					break;
				case 3:
					moves = knightRepo.GetPossibleMoves(game.GameBoard, row, col, true);
					break;
				case 4:
					moves = bishopRepo.GetPossibleMoves(game.GameBoard, row, col, true);
					break;
				case 5:
					moves = rookRepo.GetPossibleMoves(game.GameBoard, row, col, true);
					break;
				case 6:
					moves = queenRepo.GetPossibleMoves(game.GameBoard, row, col, true);
					break;
				case 7:
					moves = kingRepo.GetPossibleMoves(game.GameBoard, row, col, true);
					break;
				case 8:
				case 9:
					moves = pownRepo.GetPossibleMoves(game.GameBoard, row, col, false);
					break;
				case 10:
					moves = knightRepo.GetPossibleMoves(game.GameBoard, row, col, false);
					break;
				case 11:
					moves = bishopRepo.GetPossibleMoves(game.GameBoard, row, col, false);
					break;
				case 12:
					moves = rookRepo.GetPossibleMoves(game.GameBoard, row, col, false);
					break;
				case 13:
					moves = queenRepo.GetPossibleMoves(game.GameBoard, row, col, false);
					break;
				case 14:
					moves = kingRepo.GetPossibleMoves(game.GameBoard, row, col, false);
					break;
			}
			validMoves = moves;
			Highlighter.HighlightPossibleMoves(panel, row, col, moves);
		}

		public static Move IsMoveValid(int row, int col)
		{
			foreach (Move m in validMoves)
			{
				if (m.Row == row && m.Column == col)
				{
					return m;
				}
			}
			return null;
		}

		public static void InitializeChessBoard(TableLayoutPanel tableLayoutPanel, FrmGame formInstance)
		{
			// TableLayoutPanel boyutlarını ayarlayın
			tableLayoutPanel.Width = 440;
			tableLayoutPanel.Height = 440;
			tableLayoutPanel.RowCount = 8;
			tableLayoutPanel.ColumnCount = 8;

			// TableLayoutPanel'e border (kenarlık) ekleyin
			tableLayoutPanel.BorderStyle = BorderStyle.FixedSingle; // Kenarlık ekleme

			// Satır ve sütunlar için eşit oranlı boyutlandırmayı devre dışı bırakıyoruz
			tableLayoutPanel.RowStyles.Clear();
			tableLayoutPanel.ColumnStyles.Clear();

			for (int i = 0; i < 8; i++)
			{
				tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));  // Her satır 55 piksel
				tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 55));  // Her sütun 55 piksel
			}

			for (int row = 0; row < 8; row++)
			{
				for (int col = 0; col < 8; col++)
				{
					Panel panel = new Panel
					{
						Size = new Size(55, 55),  // Panel boyutu 55x55 piksel
						BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray,
						Padding = new Padding(0),
						Margin = new Padding(0),
						BackgroundImageLayout = ImageLayout.Stretch
					};

					panel.Click += (sender, e) => OnCellClick(sender, e, tableLayoutPanel, formInstance);
					tableLayoutPanel.Controls.Add(panel, col, row);
				}
			}
		}

		public static void HandleUpgrade(int row, int col, int oldRow, int oldCol, Panel panel, Panel oldPanel)
		{
			string assetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\assets");
			assetsPath = Path.GetFullPath(assetsPath);
			using (FrmUpgradeInput upgradeForm = new FrmUpgradeInput())
			{
				if (upgradeForm.ShowDialog() == DialogResult.OK)
				{
					string selectedOption = upgradeForm.SelectedOption;
					if (isWhitesMove)
					{
						// yenecek taş varsa puanını düş
						if(game.GameBoard.BoardMatrix[row, col] == 10 || game.GameBoard.BoardMatrix[row, col] == 11)
						{
							game.GameBoard.BlacksPoints -= 3;
						}
						else if(game.GameBoard.BoardMatrix[row, col] == 12)
						{
							if(row == 0)
							{
								if(col == 0)
								{
									game.GameBoard.IsBlackLongRookMoved = true;
								}
								else if(col == 7)
								{
									game.GameBoard.IsBlackShortRookMoved = true;
								}
							}
							game.GameBoard.BlacksPoints -= 5;
						}
						else if(game.GameBoard.BoardMatrix[row, col] == 13)
						{
							game.GameBoard.BlacksPoints -= 9;
						}

						switch (selectedOption)
						{
							case "4-Queen":
								// -1 piyon +9 vezir
								game.GameBoard.WhitesPoints += 8;
								// matrisi güncelle
								game.GameBoard.BoardMatrix[row, col] = 6;
								// paneli güncelle
								Image whiteQueen = InitializePieces.LoadImage(assetsPath + "/QueenW.png");
								panel.BackgroundImage = whiteQueen;
								break;
							case "3-Rook":
								game.GameBoard.WhitesPoints += 4;
								game.GameBoard.BoardMatrix[row, col] = 5;
								Image whiteRook = InitializePieces.LoadImage(assetsPath + "/RookW.png");
								panel.BackgroundImage = whiteRook;
								break;
							case "1-Bishop":
								game.GameBoard.WhitesPoints += 2;
								game.GameBoard.BoardMatrix[row, col] = 4;
								Image whiteBishop = InitializePieces.LoadImage(assetsPath + "/BishopW.png");
								panel.BackgroundImage = whiteBishop;
								break;
							case "2-Knight":
								game.GameBoard.WhitesPoints += 2;
								game.GameBoard.BoardMatrix[row, col] = 3;
								Image whiteKnight = InitializePieces.LoadImage(assetsPath + "/KnightW.png");
								panel.BackgroundImage = whiteKnight;
								break;
						}
					}
					else
					{
						if (game.GameBoard.BoardMatrix[row, col] == 3 || game.GameBoard.BoardMatrix[row, col] == 4)
						{
							game.GameBoard.WhitesPoints -= 3;
						}
						else if (game.GameBoard.BoardMatrix[row, col] == 5)
						{
							if (row == 7)
							{
								if (col == 0)
								{
									game.GameBoard.IsWhiteLongRookMoved = true;
								}
								else if (col == 7)
								{
									game.GameBoard.IsWhiteShortRookMoved = true;
								}
							}
							game.GameBoard.WhitesPoints -= 5;
						}
						else if (game.GameBoard.BoardMatrix[row, col] == 6)
						{
							game.GameBoard.WhitesPoints -= 9;
						}

						switch (selectedOption)
						{
							case "4-Queen":
								game.GameBoard.BlacksPoints += 8;
								game.GameBoard.BoardMatrix[row, col] = 13;
								Image blackQueen = InitializePieces.LoadImage(assetsPath + "/QueenB.png");
								panel.BackgroundImage = blackQueen;
								break;
							case "3-Rook":
								game.GameBoard.BlacksPoints += 4;
								game.GameBoard.BoardMatrix[row, col] = 12;
								Image blackRook = InitializePieces.LoadImage(assetsPath + "/RookB.png");
								panel.BackgroundImage = blackRook;
								break;
							case "1-Bishop":
								game.GameBoard.BlacksPoints += 2;
								game.GameBoard.BoardMatrix[row, col] = 11;
								Image blackBishop = InitializePieces.LoadImage(assetsPath + "/BishopB.png");
								panel.BackgroundImage = blackBishop;
								break;
							case "2-Knight":
								game.GameBoard.BlacksPoints += 2;
								game.GameBoard.BoardMatrix[row, col] = 10;
								Image blackKnight = InitializePieces.LoadImage(assetsPath + "/KnightB.png");
								panel.BackgroundImage = blackKnight;
								break;
						}
					}
					game.GameBoard.BoardMatrix[oldRow, oldCol] = 0;
					oldPanel.BackgroundImage = null;
				}
				else
				{
					MessageBox.Show("Seçim yapmak zorunludur");
					HandleUpgrade(row, col, oldRow, oldCol, panel, oldPanel);
				}
			}
		}

		public void MatrixToPanel()
		{
			tableLayoutPanel2.Controls.Clear();

			// Matris değerlerini TableLayoutPanel'e ekle
			for (int row = 0; row < 8; row++)
			{
				for (int col = 0; col < 8; col++)
				{
					// Hücreye eklemek için bir Label oluştur
					Color color;
					if (game.GameBoard.BoardMatrix[row, col] == 0)
					{
						color = Color.Black;
					}
					else if(game.GameBoard.BoardMatrix[row, col] >= 8)
					{
						color = Color.PaleGreen;
					}
                    else
                    {
                        color = Color.PaleVioletRed;
                    }
                    Label cellLabel = new Label
					{
						Text = game.GameBoard.BoardMatrix[row, col].ToString(),
						Dock = DockStyle.Fill,
						TextAlign = ContentAlignment.MiddleCenter,
						BorderStyle = BorderStyle.FixedSingle,
						BackColor = color,
					};

					// Hücreyi TableLayoutPanel'e ekle
					tableLayoutPanel2.Controls.Add(cellLabel, col, row);
				}
			}

			textBox1.Text = game.GameBoard.IsChecked.ToString();
			textBox2.Text = game.GameBoard.IsMate.ToString();
			textBox3.Text = game.GameBoard.IsWhiteKingMoved.ToString();
			textBox4.Text = game.GameBoard.IsBlackKingMoved.ToString();
			textBox5.Text = game.GameBoard.IsWhiteShortRookMoved.ToString();
			textBox6.Text = game.GameBoard.IsWhiteLongRookMoved.ToString();
			textBox7.Text = game.GameBoard.IsBlackShortRookMoved.ToString();
			textBox8.Text = game.GameBoard.IsBlackLongRookMoved.ToString();
			textBox9.Text = "[" + game.GameBoard.WhiteKing.Row + ", " + game.GameBoard.WhiteKing.Col + "]";
			textBox10.Text = "[" + game.GameBoard.BlackKing.Row + ", " + game.GameBoard.BlackKing.Col + "]";
			textBox11.Text = "[" + game.GameBoard.WhitesCheckers[0].Row + ", " + game.GameBoard.WhitesCheckers[0].Col + "] / [" + game.GameBoard.WhitesCheckers[1].Row + ", " + game.GameBoard.WhitesCheckers[1].Col + "]";
			textBox12.Text = "[" + game.GameBoard.BlacksCheckers[0].Row + ", " + game.GameBoard.BlacksCheckers[0].Col + "] / [" + game.GameBoard.BlacksCheckers[1].Row + ", " + game.GameBoard.BlacksCheckers[1].Col + "]";
			textBox13.Text = game.GameBoard.WhitesPoints.ToString();
			textBox14.Text = game.GameBoard.BlacksPoints.ToString();
		}

		public static void InitializeBoardMatrix()
		{
			isWhitesMove = true;
			game.GameBoard = new Board(8, 8);
			// Setup the pieces for the white and black sides
			game.GameBoard.BoardMatrix[0, 0] = 12; // Black Rook
			game.GameBoard.BoardMatrix[0, 1] = 10; // Black Knight
			game.GameBoard.BoardMatrix[0, 2] = 11; // Black Bishop
			game.GameBoard.BoardMatrix[0, 3] = 13; // Black Queen
			game.GameBoard.BoardMatrix[0, 4] = 14; // Black King
			game.GameBoard.BoardMatrix[0, 5] = 11; // Black Bishop
			game.GameBoard.BoardMatrix[0, 6] = 10; // Black Knight
			game.GameBoard.BoardMatrix[0, 7] = 12; // Black Rook

			game.GameBoard.BoardMatrix[1, 0] = 8; // Black Pawn
			game.GameBoard.BoardMatrix[1, 1] = 8; // Black Pawn
			game.GameBoard.BoardMatrix[1, 2] = 8; // Black Pawn
			game.GameBoard.BoardMatrix[1, 3] = 8; // Black Pawn
			game.GameBoard.BoardMatrix[1, 4] = 8; // Black Pawn
			game.GameBoard.BoardMatrix[1, 5] = 8; // Black Pawn
			game.GameBoard.BoardMatrix[1, 6] = 8; // Black Pawn
			game.GameBoard.BoardMatrix[1, 7] = 8; // Black Pawn

			game.GameBoard.BlackKing = new Square()
			{
				Row = 0,
				Col = 4,
			};

			game.GameBoard.BoardMatrix[6, 0] = 1; // White Pawn
			game.GameBoard.BoardMatrix[6, 1] = 1; // White Pawn
			game.GameBoard.BoardMatrix[6, 2] = 1; // White Pawn
			game.GameBoard.BoardMatrix[6, 3] = 1; // White Pawn
			game.GameBoard.BoardMatrix[6, 4] = 1; // White Pawn
			game.GameBoard.BoardMatrix[6, 5] = 1; // White Pawn
			game.GameBoard.BoardMatrix[6, 6] = 1; // White Pawn
			game.GameBoard.BoardMatrix[6, 7] = 1; // White Pawn

			game.GameBoard.BoardMatrix[7, 0] = 5; // White Rook
			game.GameBoard.BoardMatrix[7, 1] = 3; // White Knight
			game.GameBoard.BoardMatrix[7, 2] = 4; // White Bishop
			game.GameBoard.BoardMatrix[7, 3] = 6; // White Queen
			game.GameBoard.BoardMatrix[7, 4] = 7; // White King
			game.GameBoard.BoardMatrix[7, 5] = 4; // White Bishop
			game.GameBoard.BoardMatrix[7, 6] = 3; // White Knight
			game.GameBoard.BoardMatrix[7, 7] = 5; // White Rook

			game.GameBoard.WhiteKing = new Square()
			{
				Row = 7,
				Col = 4,
			};

			Square square1 = new Square()
			{
				Row = -1,
				Col = -1,
			};
			Square square2 = new Square()
			{
				Row = -1,
				Col = -1,
			};
			Square square3 = new Square()
			{
				Row = -1,
				Col = -1,
			};
			Square square4 = new Square()
			{
				Row = -1,
				Col = -1,
			};
			game.GameBoard.WhitesCheckers = new List<Square> { square1, square2 };  // en fazla 2 şah çeken taş olabilir
			game.GameBoard.BlacksCheckers = new List<Square> { square3, square4 };

			game.GameBoard.WhitesPoints = 39;
			game.GameBoard.BlacksPoints = 39;

			game.GameBoard.IsChecked = false;
			game.GameBoard.IsMate = false;
			game.GameBoard.IsWhiteKingMoved = false;
			game.GameBoard.IsBlackKingMoved = false;
			game.GameBoard.IsWhiteShortRookMoved = false;
			game.GameBoard.IsWhiteLongRookMoved = false;
			game.GameBoard.IsBlackShortRookMoved = false;
			game.GameBoard.IsBlackLongRookMoved = false;
		}
	}
}
