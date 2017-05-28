using Microsoft.Xna.Framework;

using AIRogue.Engine;

namespace AIRogue.GameObjects
{
    public interface IGameObject
    {
        string TextureName { get; }
        void Update();
        Vector2 Position { get; }
    }
}
