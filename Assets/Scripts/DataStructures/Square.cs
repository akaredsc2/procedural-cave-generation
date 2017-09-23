namespace DataStructures
{
    public class Square
    {
        private readonly ControlNode topLeft;
        private readonly ControlNode topRight;
        private readonly ControlNode bottomRight;
        private readonly ControlNode bottomLeft;

        private readonly Node centerTop;
        private readonly Node centerRight;
        private readonly Node centerBottom;
        private readonly Node centerLeft;

        private int configuration;

        // todo move to factory
        public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft)
        {
            this.topLeft = topLeft;
            this.topRight = topRight;
            this.bottomRight = bottomRight;
            this.bottomLeft = bottomLeft;

            centerTop = topLeft.Right;
            centerRight = bottomRight.Above;
            centerBottom = bottomLeft.Right;
            centerLeft = bottomLeft.Above;

            if (topLeft.IsActive)
                configuration += 8;
            if (topRight.IsActive)
                configuration += 4;
            if (bottomRight.IsActive)
                configuration += 2;
            if (bottomLeft.IsActive)
                configuration += 1;
        }

        public ControlNode TopLeft
        {
            get { return topLeft; }
        }

        public ControlNode TopRight
        {
            get { return topRight; }
        }

        public ControlNode BottomRight
        {
            get { return bottomRight; }
        }

        public ControlNode BottomLeft
        {
            get { return bottomLeft; }
        }

        public Node CenterTop
        {
            get { return centerTop; }
        }

        public Node CenterRight
        {
            get { return centerRight; }
        }

        public Node CenterBottom
        {
            get { return centerBottom; }
        }

        public Node CenterLeft
        {
            get { return centerLeft; }
        }

        public int Configuration
        {
            get { return configuration; }
        }
    }
}