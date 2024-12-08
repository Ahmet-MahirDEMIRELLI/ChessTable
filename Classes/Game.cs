namespace ChessTable.Classes
{
	public class Game
	{
        public Board GameBoard { get; set; }
        public int FiftyCount { get; set; }
        public int ThreePositionCount { get; set; }
        public bool DidWhiteOfferDraw { get; set; }
        public bool DidBlackOfferDraw { get; set; }
        public bool DidWhiteResign { get; set; }
        public bool DidBlackResign { get; set; }
        public string Result { get; set; }
    }
}
