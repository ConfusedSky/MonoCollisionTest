using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoCollisionTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class CollisionTestGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Dot player;
        Texture2D TopBorder;
        Texture2D BottomBorder;
        Texture2D LeftBorder;
        Texture2D RightBorder;

        public CollisionTestGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Global.Camera.ViewportWidth = graphics.GraphicsDevice.Viewport.Width;
            Global.Camera.ViewportHeight = graphics.GraphicsDevice.Viewport.Height;
            player = new Dot(this, 20, Color.Black);

            testSprite = Global.CreateTexture(GraphicsDevice, 50, 50, pixel => Color.Black);
            TopBorder = BottomBorder = Global.CreateTexture(GraphicsDevice, Global.MapWidth, Global.BorderWidth, pixel => Color.Black);
            LeftBorder = RightBorder = Global.CreateTexture(GraphicsDevice, Global.BorderWidth, Global.MapHeight, pixel => Color.Black);

            base.Initialize();
        }

        Texture2D testSprite;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //TODO: use this.Content to load your game content here 
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
#endif
            // TODO: Add your update logic here
            player.Update(gameTime);
            Global.Camera.CenterOn(player.Position);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(transformMatrix:Global.Camera.TranslationMatrix);
            spriteBatch.Draw(testSprite, new Vector2(), Color.Black);
            // Draw World Borders
            spriteBatch.Draw(TopBorder, new Vector2(0, 0), Color.Black);
            spriteBatch.Draw(BottomBorder, new Vector2(0, Global.MapHeight-Global.BorderWidth), Color.Black);
            spriteBatch.Draw(LeftBorder, new Vector2(0, 0), Color.Black);
            spriteBatch.Draw(RightBorder, new Vector2(Global.MapWidth-Global.BorderWidth, 0), Color.Black);
            player.Render(spriteBatch);
            spriteBatch.End();

            //TODO: Add your drawing code here
            base.Draw(gameTime);
        }
    }
}
