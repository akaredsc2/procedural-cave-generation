using System.Collections.Generic;
using DataStructures;
using UnityEngine;

namespace Factories
{
    public class MeshFactory
    {
        public Mesh CreateMesh(SquareGrid squareGrid, MeshFilter wallsMeshFilter, bool is2d, GameObject gameObject,
            float squareSize)
        {
            // todo create class for holding this collections
            List<Vector3> vertices = new List<Vector3>();
            List<int> indecies = new List<int>();
            Dictionary<int, List<Triangle>> triangleDictionary = new Dictionary<int, List<Triangle>>();
            List<List<int>> outlines = new List<List<int>>();
            HashSet<int> checkedVertices = new HashSet<int>();

            for (int i = 0; i < squareGrid.Squares.GetLength(0); i++)
            {
                for (int j = 0; j < squareGrid.Squares.GetLength(1); j++)
                {
                    TriangulateSquare(checkedVertices, triangleDictionary, vertices, indecies,
                        squareGrid.Squares[i, j]);
                }
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = indecies.ToArray();
            mesh.RecalculateNormals();

            int tileAmount = 10;
            Vector2[] uv = new Vector2[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                float percentX = Mathf.InverseLerp(
                    -squareGrid.Squares.GetLength(0) / 2 * squareSize,
                    squareGrid.Squares.GetLength(0) / 2 * squareSize,
                    vertices[i].x) * tileAmount;
                float percentY = Mathf.InverseLerp(
                    -squareGrid.Squares.GetLength(0) / 2 * squareSize,
                    squareGrid.Squares.GetLength(0) / 2 * squareSize,
                    vertices[i].z) * tileAmount;
                uv[i] = new Vector2(percentX, percentY);
            }
            mesh.uv = uv;

            if (is2d)
            {
                Generate2DColliders(outlines, checkedVertices, triangleDictionary, vertices, gameObject);
            }
            else
            {
                CreateWallMesh(outlines, checkedVertices, triangleDictionary, vertices, wallsMeshFilter);
            }
            return mesh;
        }

        // todo move to separate class
        private void CreateWallMesh(List<List<int>> outlines, HashSet<int> checkedVertices,
            Dictionary<int, List<Triangle>> triangleDictionary, List<Vector3> vertices,
            MeshFilter wallsMeshFilter)
        {
            CalculateMeshOutlines(outlines, checkedVertices, triangleDictionary, vertices);

            List<Vector3> wallVertices = new List<Vector3>();
            List<int> wallTriangles = new List<int>();
            Mesh wallMesh = new Mesh();
            float wallHeight = 5;

            foreach (List<int> outline in outlines)
            {
                for (int i = 0; i < outline.Count - 1; i++)
                {
                    int startIndex = wallVertices.Count;
                    wallVertices.Add(vertices[outline[i]]); // top left vertex
                    wallVertices.Add(vertices[outline[i + 1]]); // top right vertex
                    wallVertices.Add(vertices[outline[i]] - Vector3.up * wallHeight); // bottom left vertex
                    wallVertices.Add(vertices[outline[i + 1]] - Vector3.up * wallHeight); // bottom right vertex

                    wallTriangles.Add(startIndex + 0); // from top left
                    wallTriangles.Add(startIndex + 2); // to bottom left
                    wallTriangles.Add(startIndex + 3); // to bottom right

                    wallTriangles.Add(startIndex + 3); // from bottom right
                    wallTriangles.Add(startIndex + 1); // to top right
                    wallTriangles.Add(startIndex + 0); // to top left
                }
            }

            wallMesh.vertices = wallVertices.ToArray();
            wallMesh.triangles = wallTriangles.ToArray();
            wallsMeshFilter.mesh = wallMesh;

            MeshCollider wallCollider = wallsMeshFilter.gameObject.AddComponent<MeshCollider>();
            wallCollider.sharedMesh = wallMesh;
        }

        // todo fix outline generation
        void Generate2DColliders(List<List<int>> outlines, HashSet<int> checkedVertices,
            Dictionary<int, List<Triangle>> triangleDictionary, List<Vector3> vertices, GameObject gameObject)
        {
            EdgeCollider2D[] currentColliders = gameObject.GetComponents<EdgeCollider2D>();
            for (int i = 0; i < currentColliders.Length; i++)
            {
                GameObject.Destroy(currentColliders[i]);
            }

            CalculateMeshOutlines(outlines, checkedVertices, triangleDictionary, vertices);

            foreach (List<int> outline in outlines)
            {
                EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
                Vector2[] edgePoints = new Vector2[outline.Count];

                for (int i = 0; i < outline.Count; i++)
                {
                    edgePoints[i] = new Vector2(vertices[outline[i]].x, vertices[outline[i]].z);
                }

                edgeCollider.points = edgePoints;
            }
        }

        private void TriangulateSquare(HashSet<int> checkedVertices, Dictionary<int, List<Triangle>> triangleDictionary,
            List<Vector3> vertices,
            List<int> indecies, Square square)
        {
            switch (square.Configuration)
            {
                case 0:
                    break;

                // 1 active control node
                case 1:
                    MeshFromPoints(triangleDictionary, vertices, indecies, square.CenterLeft, square.CenterBottom,
                        square.BottomLeft);
                    break;
                case 2:
                    MeshFromPoints(triangleDictionary, vertices, indecies, square.BottomRight, square.CenterBottom,
                        square.CenterRight);
                    break;
                case 4:
                    MeshFromPoints(triangleDictionary, vertices, indecies, square.TopRight, square.CenterRight,
                        square.CenterTop);
                    break;
                case 8:
                    MeshFromPoints(triangleDictionary, vertices, indecies, square.TopLeft, square.CenterTop,
                        square.CenterLeft);
                    break;

                // 2 active control nodes
                case 3:
                    MeshFromPoints(triangleDictionary, vertices, indecies, square.CenterRight, square.BottomRight,
                        square.BottomLeft,
                        square.CenterLeft);
                    break;
                case 6:
                    MeshFromPoints(triangleDictionary, vertices, indecies, square.CenterTop, square.TopRight,
                        square.BottomRight,
                        square.CenterBottom);
                    break;
                case 9:
                    MeshFromPoints(triangleDictionary, vertices, indecies, square.TopLeft, square.CenterTop,
                        square.CenterBottom,
                        square.BottomLeft);
                    break;
                case 12:
                    MeshFromPoints(triangleDictionary, vertices, indecies, square.TopLeft, square.TopRight,
                        square.CenterRight,
                        square.CenterLeft);
                    break;
                case 5:
                    MeshFromPoints(triangleDictionary, vertices, indecies, square.CenterTop, square.TopRight,
                        square.CenterRight,
                        square.CenterBottom,
                        square.BottomLeft, square.CenterLeft);
                    break;
                case 10:
                    MeshFromPoints(triangleDictionary, vertices, indecies, square.TopLeft, square.CenterTop,
                        square.CenterRight,
                        square.BottomRight,
                        square.CenterBottom, square.CenterLeft);
                    break;

                // 3 active control nodes
                case 7:
                    MeshFromPoints(triangleDictionary, vertices, indecies, square.CenterTop, square.TopRight,
                        square.BottomRight,
                        square.BottomLeft,
                        square.CenterLeft);
                    break;
                case 11:
                    MeshFromPoints(triangleDictionary, vertices, indecies, square.TopLeft, square.CenterTop,
                        square.CenterRight,
                        square.BottomRight,
                        square.BottomLeft);
                    break;
                case 13:
                    MeshFromPoints(triangleDictionary, vertices, indecies, square.TopLeft, square.TopRight,
                        square.CenterRight,
                        square.CenterBottom,
                        square.BottomLeft);
                    break;
                case 14:
                    MeshFromPoints(triangleDictionary, vertices, indecies, square.TopLeft, square.TopRight,
                        square.BottomRight,
                        square.CenterBottom,
                        square.CenterLeft);
                    break;

                // 4 active control nodes
                case 15:
                    var meshPoints = MeshFromPoints(triangleDictionary, vertices, indecies, square.TopLeft,
                        square.TopRight,
                        square.BottomRight,
                        square.BottomLeft);
                    foreach (Node meshPoint in meshPoints)
                        checkedVertices.Add(meshPoint.VertexIndex);
                    break;
            }
        }

        private Node[] MeshFromPoints(Dictionary<int, List<Triangle>> triangleDictionary, List<Vector3> vertices,
            List<int> indecies, params Node[] points)
        {
            Node[] indexedPoints = AssignVertices(vertices, points);

            if (points.Length >= 3)
                CreateTriangle(triangleDictionary, indecies, indexedPoints[0], indexedPoints[1], indexedPoints[2]);
            if (points.Length >= 4)
                CreateTriangle(triangleDictionary, indecies, indexedPoints[0], indexedPoints[2], indexedPoints[3]);
            if (points.Length >= 5)
                CreateTriangle(triangleDictionary, indecies, indexedPoints[0], indexedPoints[3], indexedPoints[4]);
            if (points.Length >= 6)
                CreateTriangle(triangleDictionary, indecies, indexedPoints[0], indexedPoints[4], indexedPoints[5]);

            return indexedPoints;
        }

        private Node[] AssignVertices(List<Vector3> vertices, Node[] points)
        {
            Node[] nodesWithVertexIndecies = new Node[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].VertexIndex == -1)
                {
                    nodesWithVertexIndecies[i] = points[i].CopyWithVertexIndex(vertices.Count);
                }
                vertices.Add(nodesWithVertexIndecies[i].Position);
            }
            return nodesWithVertexIndecies;
        }

        private void CreateTriangle(Dictionary<int, List<Triangle>> triangleDictionary, List<int> indecies, Node a,
            Node b, Node c)
        {
            indecies.Add(a.VertexIndex);
            indecies.Add(b.VertexIndex);
            indecies.Add(c.VertexIndex);

            Triangle triangle = new Triangle(a.VertexIndex, b.VertexIndex, c.VertexIndex);
            AddTriangleToDictrionary(triangleDictionary, triangle.vertexIndexA, triangle);
            AddTriangleToDictrionary(triangleDictionary, triangle.vertexIndexB, triangle);
            AddTriangleToDictrionary(triangleDictionary, triangle.vertexIndexC, triangle);
        }

        void AddTriangleToDictrionary(Dictionary<int, List<Triangle>> triangleDictionary, int vertexIndexKey,
            Triangle triangle)
        {
            if (!triangleDictionary.ContainsKey(vertexIndexKey))
            {
                triangleDictionary.Add(vertexIndexKey, new List<Triangle>());
            }
            triangleDictionary[vertexIndexKey].Add(triangle);
        }

        // todo refactor
        void CalculateMeshOutlines(List<List<int>> outlines, HashSet<int> checkedVertices,
            Dictionary<int, List<Triangle>> triangleDictionary,
            List<Vector3> vertices)
        {
            for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
            {
                if (!checkedVertices.Contains(vertexIndex))
                {
                    int newOutlineVertex = GetConnectedOutlineVertex(checkedVertices, triangleDictionary, vertexIndex);
                    if (newOutlineVertex != -1)
                    {
                        checkedVertices.Add(vertexIndex);

                        List<int> newOutline = new List<int>();
                        newOutline.Add(vertexIndex);
                        outlines.Add(newOutline);
                        FollowOutline(outlines, checkedVertices, triangleDictionary, newOutlineVertex,
                            outlines.Count - 1);
                        outlines[outlines.Count - 1].Add(vertexIndex);
                    }
                }
            }
        }

        // todo replace recursion with loop
        void FollowOutline(List<List<int>> outlines, HashSet<int> checkedVertices,
            Dictionary<int, List<Triangle>> triangleDictionary, int vertexIndex, int outlineIndex)
        {
            outlines[outlineIndex].Add(vertexIndex);
            checkedVertices.Add(vertexIndex);
            int nextVertexIndex = GetConnectedOutlineVertex(checkedVertices, triangleDictionary, vertexIndex);

            if (nextVertexIndex != -1)
            {
                FollowOutline(outlines, checkedVertices, triangleDictionary, nextVertexIndex, outlineIndex);
            }
        }

        private int GetConnectedOutlineVertex(HashSet<int> checkedVertices,
            Dictionary<int, List<Triangle>> triangleDictionary,
            int vertexIndex)
        {
            List<Triangle> trianglesContainingVertex = triangleDictionary[vertexIndex];

            foreach (Triangle triangle in trianglesContainingVertex)
            {
                for (int j = 0; j < 3; j++)
                {
                    int vertexB = triangle[j];
                    if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB))
                    {
                        if (IsOutlineEdge(triangleDictionary, vertexIndex, vertexB))
                        {
                            return vertexB;
                        }
                    }
                }
            }

            return -1;
        }

        private bool IsOutlineEdge(IDictionary<int, List<Triangle>> triangleDictionary, int vertexA, int vertexB)
        {
            List<Triangle> trianglesContainingVertexA = triangleDictionary[vertexA];
            int sharedTrianglesCount = 0;

            for (int i = 0; i < trianglesContainingVertexA.Count; i++)
            {
                if (trianglesContainingVertexA[i].Contains(vertexB))
                {
                    sharedTrianglesCount += 1;
                }
            }

            return sharedTrianglesCount == 1;
        }

        private struct Triangle
        {
            public readonly int vertexIndexA;
            public readonly int vertexIndexB;
            public readonly int vertexIndexC;
            private readonly int[] vertices;

            public Triangle(int vertexIndexA, int vertexIndexB, int vertexIndexC)
            {
                this.vertexIndexA = vertexIndexA;
                this.vertexIndexB = vertexIndexB;
                this.vertexIndexC = vertexIndexC;

                vertices = new int[3];
                vertices[0] = vertexIndexA;
                vertices[1] = vertexIndexB;
                vertices[2] = vertexIndexC;
            }

            public int this[int i]
            {
                get { return vertices[i]; }
            }

            public bool Contains(int vertexIndex)
            {
                return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
            }
        }
    }
}