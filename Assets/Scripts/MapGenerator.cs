using System;
using System.Diagnostics;
using UnityEngine;
using Random = System.Random;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)] public int randomFillPercent;

    public int smootingIterationCount;

    private int[,] map;

    private void Start()
    {
        map = GenerateMap(width, height, smootingIterationCount);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            map = GenerateMap(width, height, smootingIterationCount);
        }
    }

    private int[,] GenerateMap(int width, int height, int smootingIterationCount)
    {
        int[,] emptyMap = new int[width, height];
        int[,] filledMap = RandomFillMap(emptyMap);
        int[,] smoothedMap = SmoothMap(filledMap, smootingIterationCount);
        return smoothedMap;
    }

    private int[,] RandomFillMap(int[,] targetMap)
    {
        int[,] result = createCopy(targetMap);

        if (useRandomSeed)
        {
            seed = Stopwatch.GetTimestamp().ToString();
        }

        Random random = new Random(seed.GetHashCode());

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                result[i, j] = FillCell(i, j, random);
            }
        }

        return result;
    }

    private int FillCell(int column, int row, Random random)
    {
        var isPartOfVerticalBorder = IsOnBorder(column, width);
        var isPartOfHorizontalBorder = IsOnBorder(row, height);

        var isPartOfBorder = isPartOfVerticalBorder || isPartOfHorizontalBorder;

        return isPartOfBorder ? 1 : random.Next(0, 100) < randomFillPercent ? 1 : 0;
    }

    private int[,] SmoothMap(int[,] targetMap, int iterationCount)
    {
        int[,] previousIterationMap = targetMap;
        for (int i = 0; i < iterationCount; i++)
        {
            int[,] smoothedMap = SmoothMap(previousIterationMap);
            previousIterationMap = smoothedMap;
        }
        return previousIterationMap;
    }

    private int[,] SmoothMap(int[,] targetMap)
    {
        int[,] result = createCopy(targetMap);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int surroundingWallsCount = CountSurroundingWalls(i, j, result);

                if (surroundingWallsCount > 4)
                {
                    result[i, j] = 1;
                }
                else if (surroundingWallsCount < 4)
                {
                    result[i, j] = 0;
                }
            }
        }

        return result;
    }

    private int CountSurroundingWalls(int column, int row, int[,] targetMap)
    {
        int wallCount = 0;

        for (int neighbourColumn = column - 1; neighbourColumn <= column + 1; neighbourColumn++)
        {
            for (int neighbourRow = row - 1; neighbourRow <= row + 1; neighbourRow++)
            {
                if (IsInsideBorder(neighbourColumn, width) && IsInsideBorder(neighbourRow, height))
                {
                    if (neighbourColumn != column || neighbourRow != row)
                    {
                        wallCount += targetMap[neighbourColumn, neighbourRow];
                    }
                }
                else
                {
                    wallCount += 1;
                }
            }
        }

        return wallCount;
    }

    private bool IsOnBorder(int value, int borderLength)
    {
        return value == 0 || value == borderLength - 1;
    }

    private bool IsInsideBorder(int value, int borderLength)
    {
        return value >= 0 && value < borderLength;
    }

    private int[,] createCopy(int[,] target)
    {
        int[,] copy = new int[target.GetLength(0), target.GetLength(1)];
        Array.Copy(target, 0, copy, 0, target.Length);
        return copy;
    }

    private void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Gizmos.color = map[i, j] == 1 ? Color.black : Color.white;
                    Vector3 position = new Vector3(-width / 2 + i + 0.5f, 0, -height / 2 + j + 0.5f);
                    Gizmos.DrawCube(position, Vector3.one);
                }
            }
        }
    }
}