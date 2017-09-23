using DataStructures;
using UnityEngine;

namespace Factories
{
    public class SquareGridFactory
    {
        public SquareGrid CreateSqaureGrid(int[,] map, float squareSize)
        {
            ControlNode[,] controlNodes = CreateControlNodes(map, squareSize);
            Square[,] squares = CreateSquares(controlNodes);
            return new SquareGrid(squares);
        }

        private ControlNode[,] CreateControlNodes(int[,] map, float squareSize)
        {
            int horizontalNodeCount = map.GetLength(0);
            int verticalNodeCount = map.GetLength(1);
            float mapWidth = horizontalNodeCount * squareSize;
            float mapHeight = verticalNodeCount * squareSize;

            ControlNode[,] result = new ControlNode[horizontalNodeCount, verticalNodeCount];

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    Vector3 position = new Vector3(
                        -mapWidth / 2 + i * squareSize + squareSize / 2,
                        0,
                        -mapHeight / 2 + j * squareSize + squareSize / 2);

                    result[i, j] = new ControlNode(position, map[i, j] == 1, squareSize);
                }
            }

            return result;
        }

        private Square[,] CreateSquares(ControlNode[,] controlNodes)
        {
            Square[,] result = new Square[controlNodes.GetLength(0) - 1, controlNodes.GetLength(1) - 1];

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = new Square(
                        controlNodes[i, j + 1],
                        controlNodes[i + 1, j + 1],
                        controlNodes[i + 1, j],
                        controlNodes[i, j]);
                }
            }

            return result;
        }
    }
}