using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleSystemStarter
{
    class ParticleSystem
    {
        public Vector2 Emitter { get; set; }

        protected Particle[] particles;
        protected Texture2D texture;

        protected SpriteBatch spriteBatch;

        Random random = new Random();

        int nextIndex = 0;
        
        public int SpawnPerFrame { get; set; }

        public delegate void ParticleSpawner(ref Particle particle);
        public delegate void ParticleUpdater(float deltaT, ref Particle particle);

        public ParticleSpawner SpawnParticle { get; set; }
        public ParticleUpdater UpdateParticle { get; set; }

        public ParticleSystem(GraphicsDevice graphicsDevice, int size, Texture2D texture)
        {
            this.particles = new Particle[size];
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.texture = texture;
            this.SpawnPerFrame = 4;
        }

        public void Update(GameTime gameTime)
        {

            if (SpawnParticle == null || UpdateParticle == null) return;

            // Spawn Particles
            for (int i = 0; i < SpawnPerFrame; i++)
            {
                // Spawn a particle at nextIndex
                /*[nextIndex].Position = Emitter;
                particles[nextIndex].Velocity = 100 * new Vector2((float)random.NextDouble(), (float)random.NextDouble());
                particles[nextIndex].Acceleration = 0.1f * new Vector2((float)random.NextDouble(), (float)random.NextDouble());
                particles[nextIndex].Color = Color.White;
                particles[nextIndex].Scale = 1f;
                particles[nextIndex].Life = 3.0f;*/
                SpawnParticle(ref particles[nextIndex]);

                // Advance the index
                nextIndex++;
                if (nextIndex > particles.Length - 1) nextIndex = 0;
            }

            // Update Particles
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i = 0; i < particles.Length; i++)
            {
                // Skip "dead" particles
                if (particles[i].Life <= 0) continue;

                // Update individual particle
                /*particles[i].Velocity += deltaT * particles[i].Acceleration;
                particles[i].Position += deltaT * particles[i].Velocity;
                particles[i].Life -= deltaT;*/
                UpdateParticle(deltaT, ref particles[i]);
            }

        }

        public void Draw()
        {
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            // Draw the particles
            for (int i = 0; i < particles.Length; i++)
            {
                // Skip dead particles
                if (particles[i].Life <= 0) continue;

                // Draw a particle
                spriteBatch.Draw(texture, particles[i].Position, null, particles[i].Color, 0f, Vector2.Zero, particles[i].Scale, SpriteEffects.None, 0);
            }

            spriteBatch.End();

        }

    }
}
