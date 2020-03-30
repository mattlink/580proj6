using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ParticleSystemStarter
{
    class FireBall : ParticleSystem
    {

        enum FireBallState
        {
            Normal,
            Falling,
            Explode
        }

        FireBallState state = FireBallState.Normal;

        float fallSpeed = 0.1f;
        float fallAcceleration = .01f;

        int dropUntilY;

        float explosionLife = 20.0f;

        Random random = new Random();
        Color[] colors = new Color[] { Color.Red, Color.DarkRed, Color.Orange, Color.DarkOrange, Color.Yellow };
        Color[] explodeColors = new Color[] { Color.White, Color.Brown, Color.Chocolate };

        public FireBall(GraphicsDevice graphics, int size, Texture2D texture): base(graphics, size, texture)
        {

            // set the SpawnParticle method
            SpawnParticle = Spawn;

            // set the UpdateParticle method
            UpdateParticle = Update;
          
        }

        public void Drop(int dropUntil)
        {
            dropUntilY = dropUntil;
            if (state == FireBallState.Normal)
            {
                state = FireBallState.Falling;
            }
        }

        private void Spawn (ref Particle particle)
        {
            if (!(state == FireBallState.Explode))
            {
                
                particle.Position = Emitter;
                particle.Velocity = new Vector2(
                            MathHelper.Lerp(-5, 5, (float)random.NextDouble()),
                            MathHelper.Lerp(0, -100, (float)random.NextDouble()));
                particle.Acceleration = 0.1f * new Vector2(0, (float)-random.NextDouble());

                particle.Color = colors[random.Next(colors.Length)];
                particle.Scale = 2f;
                particle.Life = 1.0f;
            }

            if (state == FireBallState.Explode)
            {
                particle.Position = Emitter;/* new Vector2(
                    Emitter.X + MathHelper.Lerp(-3, 3, (float)random.NextDouble()),
                    Emitter.Y + MathHelper.Lerp(-3, 3, (float)random.NextDouble()));*/
                particle.Velocity = new Vector2(
                    MathHelper.Lerp(-20, 20, (float)random.NextDouble()),
                    MathHelper.Lerp(0, 20, (float)random.NextDouble()));
                particle.Acceleration = new Vector2(//22, 22);
                    MathHelper.Lerp(-1.0f, 1.0f, (float)random.NextDouble()),
                    MathHelper.Lerp(-1.85f, 1.85f, (float)random.NextDouble()));
                particle.Color = explodeColors[random.Next(explodeColors.Length)];
                particle.Scale = MathHelper.Lerp(.5f, .9f, (float)random.NextDouble());
            }
            
        }

        private void Update (float deltaT, ref Particle particle)
        {
            if (state == FireBallState.Normal)
            {
                MouseState mouse = Mouse.GetState();
                Emitter = new Vector2(mouse.X, mouse.Y);
               
            }

            if (state == FireBallState.Falling)
            {
                float y = Emitter.Y;
                y += deltaT * fallSpeed;
                fallSpeed += deltaT * fallAcceleration;
                Emitter = new Vector2(Emitter.X, y);

                // Check for collision with floor/bottom of screen.
                if (Emitter.Y >= dropUntilY)
                {
                    // Initialize explode state.
                    state = FireBallState.Explode;
                    explosionLife = 20.0f;
                }
                
            }

            
            particle.Velocity += deltaT * particle.Acceleration;
            particle.Position += deltaT * particle.Velocity;

            if (!(state == FireBallState.Explode))
            {
                particle.Scale -= deltaT;
                particle.Life -= deltaT;
            }
            
            

            if (state == FireBallState.Explode)
            {
                
                explosionLife -= deltaT;


                if (explosionLife <= 0.0f)
                {
                    state = FireBallState.Normal;
                    explosionLife = 4.0f;
                }
            }


            
        }

        public new void Draw()
        {
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
