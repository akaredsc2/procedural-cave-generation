using UnityEngine;

namespace DataStructures
{
    public class ControlNode : Node
    {
        private readonly bool isActive;
        private readonly Node above;
        private readonly Node right;

        public ControlNode(Vector3 position, bool isActive, float squareSize) : base(position)
        {
            this.isActive = isActive;
            above = new Node(position + Vector3.forward * squareSize / 2f);
            right = new Node(position + Vector3.right * squareSize / 2f);
        }

        public bool IsActive
        {
            get { return isActive; }
        }

        public Node Above
        {
            get { return above; }
        }

        public Node Right
        {
            get { return right; }
        }
    }
}