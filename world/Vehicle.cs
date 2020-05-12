using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS580_final_project.world
{
    public abstract class Vehicle
    {

        public float Position { get; set; }

        public float Velocity { get; set; }

        public Direction EnterDirection { get; set; }

        public Vehicle(float position, float velocity, Direction enterDirection)
        {
            Position = position;
            Velocity = velocity;
            EnterDirection = enterDirection;
        }

        public abstract void Draw(GameState gameState, float x, float y, Rail rail);

    }
}
