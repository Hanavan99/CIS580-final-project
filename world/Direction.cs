using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS580_final_project.world
{
    public enum Direction
    {

        North,
        East,
        South,
        West

    }

    public static class DirectionFunctionality
    {

        public static Direction Left(this Direction direction)
        {
            return (Direction) (((int) direction + 3) % 4);
        }

        public static Direction Right(this Direction direction)
        {
            return (Direction)(((int)direction + 1) % 4);
        }

        public static Direction Reverse(this Direction direction)
        {
            return (Direction)(((int)direction + 2) % 4);
        }

        public static float GetAngle(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return 0;
                case Direction.East:
                    return (float) Math.PI / 2f;
                case Direction.South:
                    return (float) Math.PI;
                case Direction.West:
                    return 3 * (float) Math.PI / 2f;
            }
            throw new ArgumentOutOfRangeException();
        }

    }

}
