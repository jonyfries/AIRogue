using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace AIRogue.Engine
{
    static public class Clock
    {
        static public GameTime gameTime;

        /// <summary>
        /// Get the time since the last frame in seconds.
        /// </summary>
        /// <returns>float</returns>
        static public float GetTick()
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Get the time since the game started in seconds.
        /// </summary>
        /// <returns>float</returns>
        static public float GetTime()
        {
            return (float)gameTime.TotalGameTime.TotalSeconds;
        }
    }
}
