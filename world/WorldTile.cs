using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS580_final_project.world
{

    public abstract class WorldTile
    {

        private Color color;

        public WorldTile(Color color)
        {
            this.color = color;
        }

        public abstract Texture2D GetTexture();

        public void Draw(GameState gameState, float x, float y)
        {
            gameState.SpriteBatch.Draw(GetTexture(), new Vector2(x, y), null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

    }
}
