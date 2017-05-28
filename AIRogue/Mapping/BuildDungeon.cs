using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using AIRogue.Engine;

namespace AIRogue.Mapping
{
    class BuildDungeon
    {
        public enum PointType
        {
            WALL,
            CLEAR
        }

        enum Direction
        {
            NORTH,
            EAST,
            SOUTH,
            WEST
        }

        private List<List<PointType>> map = new List<List<PointType>>();
        private List<Rectangle> roomList = new List<Rectangle>();
        private List<Rectangle> exteriorRoomList = new List<Rectangle>();
        private int roomsCreated = 0;

        Point minRoomSize;
        Point maxRoomSize;

        /// <summary>
        /// Creates a map by creating a room in a random place on the map and then building rooms off of that. 
        /// </summary>
        /// <param name="roomCount">Maximum number of rooms to include in the map</param>
        /// <param name="mapSize">The size of the map.</param>
        /// <param name="minRoomSize">The smallest allowable room. Must be smaller than or equal to maxSize in both directions.</param>
        /// <param name="maxRoomSize">The largest allowable room. Must be larger than or equal to minSize in both directions.</param>
        public List<List<PointType>> CreateMap(int roomCount, Point mapSize, Point minRoomSize, Point maxRoomSize)
        {
            //Set the number of rooms created.
            roomsCreated = 0;

            this.minRoomSize = minRoomSize;
            //Add one to max size so that Random number generation will return upt ot he maxSize passed.
            this.maxRoomSize.X = maxRoomSize.X + 1;
            this.maxRoomSize.Y = maxRoomSize.Y + 1;

            //Create the blank map.
            for (int i=0; i < mapSize.X; ++i)
            {
                map.Add(new List<PointType>());
                for (int j=0; j < mapSize.Y; ++j)
                {
                    map[i].Add(PointType.WALL);
                }
            }

            //Create the first room
            Point roomSize = Random.Next(minRoomSize, maxRoomSize);
            Point roomPosition = Random.Next(mapSize - roomSize);

            Rectangle room = new Rectangle(roomPosition, roomSize);
            AddRoom(room);

            while (roomsCreated < roomCount)
            {
                if (exteriorRoomList.Count == 0)
                {
                    break;
                }
                CreateRoom();
            }

            return map;
        }

        /// <summary>
        /// Attempts to create a room in the dungeon.
        /// </summary>
        /// <returns>true if a room was successfully created.</returns>
        private bool CreateRoom()
        {
            //Ensure that rooms can still be added.
            if (exteriorRoomList.Count == 0)
            {
                return false;
            }

            //Determine room to build off of.
            Rectangle parentRoom = Random.Choice<Rectangle>(exteriorRoomList);

            //Determine size of new room.
            Point roomSize = Random.Next(minRoomSize, maxRoomSize);

            //Set all of the directions to try to create the new room
            List<Direction> directionList = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList<Direction>();

            bool roomCreated;
            Rectangle room;

            do
            {
                //Determine which direction the new room should go from the old room.
                Direction direction = Random.Choice<Direction>(directionList);
                directionList.Remove(direction);

                //Determine the minPoint and maxPoint of the new room based on it's size.
                int minX, minY, maxX, maxY;
                switch (direction)
                {
                    case Direction.NORTH:
                        minX = parentRoom.Left - roomSize.X + 1;
                        maxX = parentRoom.Right;
                        minY = parentRoom.Bottom + 1;
                        maxY = parentRoom.Bottom + 1;
                        break;
                    case Direction.SOUTH:
                        minX = parentRoom.Left - roomSize.X + 1;
                        maxX = parentRoom.Right;
                        minY = parentRoom.Top - roomSize.Y - 1;
                        maxY = parentRoom.Top - roomSize.Y - 1;
                        break;
                    case Direction.EAST:
                        minX = parentRoom.Right + 1;
                        maxX = parentRoom.Right + 1;
                        minY = parentRoom.Top - roomSize.Y + 1;
                        maxY = parentRoom.Bottom;
                        break;
                    case Direction.WEST:
                        minX = parentRoom.Left - roomSize.X - 1;
                        maxX = parentRoom.Left - roomSize.X - 1;
                        minY = parentRoom.Top - roomSize.Y + 1;
                        maxY = parentRoom.Bottom;
                        break;
                    default:
                        return false;
                }

                //Create the room
                Point topLeft = new Point(minX, minY);
                Point bottomRight = new Point(maxX, maxY);
                Point roomPosition = Random.Next(topLeft, bottomRight);
                room = new Rectangle(roomPosition, roomSize);

                //Check if room can be placed
                roomCreated = CheckRoom(room);

                //Loop until the room is placed or all directions have been tried.
            } while (!roomCreated && directionList.Count > 0);

            if (!roomCreated)
            {
                exteriorRoomList.Remove(parentRoom);
                return false;
            }

            AddRoom(room);
            ConnectRooms(room, parentRoom);
            return true;
        }

        /// <summary>
        /// Check if a room is overlaps a cleared area on the map.
        /// </summary>
        /// <param name="room">Room to check</param>
        /// <returns>True if room does not overlap a cleared area on the map.</returns>
        private bool CheckRoom(Rectangle room)
        {
            if (room.Left < 0 || room.Top < 0 || room.Right >= map.Count || room.Bottom >= map[0].Count)
            {
                return false;
            }

            for (int i = -1; i <= room.Width; ++i)
            {
                for (int j = -1; j <= room.Height; ++j)
                {
                    try {
                        if (map[room.Left + i][room.Top + j] == PointType.CLEAR) return false;
                    }
                    catch (ArgumentOutOfRangeException) { }
                }
            }

            return true;
        }

        /// <summary>
        /// Clears all of the areas with in the room.
        /// </summary>
        /// <param name="room">Room to be added to the map.</param>
        private void AddRoom(Rectangle room)
        {
            for (int i = 0; i < room.Width; ++i)
            {
                for (int j = 0; j < room.Height; ++j)
                {
                    map[room.Left + i][room.Top + j] = PointType.CLEAR;
                }
            }
            roomList.Add(room);
            exteriorRoomList.Add(room);
            roomsCreated += 1;
        }

        /// <summary>
        /// Creates a hall between two room one Point way from each other in either X or Y direction.
        /// </summary>
        /// <param name="room1"></param>
        /// <param name="room2"></param>
        private void ConnectRooms(Rectangle room1, Rectangle room2)
        {
            int minX, maxX, minY, maxY;
            //Determine area where the hallway may be placed.
            //If room2 is above room1
            if (room1.Top > room2.Bottom)
            {
                minX = room1.Left < room2.Left ? room2.Left : room1.Left;
                maxX = room1.Right < room2.Right ? room1.Right: room2.Right;
                minY = room1.Top - 1;
                maxY = room1.Top - 1;
            }
            //If room2 is below room 1
            else if (room1.Bottom < room2.Top)
            {
                minX = room1.Left < room2.Left ? room2.Left : room1.Left;
                maxX = room1.Right < room2.Right ? room1.Right : room2.Right;
                minY = room1.Bottom;
                maxY = room1.Bottom;
            }
            //If room2 is left of room1
            else if (room1.Left > room2.Right)
            {
                minX = room1.Left - 1;
                maxX = room1.Left - 1;
                minY = room1.Top < room2.Top ? room2.Top : room1.Top;
                maxY = room1.Bottom < room2.Bottom ? room1.Bottom: room2.Bottom;
            }
            //if room2 is right of room1
            else if (room1.Right < room2.Left)
            {
                minX = room1.Right;
                maxX = room1.Right;
                minY = room1.Top < room2.Top ? room2.Top : room1.Top;
                maxY = room1.Bottom < room2.Bottom ? room1.Bottom : room2.Bottom;
            }
            //if the rooms are within each other...
            else
            {
                throw (new Exception());
            }

            Point topLeft = new Point(minX, minY);
            Point bottomRight = new Point(maxX, maxY);
            Point hallway = Random.Next(topLeft, bottomRight);

            map[hallway.X][hallway.Y] = PointType.CLEAR;
        }
    }
}
