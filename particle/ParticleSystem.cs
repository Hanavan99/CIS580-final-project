using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CIS580_final_project.particle
{
    
    public delegate void ParticleSpawner(ref Particle particle, Random r);

    public delegate void ParticleUpdater(ref Particle particle, float deltaT);

    public class ParticleSystem
    {
        public static readonly ParticleUpdater DefaultParticleUpdater = (ref Particle p, float deltaT) =>
        {
            p.velocity += deltaT * p.acceleration;
            p.position += deltaT * p.velocity;
            p.life -= deltaT;
        };

        private Particle[] particles;
        private Texture2D texture;
        private SpriteBatch spriteBatch;
        private Random random = new Random();
        private int nextIndex = 0;

        public ParticleSystem(GraphicsDevice graphicsDevice, int size, Texture2D texture)
        {
            particles = new Particle[size];
            spriteBatch = new SpriteBatch(graphicsDevice);
            this.texture = texture;
            BlendState = BlendState.Additive;
        }

        public BlendState BlendState { get; set; }

        public Vector2 Emitter { get; set; }

        public int SpawnPerFrame { get; set; }

        public ParticleSpawner SpawnParticle { get; set; }

        public ParticleUpdater UpdateParticle { get; set; }

        public void Update(GameTime gameTime)
        {
            // spawn particles
            for (int i = 0; i < SpawnPerFrame; i++)
            {
                // create particle
                SpawnParticle(ref particles[nextIndex], random);

                nextIndex++;
                if (nextIndex >= particles.Length)
                {
                    nextIndex = 0;
                }
            }

            // update particles
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].life <= 0) continue;

                UpdateParticle(ref particles[i], deltaT);
            }
        }

        public void Draw(Matrix camera)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState, null, null, null, null, camera);

            for (int i = 0; i < particles.Length; i++)
            {
                ref Particle p = ref particles[i];

                if (p.life <= 0) continue;

                spriteBatch.Draw(texture, p.position, null, p.color, 0f, Vector2.Zero, p.scale, SpriteEffects.None, 0);
            }

            spriteBatch.End();
        }

        public Vector2 RandomVector()
        {
            return new Vector2((float)random.NextDouble() * 2.0f - 1.0f, (float)random.NextDouble() * 2.0f - 1.0f);
        }

    }
}
