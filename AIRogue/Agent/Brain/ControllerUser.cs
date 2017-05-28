using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AIRogue.Agent
{
    public class ControllerUser : Controller
    {
        KeyboardState previousState;
        List<Keys> moveDown = new List<Keys> { Keys.S, Keys.Down };
        List<Keys> moveUp = new List<Keys> { Keys.W, Keys.Up };
        List<Keys> moveRight = new List<Keys> { Keys.D, Keys.Right };
        List<Keys> moveLeft = new List<Keys> { Keys.A, Keys.Left };
        List<Keys> pickup = new List<Keys> { Keys.Q, Keys.LeftShift };

        public ControllerUser() : base() { }
        public ControllerUser(Agent _parent, Goal.GoalState _goal) : base(_parent, _goal) { }

        public override Action.Option GetAction() {
            KeyboardState keyState = Keyboard.GetState();
            Action.Option nextAction = Action.Option.WAIT;

            foreach (Keys key in keyState.GetPressedKeys())
            {
                if (previousState.IsKeyDown(key))
                {
                    continue;
                }

                if (moveDown.Exists(c => c.Equals(key))) {
                    nextAction = Action.Option.MOVE_DOWN;
                }
                if (moveUp.Exists(c => c.Equals(key)))
                {
                    nextAction = Action.Option.MOVE_UP;
                }
                if (moveRight.Exists(c => c.Equals(key)))
                {
                    nextAction = Action.Option.MOVE_RIGHT;
                }
                if (moveLeft.Exists(c => c.Equals(key)))
                {
                    nextAction = Action.Option.MOVE_LEFT;
                }
                if (pickup.Exists(c => c.Equals(key)))
                {
                    nextAction = Action.Option.PICKUP;
                }
            }
            previousState = keyState;

            return nextAction;
        }
    }
}
