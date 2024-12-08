using System.Collections.Generic;

namespace ChessTable.Classes
{
	public class Board
	{
        public byte[,] BoardMatrix { get; set; }
        public bool IsChecked { get; set; }     // Şah çekildi mi
        public bool IsMate { get; set; }        // Mat edildi mi
        public bool IsWhiteKingMoved { get; set; }
        public bool IsBlackKingMoved { get; set; }
		public bool IsWhiteShortRookMoved { get; set; }
		public bool IsWhiteLongRookMoved { get; set; }
		public bool IsBlackShortRookMoved { get; set; }
		public bool IsBlackLongRookMoved { get; set; }
        public Square WhiteKing { get; set; }   // Beyazın şahı
		public Square BlackKing { get; set; }
		public List<Square> WhitesCheckers { get; set; }   // Beyazın şah çekmek için kullandığı taşlar
		public List<Square> BlacksCheckers { get; set; }
		public int WhitesPoints { get; set; } // Tahtada olan taşların toplam puanı (Şah=0)
        public int BlacksPoints { get; set; }

        public Board(int rows, int columns)
		{
			BoardMatrix = new byte[rows, columns];
		}
	}
}
