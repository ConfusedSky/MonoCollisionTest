using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoCollisionTest
{
    public class RectCollisionSurface
    {
        private Rectangle collider;
        private Texture2D texture;
        private Color color;
        private bool colliderOff;
        private bool climbable;
        private bool highGravCollide;

        public RectCollisionSurface(int x, int y, int w, int h, Color color, bool climbable = false, bool highGravCollide = true)
        {
            Rectangle collider = new Rectangle(x, y, w, h);
            Initialize(collider, color, climbable);
        }

        public RectCollisionSurface(Rectangle collider, Color color, bool climbable = false, bool highGravCollide = true)
        {
            Initialize(collider, color, climbable, highGravCollide);;
        }

        private void Initialize(Rectangle collider, Color color, bool climbable = false, bool highGravCollide = true)
        {
            this.collider = collider;
            this.texture = Global.CreateTexture(Global.Graphics, collider.Width, collider.Height, pixel => color);
            this.color = color;
            this.colliderOff = false;
            this.climbable = climbable;
            this.highGravCollide = highGravCollide;
        }

        public double Y 
        { 
            get
            {
                if(climbable)
                {
                    return (Double)Global.MapHeight;
                }
                return (Double)collider.Y;
            }
        }
        public void Render(SpriteBatch s)
        {
            Vector2 position = new Vector2(collider.X, collider.Y);
            s.Draw(texture, position, color);
        }

        public bool IsCollided(Rectangle r)
        {
            int leftA, leftB;
            int rightA, rightB;
            int topA, topB;
            int bottomA, bottomB;

            leftA = collider.X;
            rightA = collider.X + collider.Width;
            topA = collider.Y;
            bottomA = collider.Y + collider.Height;

            leftB = r.X;
            rightB = r.X + r.Width;
            topB = r.Y;
            bottomB = r.Y + r.Height;

            if(bottomA <= topB)
            {
                return false;
            }

            if(topA >= bottomB)
            {
                return false;
            }

            if(rightA <= leftB)
            {
                return false;
            }

            if(leftA <= rightB)
            {
                return false;
            }

            return !colliderOff;
        }
    }
}