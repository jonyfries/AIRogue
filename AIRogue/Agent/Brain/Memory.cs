using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AIRogue.Agent.Brain.Constructs;

namespace AIRogue.Agent.Brain
{
    public class Memory
    {
        private Agent parent;
        public Vector2 position;
        public Dictionary<Vector2, ConstructLocation> locationDict = new Dictionary<Vector2, ConstructLocation>();

        public Memory(Agent _parent)
        {
            this.parent = _parent;
            position = Vector2.Zero;
        }

        public void AddPosition(Vector2 relativePosition, ConstructLocation location)
        {
            Vector2 addPosition = position + relativePosition;

            if (locationDict.ContainsKey(addPosition))
            {
                locationDict[addPosition].IsWalkable = location.IsWalkable;
                locationDict[addPosition].Item = location.Item;
            }
            else
            {
                locationDict[addPosition] = location;
            }
        }

        public void AddSet(Dictionary<Vector2, ConstructLocation> locationDict)
        {
            foreach (Vector2 key in locationDict.Keys)
            {
                AddPosition(key, locationDict[key]);
            }
        }
    }
}
