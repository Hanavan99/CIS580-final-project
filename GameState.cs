using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS580_final_project
{
    public class GameState
    {

        public GameTime GameTime { get; set; }

        public Matrix CameraMatrix { get; set; }

        public SpriteBatch SpriteBatch { get; set; }

    }
}
