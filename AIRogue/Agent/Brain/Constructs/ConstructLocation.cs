using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using AIRogue.GameObjects;
using AIRogue.Engine;

namespace AIRogue.Agent.Brain.Constructs
{
    public class ConstructLocation : IComparable<ConstructLocation>
    {
        public bool IsWalkable { get; set; }
        public ConstructItem Item { get; set; }
        public ConstructScore Score { get; private set; }
        public Drawable Drawable { get; set; }
        
        public ConstructLocation(Location _location) {
            IsWalkable = _location.IsWalkable;
            Drawable = new Drawable(_location.TextureName);
            IItem locationItem;
            _location.LookItem(out locationItem);
            Item = new ConstructItem(locationItem.itemType, locationItem.TextureName);
            Score = new ConstructScore();
        }

        public ConstructLocation() {
            IsWalkable = false;
            Item = new ConstructItem(ItemType.EMPTY, null);
        }

        public int CompareTo(ConstructLocation other) {
            return Score.Score.CompareTo(other.Score.Score);
        }
    }
}
