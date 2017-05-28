using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIRogue.GameObjects;

namespace AIRogue.Goal
{
    /// <summary>
    /// The state of the world that the agent is attempting to achieve.
    /// </summary>
    public abstract class GoalState
    {
        protected World world;

        /// <summary>
        /// Create the GoalState for the World.
        /// </summary>
        /// <param name="_world">The world in which the agent is acting.</param>
        internal GoalState(World _world) {
            world = _world;
        }

        /// <summary>
        /// Test if the World passed into the constructor is a state which meets the desired state.
        /// </summary>
        /// <returns>Returns true if the world is a desired GoalState.</returns>
        internal abstract bool IsGoalState();
    }
}
