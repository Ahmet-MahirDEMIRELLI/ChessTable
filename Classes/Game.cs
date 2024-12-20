using System.Collections.Generic;

namespace ChessTable.Classes
{
	public class Game
	{
        public Board GameBoard { get; set; }
        public List<string> WhiteMoves { get; set; }
        public List<string> BlackMoves { get; set; }
        public int MoveCounter { get; set; }
        public int FiftyCount { get; set; }
        public int ThreePositionCount { get; set; }
        public bool DidWhiteOfferDraw { get; set; }
        public bool DidBlackOfferDraw { get; set; }
        public bool DidWhiteResign { get; set; }
        public bool DidBlackResign { get; set; }
        public string Result { get; set; }
    }
}
