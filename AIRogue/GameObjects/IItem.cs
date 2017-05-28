using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRogue.GameObjects
{
    public enum ItemType {
        TREASURE,
        EMPTY
    }

    public interface IItem
    {
        IItem Pickup();
        ItemType itemType { get; }
        string TextureName { get; }
    }
}
