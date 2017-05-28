using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace AIRogue.Agent.Brain
{
    public abstract class Controller
    {
        protected Goal.GoalState goal;
        public Agent parent;

        public Controller() { }

        public Controller(Agent _parent, Goal.GoalState _goal)
        {
            this.goal = _goal;
            this.parent = _parent;
        }

        abstract public Action.Option GetAction();
    }
}
