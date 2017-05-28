using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using AIRogue.Engine;

namespace AIRogue.GameObjects
{
    public class ItemEmpty : IItem
    {
        public ItemType itemType { get { return ItemType.EMPTY; } }

        public void Update() { }
        public Vector2 Position { get; set; }
        public string TextureName
        {
            get
            {
                return null;
            }
        }

        public IItem Pickup() {
            return null;
        }
    }
}
