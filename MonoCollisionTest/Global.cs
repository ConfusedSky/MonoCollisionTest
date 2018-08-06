using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoCollisionTest
{
    public static class Global
    {
        public static readonly int BorderWidth = 10;
        public static readonly int MapWidth = 1280;
        public static readonly int MapHeight = 720;
        public static readonly int Gravity = 1200;
        public static readonly Camera Camera = new Camera();
        public static GraphicsDevice Graphics;

        public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
        {
            //initialize a texture
            Texture2D texture = new Texture2D(device, width, height);

            //the array holds the color for each pixel in the texture
            Color[] data = new Color[width * height];
            for (int pixel = 0; pixel < data.Count(); pixel++)
            {
                //the function applies the color according to the specified pixel
                data[pixel] = paint(pixel);
            }

            //set the color
            texture.SetData(data);

            return texture;
        }
    }
}
