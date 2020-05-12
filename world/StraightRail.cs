using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CIS580_final_project.world
{
    public class StraightRail : Rail
    {
        public static Texture2D RailTexture;

        public StraightRail(Direction orientation, bool deletable) : base(orientation, deletable)
        {
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
