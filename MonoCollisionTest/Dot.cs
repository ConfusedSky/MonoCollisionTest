﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

namespace MonoCollisionTest
{
    public class Dot
    {
        public static Texture2D createCircleText(GraphicsDevice graphics, int diameter, Color color)
        {
            Texture2D texture = new Texture2D(graphics, diameter, diameter);
            Color[] colorData = new Color[diameter*diameter];
        
            float diam = diameter / 2f;
            float diamsq = diam * diam;
        
            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {
                    int index = x * diameter + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = color;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }
        
            texture.SetData(colorData);
            return texture;
        }

        public const int DEFAULT_JUMP_HEIGHT = 450;
        public const float DEFAULT_DOUBLE_JUMP_MULTIPLIER = .7667f;
        public const float DEFAULT_POWER_JUMP_MULTIPLIER = 2.6f;
        public const float TO_POWER_JUMP = 0;

        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;

        private int diameter;
        private Color color;
        private float speed;
        private float jumpHeight;
        private float doubleJumpHeight;
        private float powerJumpHeight;
        private bool grounded;
        private bool hasDoubleJump;
        private float powerJump;
        private IList<RectCollisionSurface> platforms;

        public Vector2 Position { get {return position;} }

        public Dot(CollisionTestGame game, int diameter, Color color)
        {
            texture = createCircleText(Global.Graphics, diameter, color);
            position = new Vector2();
            velocity = new Vector2();
            this.diameter = diameter;
            this.color = color;
            this.speed = 200;
            this.jumpHeight = DEFAULT_JUMP_HEIGHT;
            this.doubleJumpHeight = DEFAULT_DOUBLE_JUMP_MULTIPLIER * this.jumpHeight;
            this.powerJumpHeight = DEFAULT_POWER_JUMP_MULTIPLIER * this.jumpHeight;
            this.grounded = false;
            this.hasDoubleJump = false;
            this.platforms = game.platforms;
            this.powerJump = TO_POWER_JUMP;

            prevState = Keyboard.GetState(); 
        }

        KeyboardState prevState;
        public void HandleInput(KeyboardState state)
        {
            if(state.IsKeyDown(Keys.A))
            {
                velocity.X = -speed; 
            }
            else if(state.IsKeyDown(Keys.D))
            {
                velocity.X = speed;
            }
            else
            {
                velocity.X = 0;
            }

            if(state.IsKeyDown(Keys.Space) && !prevState.IsKeyDown(Keys.Space))
            {
                if(grounded)
                { 
                    if((state.IsKeyDown(Keys.LeftControl) || 
                        state.IsKeyDown(Keys.RightControl)) &&
                        powerJump >= TO_POWER_JUMP )
                    {
                        velocity.Y = -powerJumpHeight;
                    }
                    else
                    {
                        velocity.Y = -jumpHeight;
                    }
                }
                else if(hasDoubleJump && velocity.Y >= -.5*doubleJumpHeight)
                {
                    velocity.Y = -doubleJumpHeight;
                    hasDoubleJump = false;
                }
            }

            prevState = state;
        }

        private void GlobalBoundsChecking()
        {
            // World Bounds Checking 
            if(position.X < Global.BorderWidth)
            {
                position.X = Global.BorderWidth;
                velocity.X = 0;
            }
            else if(position.X > Global.MapWidth - Global.BorderWidth - diameter)
            {
                position.X = Global.MapWidth - Global.BorderWidth - diameter;
                velocity.X = 0;
            }

            if(position.Y < Global.BorderWidth)
            {
                position.Y = Global.BorderWidth;
                velocity.Y = 0;
            }
            else if(position.Y > Global.MapHeight - Global.BorderWidth - diameter)
            {
                position.Y = Global.MapHeight - Global.BorderWidth - diameter;
                velocity.Y = 0;
                grounded = true;
                hasDoubleJump = true;
            }
        }

        public void Update(GameTime time)
        {
            Rectangle r = new Rectangle();
            r.Height = diameter;
            r.Width = diameter;

            float deltaTime = (float)time.ElapsedGameTime.TotalSeconds;

            if(powerJump < TO_POWER_JUMP)
            {
                powerJump += deltaTime;
            }

            // Do gravities
            velocity.Y += Global.Gravity * deltaTime;

            // Setting velocity
            position.X += velocity.X * deltaTime; 

            r.X = (int)position.X;
            r.Y = (int)position.Y;

            foreach(RectCollisionSurface rc in platforms)
            {
                if(rc.IsCollided(r))
                {
                    position.X -= velocity.X * deltaTime;
                }
            }

            position.Y += velocity.Y * deltaTime; 

            r.X = (int)position.X;
            r.Y = (int)position.Y;

            foreach(RectCollisionSurface rc in platforms)
            {
                if(rc.IsCollided(r))
                {
                    position.Y -= velocity.Y * deltaTime; 
                    velocity.Y = 0;
                    grounded = true;
                    if(r.Y <= rc.Y)
                    {
                        hasDoubleJump = true;
                    }
                    
                }
            }

            GlobalBoundsChecking();

            if(velocity.Y != 0)
            {
                grounded = false;
            }
        }        

        public void Render(SpriteBatch s)
        {
            s.Draw(texture, position, color);
        }

    }
}
