using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using AIRogue.Mapping;

namespace AIRogue.GameObjects
{
    /// <summary>
    /// The environment in which the Agent will be acting.
    /// </summary>
    public class World
    {
        private Dictionary<Point, Location> map = new Dictionary<Point, Location>();
        public int width = 50;
        public int height = 50;
        public int roomCount = 15;
        public Dictionary<Vector2, Agent.Actor> actorDict = new Dictionary<Vector2, Agent.Actor>();

        /// <summary>
        /// Create the environment in which the Agent will be acting.
        /// </summary>
        public World()
        {
            //Setup map parameters.
            Point mapSize = new Point(width, height);
            Point minRoomSize = new Point(2, 2);
            Point maxRoomSize = new Point(4, 4);
            List<List<BuildDungeon.PointType>> mapOutline = new BuildDungeon().CreateMap(roomCount, mapSize, minRoomSize, maxRoomSize);

            //Create world from map.
            for (int i = 0; i < width; ++i) {
                for (int j = 0; j < height; ++j) {
                    if (mapOutline[i][j] == BuildDungeon.PointType.CLEAR)
                    {
                        Point position = new Point(i, j);
                        map[position] = new Location(true, position, true);
                    }
                }
            }
        }

        /// <summary>
        /// Add an actor to the world.
        /// </summary>
        /// <param name="newActor">Actor to be added</param>
        public void AddActor(Agent.Actor newActor)
        {
            actorDict[newActor.Position] = newActor;
        }

        /// <summary>
        /// Get all the actors in the world.
        /// </summary>
        /// <returns>Dictionary of actors</returns>
        public Dictionary<Vector2, Agent.Actor> GetActorDict()
        {
            return actorDict;
        }

        /// <summary>
        /// Get the total number of items of a specific type in the world.
        /// </summary>
        /// <param name="itemType">The type of item to check.</param>
        /// <returns>int</returns>
        public int GetItemCount(Type itemType) {
            IItem checkItem;
            int itemCount = 0;

            foreach (Point key in map.Keys)
            {
                if (map[key].LookItem(out checkItem) && itemType == checkItem.GetType()) {
                    ++itemCount;
                }
            }

            return itemCount;
        }

        /// <summary>
        /// Check if an actor can move into a Location.
        /// </summary>
        /// <param name="actor">Actor wishing to move.</param>
        /// <param name="direction">The direction the agent is moving</param>
        /// <returns>Bool</returns>
        public bool IsLegalMove(Agent.Actor actor, Vector2 direction) {
            Point checkPosition = new Point((int)actor.Position.X, (int)actor.Position.Y) + new Point((int)direction.X, (int)direction.Y);
            if (map.ContainsKey(checkPosition))
            {
                return map[checkPosition].IsWalkable;
            }

            return false;
        }

        /// <summary>
        /// Try to pick up the item at a Location.
        /// </summary>
        /// <param name="actor">The actor attempting to picup the item.</param>
        /// <param name="item">IItem that the agent picks up</param>
        /// <returns>Bool</returns>
        public bool TryPickup(Agent.Actor actor, out IItem item) {
            Point checkPosition = new Point((int)actor.Position.X, (int)actor.Position.Y);
            return (map[checkPosition].PickupItem(out item));
        }

        /// <summary>
        /// Get the Location at a given Vector2 position.
        /// </summary>
        /// <param name="getPosition">The position of the Location to return.</param>
        /// <returns>Returns the Location at getPosition or null if that position isn't on the map.</returns>
        public Location GetLocation(Vector2 getPosition)
        {
            Point position = new Point((int)getPosition.X, (int)getPosition.Y);
            if (map.ContainsKey(position))
            {
                return map[position];
            } else
            {
                map[position] = new Location(false, position, false);
                return map[position];
            }
        }
    }
}
