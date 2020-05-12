using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS580_final_project.world
{
    public abstract class Rail
    {

        public List<Vehicle> Vehicles { get; set; }

        public Direction Orientation { get; set; }

        public bool Deletable { get; set; }

        public Rail(Direction orientation, bool deletable)
        {
            Orientation = orientation;
            Deletable = deletable;
            Vehicles = new List<Vehicle>();
        }

        public abstract Tuple<Direction, Direction>[] GetPaths();

        public abstract Texture2D GetRailTexture();

        public abstract Vector2 GetVehiclePosition(Direction d, float t);

        public abstract float GetVehicleRotation(Direction d, float t);

        public abstract float GetRailLength();

        public Direction GetEndDirection(Direction start)
        {
            Tuple<Direction, Direction>[] paths = GetPaths();
            foreach (var path in paths)
            {
                if (path.Item1 == start)
                {
                    return path.Item2;
                }
            }
            throw new ArgumentOutOfRangeException();
        }

        public bool CanEnterFrom(Direction d)
        {
            Tuple<Direction, Direction>[] paths = GetPaths();
            foreach (var path in paths)
            {
                if (path.Item1 == d)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void UpdateVehicle(GameState gameState, Vehicle v)
        {
            v.Position += v.Velocity * (float) gameState.GameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(GameState gameState, float x, float y, Color color)
        {
            SpriteBatch sb = gameState.SpriteBatch;

            // draw rail
            sb.Draw(GetRailTexture(), new Vector2(x + World.TileSize / 2, y + World.TileSize / 2), null, color, Orientation.GetAngle(), new Vector2(World.TileSize / 2, World.TileSize / 2), 1, SpriteEffects.None, 0.1f);

            // draw vehicles
            foreach (Vehicle v in Vehicles)
            {
                v.Draw(gameState, x, y, this);
            }
        }

    }
}
