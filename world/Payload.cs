using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS580_final_project.world
{
    public class Payload
    {

        public Texture2D Texture { get; set; }

        public Color Color { get; set; }

        public Payload(Texture2D texture, Color color)
        {
            Texture = texture;
            Color = color;
        }

    }
}
