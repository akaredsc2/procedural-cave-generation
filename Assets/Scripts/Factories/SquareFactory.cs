using DataStructures;

namespace Factories {
    public class SquareFactory {
        public Square CreateSquare(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight,
                                   ControlNode bottomLeft) {
            Node centerTop = topLeft.Right;
            Node centerRight = bottomRight.Above;
            Node centerBottom = bottomLeft.Right;
            Node centerLeft = bottomLeft.Above;

            int configuration = 0;

            if (topLeft.IsActive)
                configuration += 8;
            if (topRight.IsActive)
                configuration += 4;
            if (bottomRight.IsActive)
                configuration += 2;
            if (bottomLeft.IsActive)
                configuration += 1;

            return new Square(topLeft, topRight, bottomRight, bottomLeft, 
                              centerTop, centerRight, centerBottom, centerLeft,
                              configuration);
        }
    }
}