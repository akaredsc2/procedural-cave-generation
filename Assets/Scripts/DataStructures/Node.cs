using UnityEngine;

namespace DataStructures
{
    public class Node
    {
        private readonly Vector3 position;
        private readonly int vertexIndex = -1;

        public Node(Vector3 position)
        {
            this.position = position;
        }

        public Vector3 Position
        {
            get { return position; }
        }

        public int VertexIndex
        {
            get { return vertexIndex; }
        }
    }
}