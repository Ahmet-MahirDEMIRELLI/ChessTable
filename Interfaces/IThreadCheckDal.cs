using ChessTable.Classes;

namespace ChessTable.Interfaces
{
	public interface IThreadCheckDal
	{
		Checker CheckPawn(byte[,] m, int row, int col, bool checkWhitePieces);
		Checker CheckKnight(byte[,] m, int row, int col, bool checkWhitePieces);
		Checker CheckBishop(byte[,] m, int row, int col, bool checkWhitePieces);
		Checker CheckRook(byte[,] m, int row, int col, bool checkWhitePieces);
		Checker CheckQueen(byte[,] m, int row, int col, bool checkWhitePieces);
		bool CheckKing(byte[,] m, int row, int col, bool checkWhitePieces);
		bool CheckSquare(int row, int col);
		bool IsMovable(byte[,] m, int pieceRow, int pieceColumn, int kingRow, int kingCol, bool isWhite);
		int TraceToKing(byte[,] m, int pieceRow, int pieceColumn, int kingRow, int kingCol, bool isWhite);
	}
}
