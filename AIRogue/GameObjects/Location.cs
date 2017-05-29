using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using AIRogue.Engine;

namespace AIRogue.GameObjects
{
    /// <summary>
    /// A position on the map.
    /// </summary>
    public class Location : IGameObject
    {
        public bool IsWalkable { get; private set; }
        public Vector2 Position { get; private set; }
        public string TextureName { get; }
        public Drawable Drawable { get; set; }
        IItem item;
        
        /// <summary>
        /// Create a position on the map.
        /// </summary>
        /// <param name="_passable">Bool: Can an Agent be in this position.</param>
        /// <param name="_position">Vector2: The position of the Location on the map.</param>
        /// <param name="_hasTreasure">Bool: Does the Location contain an ItemTreasure.</param>
        public Location(bool _passable, Vector2 _position, bool _hasTreasure) {
            TextureName = _passable ? "floor" : "wall";

            this.Position = _position;
            this.IsWalkable = _passable;
            ObjectHandler.AddGameObject(this, ObjectHandler.GameObjectLayer.BACKGROUND);
            Drawable = new Drawable(TextureName);

            if (_hasTreasure && _passable)
            {
                item = new ItemTreasure(this.Position);
            } else {
                item = new ItemEmpty();
            }
        }

        public Location(bool _passable, Point _position, bool _hasTreasure)
        {
            TextureName = _passable ? "floor" : "wall";

            this.Position = new Vector2 (_position.X, _position.Y);
            this.IsWalkable = _passable;
            ObjectHandler.AddGameObject(this, ObjectHandler.GameObjectLayer.BACKGROUND);
            Drawable = new Drawable(TextureName);

            if (_hasTreasure && _passable)
            {
                item = new ItemTreasure(this.Position);
            }
            else
            {
                item = new ItemEmpty();
            }
        }

        /// <summary>
        /// Returns the IItem at this position and removes the IItem from this position.
        /// </summary>
        /// <param name="_item">IItem: The item being picked up.</param>
        /// <returns>Returns true if there is an IItem in this Location.</returns>
        public bool PickupItem(out IItem _item) {
            _item = this.item.Pickup();
            this.item = new ItemEmpty();
            return (_item != null && _item.itemType != ItemType.EMPTY);
        }

        /// <summary>
        /// Returns the IItem at this position.
        /// </summary>
        /// <param name="_item">IItem: The item at this location.</param>
        /// <returns>Returns true if there is an IItem in this Location.</returns>
        public bool LookItem(out IItem _item) {
            _item = this.item;
            return typeof(ItemEmpty) != this.item.GetType();
        }

        /// <summary>
        /// Update this item in the game loop.
        /// </summary>
        public void Update()
        {

        }
    }
}
