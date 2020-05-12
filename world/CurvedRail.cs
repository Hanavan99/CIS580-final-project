using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CIS580_final_project.world
{
    public class CurvedRail : Rail
    {
        public static Texture2D RailTexture;

        public CurvedRail(Direction orientation, bool deletable) : base(orientation, deletable)
        {
        }

        public override Tuple<Direction, Direction>[] GetPaths()
        {
            return new Tuple<Direction, Direction>[] { new Tuple<Direction, Direction>(Orientation, Orientation.Right()), new Tuple<Direction, Direction>(Orientation.Left(), Orientation.Reverse()) };
        }

        public override float GetRailLength()
        {
            return (float) Math.PI / 2f;
        }

        public override Texture2D GetRailTexture()
        {
            return RailTexture;
        }

        public override Vector2 GetVehiclePosition(Direction d, float t)
        {
            if (d == GetPaths()[0].Item1)
            {
                switch (d)
                {
                    case Direction.North:
                        return new Vector2((float)(2 - Math.Cos(t)) * World.TileSize * 0.5f, (float)(2 - Math.Sin(t)) * World.TileSize * 0.5f);
                    case Direction.East:
                        return new Vector2((float)Math.Sin(t) * World.TileSize * 0.5f, (float)(2 - Math.Cos(t)) * World.TileSize * 0.5f);
                    case Direction.South:
                        return new Vector2((float)Math.Cos(t) * World.TileSize * 0.5f, (float)Math.Sin(t) * World.TileSize * 0.5f);
                    case Direction.West:
                        return new Vector2((float)(2 - Math.Sin(t)) * World.TileSize * 0.5f, (float)Math.Cos(t) * World.TileSize * 0.5f);
                }
            } else if (d == GetPaths()[1].Item1)
            {
                switch (d)
                {
                    case Direction.North:
                        return new Vector2((float)Math.Cos(t) * World.TileSize * 0.5f, (float)(2 - Math.Sin(t)) * World.TileSize * 0.5f);
                    case Direction.East:
                        return new Vector2((float)Math.Sin(t) * World.TileSize * 0.5f, (float)Math.Cos(t) * World.TileSize * 0.5f);
                    case Direction.South:
                        return new Vector2((float)(2 - Math.Cos(t)) * World.TileSize * 0.5f, (float)Math.Sin(t) * World.TileSize * 0.5f);
                    case Direction.West:
                        return new Vector2((float)(2 - Math.Sin(t)) * World.TileSize * 0.5f, (float)(2 - Math.Cos(t)) * World.TileSize * 0.5f);
                }
            }
            throw new ArgumentOutOfRangeException();
        }

        public override float GetVehicleRotation(Direction d, float t)
        {
            if (d == GetPaths()[0].Item1)
            {
                return t + d.GetAngle();
            } else if (d == GetPaths()[1].Item1)
            {
                return -t + d.GetAngle();
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}
