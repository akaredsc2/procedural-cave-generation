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

        private readonly int configuration;

        public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft,
            Node centerTop, Node centerRight, Node centerBottom, Node centerLeft, int configuration)
        {
            this.topLeft = topLeft;
            this.topRight = topRight;
            this.bottomRight = bottomRight;
            this.bottomLeft = bottomLeft;
            this.centerTop = centerTop;
            this.centerRight = centerRight;
            this.centerBottom = centerBottom;
            this.centerLeft = centerLeft;
            this.configuration = configuration;
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