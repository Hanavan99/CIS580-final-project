using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CIS580_final_project.world
{
    public class GrassTile : WorldTile
    {

        public static Texture2D GrassTexture;

        public GrassTile(Color color) : base(color)
        {
        }

        public override Texture2D GetTexture()
        {
            return GrassTexture;
        }
    }
}
