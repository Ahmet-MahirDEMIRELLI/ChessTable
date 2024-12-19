using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ChessTable.Helper
{
	public class InitializePieces
	{
		public static void PlacePieces(TableLayoutPanel tableLayoutPanel)
		{
			// assets klasöründen görsel dosyalarını almak için yol
			string assetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\assets");
			assetsPath = Path.GetFullPath(assetsPath); // Tam yolu almak için

			// Görselleri yüklerken hata kontrolü yapalım
			Image whitePawn = LoadImage(assetsPath + "/PawnW.png");
			Image blackPawn = LoadImage(assetsPath + "/PawnB.png");
			Image whiteRook = LoadImage(assetsPath + "/RookW.png");
			Image blackRook = LoadImage(assetsPath + "/RookB.png");
			Image whiteKnight = LoadImage(assetsPath + "/KnightW.png");
			Image blackKnight = LoadImage(assetsPath + "/KnightB.png");
			Image whiteBishop = LoadImage(assetsPath + "/BishopW.png");
			Image blackBishop = LoadImage(assetsPath + "/BishopB.png");
			Image whiteKing = LoadImage(assetsPath + "/KingW.png");
			Image blackKing = LoadImage(assetsPath + "/KingB.png");
			Image whiteQueen = LoadImage(assetsPath + "/QueenW.png");
			Image blackQueen = LoadImage(assetsPath + "/QueenB.png");

			// Taşları yerleştir
			for (int col = 0; col < 8; col++)
			{
				tableLayoutPanel.GetControlFromPosition(col, 6).BackgroundImage = whitePawn;  // Beyaz piyonlar
				tableLayoutPanel.GetControlFromPosition(col, 7).BackgroundImage = col == 0 || col == 7 ? whiteRook :
																					col == 1 || col == 6 ? whiteKnight :
																					col == 2 || col == 5 ? whiteBishop :
																					col == 3 ? whiteQueen : whiteKing;
			}

			for (int col = 0; col < 8; col++)
			{
				tableLayoutPanel.GetControlFromPosition(col, 1).BackgroundImage = blackPawn;  // Siyah piyonlar
				tableLayoutPanel.GetControlFromPosition(col, 0).BackgroundImage = col == 0 || col == 7 ? blackRook :
																					col == 1 || col == 6 ? blackKnight :
																					col == 2 || col == 5 ? blackBishop :
																					col == 3 ? blackQueen : blackKing;
			}
		}

		// Görseli yüklerken hata kontrolü yapan yardımcı fonksiyon
		public static Image LoadImage(string path)
		{
			try
			{
				return Image.FromFile(path);
			}
			catch (FileNotFoundException)
			{
				MessageBox.Show($"Görsel bulunamadı: {path}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return null;
			}
		}
	}
}
