using System;
using System.Collections.Generic;
using DataStructures;
using Factories;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
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

    public int passagewayRadius = 1;

    private MapFactory mapFactory;
    private MapPropertiesFactory mapPropertiesFactory;
    private int[,] map;

    private void Start() {
        mapFactory = proceduralCaveGenerationAbstractFactory.MapFactory;
        mapPropertiesFactory = proceduralCaveGenerationAbstractFactory.MapPropertiesFactory;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            MapProperties mapProperties = mapPropertiesFactory.CreateMapProperties(width, height, seed, useRandomSeed,
                                                                                   randomFillPercent, smoothingIterationCount, squareSize);

            map = mapFactory.CreateMap(mapProperties);

            ProcessMap(map, wallThreshold, roomThreshold);

            // todo move to map factory
            int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

            for (int i = 0; i < borderedMap.GetLength(0); i++) {
                for (int j = 0; j < borderedMap.GetLength(1); j++) {
                    if (i >= borderSize && i < width + borderSize && j >= borderSize && j < height + borderSize) {
                        borderedMap[i, j] = map[i - borderSize, j - borderSize];
                    } else {
                        borderedMap[i, j] = 1;
                    }
                }
            }

            MeshGenerator meshGenerator = GetComponent<MeshGenerator>();
            meshGenerator.GenerateMesh(borderedMap, squareSize);
        }
    }

    // todo move
    private void ProcessMap(int[,] map, int wallThreshold, int roomThreshold) {
        // todo extract method
        List<List<Coord>> wallRegions = GetRegions(1, map);

        foreach (List<Coord> wallRegion in wallRegions) {
            if (wallRegion.Count < wallThreshold) {
                foreach (Coord tile in wallRegion) {
                    map[tile.TileX, tile.TileY] = 0;
                }
            }
        }

        List<List<Coord>> roomRegions = GetRegions(0, map);
        List<Room> survivingRooms = new List<Room>();

        foreach (List<Coord> roomRegion in roomRegions) {
            if (roomRegion.Count < roomThreshold) {
                foreach (Coord tile in roomRegion) {
                    map[tile.TileX, tile.TileY] = 1;
                }
            } else {
                survivingRooms.Add(new Room(roomRegion, map));
            }
        }

        survivingRooms.Sort();
        survivingRooms[0].isMainRoom = true;
        survivingRooms[0].isAccessibleFromMainRoom = true;

        ConnectClosesRooms(survivingRooms);
    }

    void ConnectClosesRooms(List<Room> allRooms, bool forceAccessiblityFromMainRoom = false) {
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceAccessiblityFromMainRoom) {
            foreach (Room room in allRooms) {
                if (room.isAccessibleFromMainRoom) {
                    roomListB.Add(room);
                } else {
                    roomListA.Add(room);
                }
            }
        } else {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        // todo consider extracting struct or class
        int bestDistance = 0;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;

        foreach (Room roomA in roomListA) {
            if (!forceAccessiblityFromMainRoom) {
                possibleConnectionFound = false;
                if (roomA.ConnectedRooms.Count > 0) {
                    continue;
                }
            }
            foreach (Room roomB in roomListB) {
                if (roomA == roomB || roomA.IsConnected(roomB)) {
                    continue;
                }

                foreach (Coord tileA in roomA.EdgeTiles) {
                    foreach (Coord tileB in roomB.EdgeTiles) {
                        int distanceBetweenRoom
                            = (int) (Math.Pow(tileA.TileX - tileB.TileX, 2) + Math.Pow(tileA.TileY - tileB.TileY, 2));

                        if (distanceBetweenRoom < bestDistance || !possibleConnectionFound) {
                            bestDistance = distanceBetweenRoom;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }

            if (possibleConnectionFound && !forceAccessiblityFromMainRoom) {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }

        if (possibleConnectionFound && forceAccessiblityFromMainRoom) {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosesRooms(allRooms, true);
        }

        if (!forceAccessiblityFromMainRoom) {
            ConnectClosesRooms(allRooms, true);
        }
    }

    void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB) {
        roomA.ConnectWith(roomB);

        List<Coord> line = GetLine(tileA, tileB);
        foreach (Coord coord in line) {
            DrawCircle(coord, passagewayRadius);
        }
    }

    void DrawCircle(Coord coord, int radius) {
        for (int x = -radius; x <= radius; x++) {
            for (int y = -radius; y <= radius; y++) {
                if (x * x + y * y <= radius * radius) {
                    int drawX = coord.TileX + x;
                    int drawY = coord.TileY + y;
                    if (IsInMapRange(drawX, drawY, width, height)) {
                        map[drawX, drawY] = 0;
                    }
                }
            }
        }
    }

    List<Coord> GetLine(Coord from, Coord to) {
        List<Coord> line = new List<Coord>();

        int x = from.TileX;
        int y = from.TileY;

        int dx = to.TileX - from.TileX;
        int dy = to.TileY - from.TileY;

        bool inverted = false;
        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest) {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }

        int gradientAccumulation = longest / 2;
        for (int i = 0; i < longest; i++) {
            line.Add(new Coord(x, y));

            if (inverted) {
                y += step;
            } else {
                x += step;
            }

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest) {
                if (inverted) {
                    x += gradientStep;
                } else {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }

        return line;
    }

    private List<List<Coord>> GetRegions(int tileType, int[,] map) {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[map.GetLength(0), map.GetLength(1)];

        for (int x = 0; x < map.GetLength(0); x++) {
            for (int y = 0; y < map.GetLength(1); y++) {
                if (mapFlags[x, y] == 0 && map[x, y] == tileType) {
                    List<Coord> newRegion = GetRegionTiles(x, y, map);
                    regions.Add(newRegion);

                    foreach (Coord tile in newRegion) {
                        mapFlags[tile.TileX, tile.TileY] = 1;
                    }
                }
            }
        }

        return regions;
    }

    // todo extract region class
    private List<Coord> GetRegionTiles(int startX, int startY, int[,] map) {
        List<Coord> tileList = new List<Coord>();
        int[,] mapFlags = new int[map.GetLength(0), map.GetLength(1)];
        int tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0) {
            Coord tile = queue.Dequeue();
            tileList.Add(tile);

            // todo remove loop
            for (int x = tile.TileX - 1; x <= tile.TileX + 1; x++) {
                for (int y = tile.TileY - 1; y <= tile.TileY + 1; y++) {
                    if (IsInMapRange(x, y, width, height) && (x == tile.TileX || y == tile.TileY)) {
                        if (mapFlags[x, y] == 0 && map[x, y] == tileType) {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }

        return tileList;
    }

    private bool IsInMapRange(int x, int y, int maxWidth, int maxHeight) {
        return x >= 0 && x < maxWidth && y >= 0 & y < maxHeight;
    }
}