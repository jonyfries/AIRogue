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
    public class ItemTreasure : IItem, IGameObject
    {
        public Vector2 Position { get; private set; }
        public ItemType itemType { get { return ItemType.TREASURE; } }
        public string TextureName { get; private set; }

        public ItemTreasure(Vector2 _position) {
            this.Position = _position;
            TextureName = "treasure";
            ObjectHandler.AddGameObject(this, ObjectHandler.GameObjectLayer.ITEM);
        }
        
        public IItem Pickup() {
            ObjectHandler.RemoveGameObject(this, ObjectHandler.GameObjectLayer.ITEM);
            return this;
        }

        public void Update() {}
    }
}
