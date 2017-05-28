using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using AIRogue.Engine;
using AIRogue.GameObjects;

namespace AIRogue.Agent
{
    public class Actor : IGameObject
    {
        public Vector2 Position { get; private set; }
        public Drawable Drawable { get; private set; }
        private Agent parent;
        private World world;
        Action.Option nextAction;
        public string TextureName { get; private set; }

        internal Actor(Agent _parent, string _image, Vector2 _position, World _world) {
            this.parent = _parent;
            this.Drawable = new Drawable(_image);
            TextureName = _image;
            this.Position = _position;
            this.world = _world;
            ObjectHandler.AddGameObject(this, ObjectHandler.GameObjectLayer.ACTOR);
        }

        /// <summary>
        /// Set the action the Actor will take next update.
        /// </summary>
        /// <param name="action">The action the Actor will take.</param>
        public void SetAction(Action.Option action) {
            nextAction = action;
        }

        /// <summary>
        /// Process the Update
        /// </summary>
        public void Update()
        {
            switch (nextAction)
            {
                case Action.Option.MOVE_DOWN:
                    Move(Vector2.UnitY);
                    break;
                case Action.Option.MOVE_UP:
                    Move(-Vector2.UnitY);
                    break;
                case Action.Option.MOVE_RIGHT:
                    Move(Vector2.UnitX);
                    break;
                case Action.Option.MOVE_LEFT:
                    Move(-Vector2.UnitX);
                    break;
                case Action.Option.PICKUP:
                    Pickup();
                    break;
            }

            parent.OnAgentAction();
            nextAction = Action.Option.WAIT;
        }

        /// <summary>
        /// Try to pickup the item at the current location.
        /// </summary>
        private void Pickup() {
            IItem item;
            this.world.TryPickup(this, out item);
        }

        /// <summary>
        /// Move the Actor in direction.
        /// </summary>
        /// <param name="direction">Unit Vector2 with the direction the actor is to move.</param>
        private void Move(Vector2 direction)
        {
            if (this.world.IsLegalMove(this, direction))
            {
                Position += direction;
                parent.OnAgentMove(direction);
            }
        }
    }
}
