using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRogue.Agent.Brain.Constructs
{
    public class ConstructScore
    {
        public float Score
        {
            get
            {
                return IntrinsicScore + ExtrinsicScore;
            }
        }

        public float IntrinsicScore = 0;
        public float ExtrinsicScore = 0;
    }
}
