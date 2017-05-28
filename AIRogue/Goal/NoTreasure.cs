using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIRogue.GameObjects;

namespace AIRogue.Goal
{
    /// <summary>
    /// GoalState is achieved when there is no ItemTreasure on the map.
    /// </summary>
    class NoTreasure : GoalState
    {
        /// <summary>
        /// Create the GoalState so that there must be no ItemTreasure in the world.
        /// </summary>
        /// <param name="_world">The World in which the Agent is acting.</param>
        internal NoTreasure(World _world) : base(_world) { }

        /// <summary>
        /// Determine if the World passed to the constructor has no ItemTreasure in it.
        /// </summary>
        /// <returns>Returns true if there is no ItemTreasure in the World.</returns>
        internal override bool IsGoalState() {
            return world.GetItemCount(typeof(ItemTreasure)) == 0;
        }
    }
}
