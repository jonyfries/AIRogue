using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AIRogue.Agent.Brain
{
    public class ControllerUser : Controller
    {
        List<Keys> moveDown = new List<Keys> { Keys.S, Keys.Down };
        List<Keys> moveUp = new List<Keys> { Keys.W, Keys.Up };
        List<Keys> moveRight = new List<Keys> { Keys.D, Keys.Right };
        List<Keys> moveLeft = new List<Keys> { Keys.A, Keys.Left };
        List<Keys> pickup = new List<Keys> { Keys.Q, Keys.LeftShift };

        Action.Option nextAction = Action.Option.WAIT;

        public ControllerUser() : base() { }
        public ControllerUser(Agent _parent, Goal.GoalState _goal) : base(_parent, _goal)
        {
            Engine.InputHandler.KeyDownListeners.Add(this.OnKeyDown);
            Engine.InputHandler.KeyUpListeners.Add(this.OnKeyUp);
        }

        private void OnKeyDown(Keys key)
        {
            if (moveDown.Exists(c => c.Equals(key)))
            {
                nextAction = Action.Option.MOVE_DOWN;
            }
            else if (moveUp.Exists(c => c.Equals(key)))
            {
                nextAction = Action.Option.MOVE_UP;
            }
            else if (moveRight.Exists(c => c.Equals(key)))
            {
                nextAction = Action.Option.MOVE_RIGHT;
            }
            else if (moveLeft.Exists(c => c.Equals(key)))
            {
                nextAction = Action.Option.MOVE_LEFT;
            }
            else if (pickup.Exists(c => c.Equals(key)))
            {
                nextAction = Action.Option.PICKUP;
            }
        }

        private void OnKeyUp(Keys key)
        {
            switch (nextAction)
            {
                case Action.Option.MOVE_DOWN:
                    if (moveDown.Exists(c => c.Equals(key)))
                    {
                        nextAction = Action.Option.WAIT;
                    }
                    break;
                case Action.Option.MOVE_LEFT:
                    if (moveLeft.Exists(c => c.Equals(key)))
                    {
                        nextAction = Action.Option.WAIT;
                    }
                    break;
                case Action.Option.MOVE_RIGHT:
                    if (moveRight.Exists(c => c.Equals(key)))
                    {
                        nextAction = Action.Option.WAIT;
                    }
                    break;
                case Action.Option.MOVE_UP:
                    if (moveUp.Exists(c => c.Equals(key)))
                    {
                        nextAction = Action.Option.WAIT;
                    }
                    break;
                case Action.Option.PICKUP:
                    if (pickup.Exists(c => c.Equals(key)))
                    {
                        nextAction = Action.Option.WAIT;
                    }
                    break;
            }
        }

        public override Action.Option GetAction() {
            return nextAction;
        }
    }
}
