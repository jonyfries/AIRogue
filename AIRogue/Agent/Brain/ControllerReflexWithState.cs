using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using AIRogue.Engine;
using AIRogue.GameObjects;
using AIRogue.Agent.Brain.Constructs;

namespace AIRogue.Agent.Brain
{
    class ControllerReflexWithState : Controller
    {
        protected List<Vector2> directionList = new List<Vector2> { Vector2.UnitY, -Vector2.UnitY, Vector2.UnitX, -Vector2.UnitX };

        public ControllerReflexWithState() : base() { }
        public ControllerReflexWithState(Agent _parent, Goal.GoalState _goal) : base(_parent, _goal) { }

        /// <summary>
        /// Determine next action to take
        /// </summary>
        /// <returns>The action the controller wishes to make.</returns>
        public override Action.Option GetAction()
        {
            Action.Option choice = Action.Option.WAIT;

            UpdateLocationScore();

            if (this.parent.RecallRelativeLocation(Vector2.Zero).Item.itemType == ItemType.TREASURE)
            {
                choice = Action.Option.PICKUP;
            }

            if (choice == Action.Option.WAIT)
            {
                choice = DetermineMovement();
            }

            parent.HasMoved = false;

            return choice;
        }

        /// <summary>
        /// Determine how to weight different locations based on treasure visible on the map.
        /// </summary>
        private void WeightLocations()
        {
            float treasureAttinuationFactor = 2;

            foreach (Vector2 position in parent.GetRememberedLocations())
            {
                ConstructLocation location = parent.RecallMemoryLocation(position);
                if (!(location.Item.itemType == ItemType.TREASURE))
                {
                    continue;
                }

                float treasureLure = 16384;
                location.Score.ExtrinsicScore += treasureLure;
                treasureLure /= treasureAttinuationFactor;

                Dictionary<Vector2, ConstructLocation> edge = GetAdjacentLocations(position);

                while (treasureLure >= 1 && edge.Count > 0)
                {
                    Dictionary<Vector2, ConstructLocation> newEdge = new Dictionary<Vector2, ConstructLocation>();

                    foreach (Vector2 edgePosition in edge.Keys)
                    {
                        edge[edgePosition].Score.ExtrinsicScore += treasureLure;
                        GetAdjacentLocations(edgePosition).ToList().ForEach(x => newEdge[x.Key] = x.Value);
                    }

                    treasureLure /= treasureAttinuationFactor;

                    //Ensure that positions that have already been handled aren't handled again.
                    foreach (Vector2 edgePosition in edge.Keys)
                    {
                        newEdge.Remove(edgePosition);
                    }

                    edge = newEdge;
                }
            }
        }

        /// <summary>
        /// Determine how to weight different directions based on treasure visible on the map.
        /// </summary>
        /// <returns>Dictionary of weights</returns>
        private Dictionary<Vector2, float> WeightDirections()
        {
            const float TREASURE_WEIGHT = 500;
            Dictionary<Vector2, float> directionWeight = new Dictionary<Vector2, float>();
            foreach (Vector2 direction in directionList)
            {
                directionWeight[direction] = 0;
            }

            Vector2 agentPosition = parent.MemoryPosition;
            int manhattanDistance;
            foreach (Vector2 position in parent.GetRememberedLocations())
            {
                if (parent.RecallMemoryLocation(position).Item.itemType == ItemType.TREASURE)
                {
                    if (position.X > agentPosition.X)
                    {
                        manhattanDistance = (int)(position.X - agentPosition.X + Math.Abs(position.Y - agentPosition.Y));
                        directionWeight[Vector2.UnitX] += TREASURE_WEIGHT / (manhattanDistance * manhattanDistance);
                    }
                    else if (position.X < agentPosition.X)
                    {
                        manhattanDistance = (int)(agentPosition.X - position.X + Math.Abs(position.Y - agentPosition.Y));
                        directionWeight[-Vector2.UnitX] += TREASURE_WEIGHT / (manhattanDistance * manhattanDistance);
                    }
                    if (position.Y > agentPosition.Y)
                    {
                        manhattanDistance = (int)(position.Y - agentPosition.Y + Math.Abs(position.X - agentPosition.X));
                        directionWeight[Vector2.UnitY] += TREASURE_WEIGHT / (manhattanDistance * manhattanDistance);
                    }
                    else if (position.Y < agentPosition.Y)
                    {
                        manhattanDistance = (int)(agentPosition.Y - position.Y + Math.Abs(position.X - agentPosition.X));
                        directionWeight[-Vector2.UnitY] += TREASURE_WEIGHT / (manhattanDistance * manhattanDistance);
                    }
                }
            }

            return directionWeight;
        }

        /// <summary>
        /// Get all of the ConstructLocations adjacent to a point that are walkable
        /// </summary>
        /// <param name="position">Position to get adjacent ConstructLocations to</param>
        /// <returns>Dictionary of walkable locations</returns>
        public Dictionary<Vector2, ConstructLocation> GetAdjacentLocations(Vector2 position)
        {
            Dictionary<Vector2, ConstructLocation> adjacentDict = new Dictionary<Vector2, ConstructLocation>();
            foreach (Vector2 direction in directionList)
            {
                ConstructLocation testLocation = parent.RecallMemoryLocation(position + direction);
                if (testLocation != null && testLocation.IsWalkable)
                {
                    adjacentDict[position + direction] = testLocation;
                }
            }

            return adjacentDict;
        }

        /// <summary>
        /// Update the location score of each ConstructLocation in Agent Memory.
        /// </summary>
        protected void UpdateLocationScore()
        {
            foreach (Vector2 position in parent.GetRememberedLocations())
            {
                parent.RecallMemoryLocation(position).Score.IntrinsicScore += 1;
                parent.RecallMemoryLocation(position).Score.ExtrinsicScore = 0;
            }

            parent.RecallRelativeLocation(Vector2.Zero).Score.IntrinsicScore = 0;
        }

        /// <summary>
        /// Find the next option that the Agent should take
        /// </summary>
        /// <returns>Returns Action.Option</returns>
        protected Action.Option DetermineMovement()
        {
            //Dictionary<Vector2, float> directionWeight = WeightDirections();
            //List<Vector2> directionOptions = new List<Vector2>();
            //foreach (Vector2 direction in directionList)
            //{
            //    ConstructLocation checkLocation = parent.RecallRelativeLocation(direction);
            //    checkLocation.Score.ExtrinsicScore = directionWeight[direction];
            //    if (checkLocation.IsWalkable)
            //    {
            //        directionOptions.Add(direction);
            //    }
            //}

            WeightLocations();
            List<Vector2> directionOptions = new List<Vector2>();
            foreach (Vector2 direction in directionList)
            {
                ConstructLocation checkLocation = parent.RecallRelativeLocation(direction);
                if (checkLocation.IsWalkable)
                {
                    directionOptions.Add(direction);
                }
            }

            double highScore = double.NegativeInfinity;
            int highIndex = -1;
            double directionScore;
            for (int i = 0; i < directionOptions.Count; ++i)
            {
                Vector2 checkPosition = directionOptions[i];
                directionScore = parent.RecallRelativeLocation(checkPosition).Score.Score;
                if (directionScore > highScore) {
                    highScore = directionScore;
                    highIndex = i;
                }
            }

            if (highIndex >= 0)
            {
                return Action.ConvertVector2ToAction(directionOptions[highIndex]);
            }

            return Action.Option.WAIT;
        }
    }
}
