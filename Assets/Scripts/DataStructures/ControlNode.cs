using UnityEngine;

namespace DataStructures {
    public class ControlNode : Node {
        private readonly bool isActive;
        private readonly Node above;
        private readonly Node right;

        protected ControlNode(Vector3 position, bool isActive, Node above, Node right, int vertexIndex)
            : base(position, vertexIndex) {
            this.isActive = isActive;
            this.above = above;
            this.right = right;
        }

        public ControlNode(Vector3 position, bool isActive, float squareSize) : base(position) {
            this.isActive = isActive;
            above = new Node(position + Vector3.forward * squareSize / 2f);
            right = new Node(position + Vector3.right * squareSize / 2f);
        }

        public bool IsActive {
            get { return isActive; }
        }

        public Node Above {
            get { return above; }
        }

        public Node Right {
            get { return right; }
        }

        public override Node CopyWithNewVertexIndex(int vertexIndex) {
            return new ControlNode(Position, isActive, above, right, vertexIndex);
        }
    }
}