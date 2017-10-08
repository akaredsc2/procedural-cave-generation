namespace DataStructures {
    public class SquareGrid {
        private readonly Square[,] squares;

        public SquareGrid(Square[,] squares) {
            this.squares = squares;
        }

        public Square[,] Squares {
            get { return squares; }
        }
    }
}