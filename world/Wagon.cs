using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS580_final_project.world
{
    public class Wagon : Vehicle
    {

        public static Texture2D WagonTexture;

        public Payload Payload { get; set; }

        public Wagon(float position, float velocity, Direction enterDirection, Payload payload) : base(position, velocity, enterDirection)
        {
            Payload = payload;
        }

        public override void Draw(GameState gameState, float x, float y, Rail rail)
        {
            SpriteBatch sb = gameState.SpriteBatch;

            Vector2 offset = rail.GetVehiclePosition(EnterDirection, Position);
            float rotation = rail.GetVehicleRotation(EnterDirection, Position);
            x += offset.X;
            y += offset.Y;

            //sb.Begin(transformMatrix: gameState.CameraMatrix);
            sb.Draw(WagonTexture, new Vector2(x, y), null, Color.White, rotation, new Vector2(World.TileSize / 2, World.TileSize / 2), 1, SpriteEffects.None, 0.2f);
            if (Payload != null)
            {
                sb.Draw(Payload.Texture, new Vector2(x, y), null, Payload.Color, rotation, new Vector2(World.TileSize / 2, World.TileSize / 2), 1, SpriteEffects.None, 0.3f);
            }
            //sb.End();
        }
    }
}
