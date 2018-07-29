using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoCollisionTest
{
    public class Dot
    {
        public static Texture2D createCircleText(Game game, int diameter, Color color)
        {
            Texture2D texture = new Texture2D(game.GraphicsDevice, diameter, diameter);
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

        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;

        private int diameter;
        private Color color;
        private float speed;
        private float jumpHeight;
        private float doubleJumpHeight;
        private bool grounded;
        private bool hasDoubleJump;

        public Vector2 Position { get {return position;} }

        public Dot(CollisionTestGame game, int diameter, Color color)
        {
            texture = createCircleText(game, diameter, color);
            position = new Vector2();
            velocity = new Vector2();
            this.diameter = diameter;
            this.color = color;
            this.speed = 200;
            this.jumpHeight = 450;
            this.doubleJumpHeight = .7667f * 450;
            this.grounded = false;
            this.hasDoubleJump = false;

            prevState = Keyboard.GetState(); 
        }

        KeyboardState prevState;
        private void HandleInput(KeyboardState state)
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
                    velocity.Y = -jumpHeight;
                }
                else if(hasDoubleJump && velocity.Y >= -.5*doubleJumpHeight)
                {
                    velocity.Y = -doubleJumpHeight;
                    hasDoubleJump = false;
                }
            }
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
            else
            {
                grounded = false;                
            }
        }

        public void Update(GameTime time)
        {
            float deltaTime = (float)time.ElapsedGameTime.TotalSeconds;

            // Get input
            KeyboardState state = Keyboard.GetState();
            HandleInput(state);

            // Do gravities
            velocity.Y += 1200 * deltaTime;

            // Setting velocity
            position += velocity * deltaTime; 

            GlobalBoundsChecking();

            prevState = state;
        }        

        public void Render(SpriteBatch s)
        {
            s.Draw(texture, position, color);
        }

    }
}
