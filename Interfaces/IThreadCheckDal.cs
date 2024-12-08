namespace ChessTable.Interfaces
{
	public interface IThreadCheckDal
	{
		bool CheckPawn(byte[,] m, int row, int col, bool checkWhitePieces);
		bool CheckKnight(byte[,] m, int row, int col, bool checkWhitePieces);
		bool CheckBishop(byte[,] m, int row, int col, bool checkWhitePieces);
		bool CheckRook(byte[,] m, int row, int col, bool checkWhitePieces);
		bool CheckQueen(byte[,] m, int row, int col, bool checkWhitePieces);
		bool CheckKing(byte[,] m, int row, int col, bool checkWhitePieces);
		bool CheckSquare(int row, int col);
		bool IsUnderCheck(byte[,] m, bool checkWhiteKing);
		bool IsMovable(byte[,] m, int pieceRow, int pieceColumn, int kingRow, int kingCol, bool isWhite);
		int TraceToKing(byte[,] m, int pieceRow, int pieceColumn, int kingRow, int kingCol, bool isWhite);
	}
}
