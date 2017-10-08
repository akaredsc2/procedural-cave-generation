using System;
using System.Collections.Generic;

namespace DataStructures {
    public class Room : IComparable<Room> {
        private readonly List<Coord> tiles;
        private readonly List<Coord> edgeTiles;
        private readonly List<Room> connectedRooms;
        private readonly int roomSize;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;

        public Room() {
        }

        public Room(List<Coord> tiles, int[,] map) {
            this.tiles = tiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();
            edgeTiles = new List<Coord>();

            // todo move this logic to factory
            // todo consider removing loop
            foreach (Coord tile in tiles) {
                for (int x = tile.TileX - 1; x < tile.TileX + 1; x++) {
                    for (int y = tile.TileY - 1; y < tile.TileY + 1; y++) {
                        if (x == tile.TileX || y == tile.TileY) {
                            if (map[x, y] == 1) {
                                edgeTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        public List<Room> ConnectedRooms {
            get { return connectedRooms; }
        }

        public List<Coord> EdgeTiles {
            get { return edgeTiles; }
        }

        public void MarkAccessibleFromMainRoom() {
            if (!isAccessibleFromMainRoom) {
                isAccessibleFromMainRoom = true;
                foreach (Room connectedRoom in connectedRooms) {
                    connectedRoom.MarkAccessibleFromMainRoom();
                }
            }
        }

        public void ConnectWith(Room other) {
            if (this.isAccessibleFromMainRoom) {
                other.MarkAccessibleFromMainRoom();
            }
            if (other.isAccessibleFromMainRoom) {
                this.MarkAccessibleFromMainRoom();
            }
            this.connectedRooms.Add(other);
            other.connectedRooms.Add(this);
        }
        
        public bool IsConnected(Room otherRoom) {
            return connectedRooms.Contains(otherRoom);
        }

        public int CompareTo(Room other) {
            return other.roomSize.CompareTo(roomSize);
        }
    }
}