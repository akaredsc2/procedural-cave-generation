using System;
using System.Collections.Generic;
using DataStructures;
using Factories;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public ProceduralCaveGenerationAbstractFactory proceduralCaveGenerationAbstractFactory;

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)] public int randomFillPercent;

    public int smoothingIterationCount;
    public float squareSize;

    public int borderSize;

    public int wallThreshold = 50; 
    public int roomThreshold = 50; 

    private MapFactory mapFactory;
    private MapPropertiesFactory mapPropertiesFactory;
    private int[,] map;

    private void Start()
    {
        mapFactory = proceduralCaveGenerationAbstractFactory.MapFactory;
        mapPropertiesFactory = proceduralCaveGenerationAbstractFactory.MapPropertiesFactory;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MapProperties mapProperties = mapPropertiesFactory.createMapProperties(width, height, seed, useRandomSeed,
                randomFillPercent, smoothingIterationCount, squareSize);

            map = mapFactory.CreateMap(mapProperties);

            ProcessMap(map, wallThreshold, roomThreshold);
            
            // todo move to map factory
            int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

            for (int i = 0; i < borderedMap.GetLength(0); i++)
            {
                for (int j = 0; j < borderedMap.GetLength(1); j++)
                {
                    if (i >= borderSize && i < width + borderSize && j >= borderSize && j < height + borderSize)
                    {
                        borderedMap[i, j] = map[i - borderSize, j - borderSize];
                    }
                    else
                    {
                        borderedMap[i, j] = 1;
                    }
                }
            }

            MeshGenerator meshGenerator = GetComponent<MeshGenerator>();
            meshGenerator.GenerateMesh(borderedMap, 1);
        }
    }

    // todo move
    private void ProcessMap(int[,] map, int wallThreshold, int roomThreshold)
    {
        // todo extract method
        List<List<Coord>> wallRegions = GetRegions(1, map);
        
        foreach (List<Coord> wallRegion in wallRegions)
        {
            if (wallRegion.Count < wallThreshold)
            {
                foreach (Coord tile in wallRegion)
                {
                    map[tile.TileX, tile.TileY] = 0;
                }
            }
        }
        
        List<List<Coord>> roomRegions = GetRegions(0, map);
        
        foreach (List<Coord> roomRegion in roomRegions)
        {
            if (roomRegion.Count < roomThreshold)
            {
                foreach (Coord tile in roomRegion)
                {
                    map[tile.TileX, tile.TileY] = 1;
                }
            }
        }
    }

    private List<List<Coord>> GetRegions(int tileType, int[,] map)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[map.GetLength(0), map.GetLength(1)];

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y, map);
                    regions.Add(newRegion);

                    foreach (Coord tile in newRegion)
                    {
                        mapFlags[tile.TileX, tile.TileY] = 1;
                    }
                }
            }
        }

        return regions;
    }

    // todo extract region class
    private List<Coord> GetRegionTiles(int startX, int startY, int[,] map)
    {
        List<Coord> tileList = new List<Coord>();
        int[,] mapFlags = new int[map.GetLength(0), map.GetLength(1)];
        int tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tileList.Add(tile);

            // todo remove loop
            for (int x = tile.TileX - 1; x <= tile.TileX + 1; x++)
            {
                for (int y = tile.TileY - 1; y <= tile.TileY + 1; y++)
                {
                    if (IsInMapRange(x, y, width, height) && (x == tile.TileX || y == tile.TileY))
                    {
                        if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                        {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }

        return tileList;
    }

    private bool IsInMapRange(int x, int y, int maxWidth, int maxHeight)
    {
        return x >= 0 && x < maxWidth && y >= 0 & y < maxHeight ;
    }

    private struct Coord
    {
        private readonly int tileX;
        private readonly int tileY;

        public Coord(int tileX, int tileY)
        {
            this.tileX = tileX;
            this.tileY = tileY;
        }

        public int TileX
        {
            get { return tileX; }
        }

        public int TileY
        {
            get { return tileY; }
        }
    }
}