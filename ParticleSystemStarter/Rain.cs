using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleSystemStarter
{
    class Rain : ParticleSystem
    {

        Random random = new Random();

        int screenWidth;

        public Rain(int screenWidth, GraphicsDevice device, int size, Texture2D texture) : base(device, size, texture)
        {
            this.screenWidth = screenWidth;

            // Set the SpawnParticle function
            SpawnParticle = Spawn;

            // Set the UpdateParticle function
            UpdateParticle = Update;
        }

        private void Spawn(ref Particle particle)
        {
            // spawn "drops" all across the top of the viewport with
            int x = random.Next(screenWidth);
            int y = 0;

            particle.Position = new Vector2(x, y);
            particle.Velocity = new Vector2(0, 25);
            //MathHelper.Lerp(0, 5, (float)random.NextDouble()));
            //particle.Acceleration = 0.2f * new Vector2(0, (float)random.NextDouble());
            particle.Acceleration = new Vector2(0,
                MathHelper.Lerp(2, 6, (float)random.NextDouble()));
            particle.Color = Color.CornflowerBlue;
            particle.Scale = 1f;
            particle.Life = 250.0f;


        }

        private void Update(float deltaT, ref Particle particle)
        {
            particle.Velocity += deltaT * particle.Acceleration;
            particle.Position += deltaT * particle.Velocity;
            //particle.Scale -= deltaT;
            particle.Life -= deltaT;
        }
    }
}
