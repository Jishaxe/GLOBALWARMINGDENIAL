using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GLOBALWARMINGDENIAL
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GlobalWarmingDenial : Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        World world;

        public GlobalWarmingDenial()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            world = new World(this);
            world.dirt = Content.Load<Texture2D>("dirt");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            player = new Player();
            player.texture = Content.Load<Texture2D>("player");

            world.Build();
        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // If mouse is clicked, dig out the specified tile
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                Tile tile = world.GetTile(mouse.Position.ToVector2());
                if (tile != null) tile.IsDug = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            player.Draw(spriteBatch);
            world.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
