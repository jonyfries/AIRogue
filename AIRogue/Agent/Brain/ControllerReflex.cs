using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using AIRogue.Engine;
using AIRogue.GameObjects;
using AIRogue.Agent.Brain.Constructs;

namespace AIRogue.Agent
{
    public class ControllerReflex : Controller
    {
        protected List<Vector2> directionList = new List<Vector2> { Vector2.UnitY, -Vector2.UnitY, Vector2.UnitX, -Vector2.UnitX };

        public ControllerReflex() : base() { }
        public ControllerReflex(Agent _parent, Goal.GoalState _goal) : base(_parent, _goal) { }

        public override Action.Option GetAction()
        {
            ConstructLocation currentLocation = parent.RecallRelativeLocation(Vector2.Zero);
            if (currentLocation.Item.itemType == ItemType.TREASURE) {
                return Action.Option.PICKUP;
            } else {
                List<Vector2> directionList = CheckSurrounding();
                Vector2 choice = directionList[Random.Next(0, directionList.Count)];
                return Action.ConvertVector2ToAction(choice);
            }
        }

        protected List<Vector2> CheckSurrounding()
        {
            List<Vector2> directionOptions = new List<Vector2>();
            List<Vector2> treasureOptions = new List<Vector2>();
            foreach (Vector2 direction in directionList) {
                ConstructLocation checkLocation = parent.RecallRelativeLocation(direction);
                if (checkLocation.Item.itemType == ItemType.TREASURE) {
                    treasureOptions.Add(direction);
                } else if (checkLocation.IsWalkable) {
                    directionOptions.Add(direction);
                }
            }

            if (treasureOptions.Count > 0) {
                return treasureOptions;
            } else if (directionOptions.Count > 0) {
                return directionOptions;
            } else {
                return new List<Vector2> { Vector2.Zero };
            }
        }
    }
}
