using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CIS580_final_project.world
{
    public class BufferRail : Rail
    {
        public static Texture2D RailTexture;

        public Payload TargetPayload { get; set; }

        public BufferRail(Direction orientation, bool deletable, Payload targetPayload) : base(orientation, deletable)
        {
            TargetPayload = targetPayload;
        }

        public override Tuple<Direction, Direction>[] GetPaths()
        {
            return new Tuple<Direction, Direction>[] { new Tuple<Direction, Direction>(Orientation, Orientation), new Tuple<Direction, Direction>(Orientation.Reverse(), Orientation.Reverse()) };
        }

        public override float GetRailLength()
        {
            return 1;
        }

        public override Texture2D GetRailTexture()
        {
            return RailTexture;
        }

        public override void Draw(GameState gameState, float x, float y, Color color)
        {
            base.Draw(gameState, x, y, color);
            if (TargetPayload != null)
            {
                gameState.SpriteBatch.Draw(TargetPayload.Texture, new Vector2(x + World.TileSize, y), TargetPayload.Color);
            }
        }

        public override void UpdateVehicle(GameState gameState, Vehicle v)
        {
            base.UpdateVehicle(gameState, v);
            if (v.EnterDirection == Orientation && v.Position > 0.2f)
            {
                v.Velocity = 0;
                v.Position = 0.2f;
            } else if (v.EnterDirection == Orientation.Reverse() && v.Position < 0.1f)
            {
                v.Velocity = 0;
                v.Position = 0.1f;
            }
        }

        public override Vector2 GetVehiclePosition(Direction d, float t)
        {
            if (CanEnterFrom(d))
            {
                switch (d)
                {
                    case Direction.North:
                        return new Vector2(World.TileSize / 2, (1 - t) * World.TileSize);
                    case Direction.East:
                        return new Vector2(t * World.TileSize, World.TileSize / 2);
                    case Direction.South:
                        return new Vector2(World.TileSize / 2, t * World.TileSize);
                    case Direction.West:
                        return new Vector2((1 - t) * World.TileSize, World.TileSize / 2);
                }
            }
            throw new ArgumentOutOfRangeException();
        }

        public override float GetVehicleRotation(Direction d, float t)
        {
            if (CanEnterFrom(d))
            {
                return d.GetAngle();
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}
