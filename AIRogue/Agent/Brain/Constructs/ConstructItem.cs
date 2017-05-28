using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIRogue.Engine;
using AIRogue.GameObjects;

namespace AIRogue.Agent.Brain.Constructs
{
    public class ConstructItem
    {
        public ItemType itemType;
        public Drawable Drawable { get; set; }

        public ConstructItem(ItemType _item, string textureName) {
            Drawable = new Drawable(textureName);
            itemType = _item;
        }
    }
}
