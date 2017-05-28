using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using AIRogue.GameObjects;

namespace AIRogue.Engine
{
    public class Drawable
    {
        public String textureName;
        public static SpriteBatch spriteBatch;
        public Color filterColor;

        Texture2D Image
        {
            get
            {
                if (textureName != null)
                    return ObjectHandler.GetTexture(textureName);
                else
                    return null;
            }
        }

        /// <summary>
        /// Create a Drawable
        /// </summary>
        /// <param name="_image">The image to draw.</param>
        public Drawable(string _textureName) {
            this.textureName = _textureName;
            this.filterColor = Color.White;
        }

        /// <summary>
        /// Draw the image at position.
        /// </summary>
        /// <param name="position">A Vector2 offset for drawing the image.</param>
        public void Draw(Vector2 position) {
            Draw(position, filterColor);
        }

        /// <summary>
        /// Draw the image at parent.Position with offset and tinted _filterColor.
        /// </summary>
        /// <param name="offset">A Vector2 offset for drawing the image.</param>
        /// <param name="_filterColor">A color to tint the image with.</param>
        public void Draw(Vector2 position, Color _filterColor)
        {
            if ( textureName != null)
                spriteBatch.Draw(Image, position * 64, _filterColor);
        }
    }
}
