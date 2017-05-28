using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace AIRogue.Agent
{
    public static class Action {
        public enum Option {
            MOVE_DOWN,
            MOVE_UP,
            MOVE_RIGHT,
            MOVE_LEFT,
            PICKUP,
            WAIT,
        }

        static public Option ConvertVector2ToAction(Vector2 direction) {
            if (direction == Vector2.UnitY) {
                return Option.MOVE_DOWN;
            } else if (direction == -Vector2.UnitY){
                return Option.MOVE_UP;
            } else if (direction == Vector2.UnitX) {
                return Option.MOVE_RIGHT;
            } else if (direction == -Vector2.UnitX) {
                return Option.MOVE_LEFT;
            }

            return Option.WAIT;
        }

        static public Vector2 ConvertActionToVector2(Action.Option direction){
            switch (direction) {
                case Action.Option.MOVE_DOWN:
                    return Vector2.UnitY;
                case Action.Option.MOVE_UP:
                    return -Vector2.UnitY;
                case Action.Option.MOVE_RIGHT:
                    return Vector2.UnitX;
                case Action.Option.MOVE_LEFT:
                    return -Vector2.UnitX;
                default:
                    return Vector2.Zero;
            }
        }
    }
}
