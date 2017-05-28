using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using AIRogue.Agent.Brain.Constructs;

namespace AIRogue.Agent
{
    public class Vision
    {
        public float VisionRange { get; private set; }
        private Agent parent { get; }
        
        public Vision(Agent parent) {
            this.parent = parent;
            VisionRange = 1f;
        }

        /// <summary>
        /// Returns a dictionary with all of the surrounding Locations represented as LocationConstructs that the Agent can see.
        /// </summary>
        /// <returns>Dictionary of surrounding locations</returns>
        public Dictionary<Vector2, ConstructLocation> CheckSurroundings()
        {
            Dictionary<Vector2, ConstructLocation> surroundingsReport = new Dictionary<Vector2, ConstructLocation>();
            Vector2 checkVector = new Vector2();
            for (int i = (int)-VisionRange; i <= VisionRange; ++i)
            {
                for (int j = (int)-VisionRange; j <= VisionRange; ++j)
                {
                    checkVector.X = i;
                    checkVector.Y = j;
                    if (checkVector.Length() <= VisionRange) {
                        surroundingsReport[checkVector] = parent.CreateLocationReport(checkVector);
                    }
                }
            }

            return surroundingsReport;
        }
    }
}
