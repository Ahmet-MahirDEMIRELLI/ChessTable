using ChessTable.Classes;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ChessTable.Helpers
{
	public class Highlighter
	{
		public static void HighlightPossibleMoves(TableLayoutPanel tableLayoutPanel, int row, int column, List<Move> moves)
		{
			// Geçerli taşı işaretle
			Panel targetPanel = (Panel)tableLayoutPanel.GetControlFromPosition(column, row);
			targetPanel.BackColor = Color.Blue;

			// Geçerli taşın gidebileceği kareleri işaretle
			foreach (var move in moves)
			{
				targetPanel = (Panel)tableLayoutPanel.GetControlFromPosition(move.Column, move.Row);
				targetPanel.BackColor = Color.Green;
			}
		}

		public static void ResetHighlightedSquares(TableLayoutPanel panel, List<Move> highlightedMoves, int row, int col)
		{
			// Geçerli taşın işaretini kaldır
			Panel square = (Panel)panel.GetControlFromPosition(col, row);
			square.BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray;

			foreach (var move in highlightedMoves)
			{
				// Panelin konumunu bul
				square = (Panel)panel.GetControlFromPosition(move.Column, move.Row);
				// Satır ve sütuna göre eski rengi belirle
				square.BackColor = (move.Row + move.Column) % 2 == 0 ? Color.White : Color.Gray;
			}
		}
	}
}
