using System;
using System.Diagnostics;
using DataStructures;

namespace Factories {
    public class MapFactory {
        public int[,] CreateMap(MapProperties mapProperties) {
            int[,] filledMap = RandomFillMap(mapProperties);
            return SmoothMap(filledMap, mapProperties);
        }

        private int[,] RandomFillMap(MapProperties mapProperties) {
            int width = mapProperties.Width;
            int height = mapProperties.Height;

            string seed = mapProperties.UseRandomSeed ? Stopwatch.GetTimestamp().ToString() : mapProperties.Seed;
            Random random = new Random(seed.GetHashCode());

            int[,] result = new int[width, height];
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    result[i, j] = FillCell(i, j, random, mapProperties);
                }
            }

            return result;
        }

        private int FillCell(int column, int row, Random random, MapProperties mapProperties) {
            var isPartOfVerticalBorder = IsOnBorder(column, mapProperties.Width);
            var isPartOfHorizontalBorder = IsOnBorder(row, mapProperties.Height);

            var isPartOfBorder = isPartOfVerticalBorder || isPartOfHorizontalBorder;

            return isPartOfBorder ? 1 : random.Next(0, 100) < mapProperties.RandomFillPercent ? 1 : 0;
        }

        private int[,] SmoothMap(int[,] targetMap, MapProperties mapProperties) {
            int[,] previousIterationMap = targetMap;
            for (int i = 0; i < mapProperties.SmoothingIterationCount; i++) {
                int[,] smoothedMap = SmoothMapOnce(previousIterationMap, mapProperties);
                previousIterationMap = smoothedMap;
            }
            return previousIterationMap;
        }

        private int[,] SmoothMapOnce(int[,] targetMap, MapProperties mapProperties) {
            int[,] result = createCopy(targetMap);

            for (int i = 0; i < mapProperties.Width; i++) {
                for (int j = 0; j < mapProperties.Height; j++) {
                    int surroundingWallsCount = CountSurroundingWalls(i, j, result, mapProperties);

                    if (surroundingWallsCount > 4) {
                        result[i, j] = 1;
                    } else if (surroundingWallsCount < 4) {
                        result[i, j] = 0;
                    }
                }
            }

            return result;
        }

        private int CountSurroundingWalls(int column, int row, int[,] targetMap, MapProperties mapProperties) {
            int wallCount = 0;

            for (int neighbourColumn = column - 1; neighbourColumn <= column + 1; neighbourColumn++) {
                for (int neighbourRow = row - 1; neighbourRow <= row + 1; neighbourRow++) {
                    if (IsInsideBorder(neighbourColumn, mapProperties.Width)
                        && IsInsideBorder(neighbourRow, mapProperties.Height)) {
                        if (neighbourColumn != column || neighbourRow != row) {
                            wallCount += targetMap[neighbourColumn, neighbourRow];
                        }
                    } else {
                        wallCount += 1;
                    }
                }
            }

            return wallCount;
        }

        private bool IsOnBorder(int value, int borderLength) {
            return value == 0 || value == borderLength - 1;
        }

        private bool IsInsideBorder(int value, int borderLength) {
            return value >= 0 && value < borderLength;
        }

        private int[,] createCopy(int[,] target) {
            int[,] copy = new int[target.GetLength(0), target.GetLength(1)];
            Array.Copy(target, 0, copy, 0, target.Length);
            return copy;
        }
    }
}