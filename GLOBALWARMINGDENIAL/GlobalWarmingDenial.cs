using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/* ISSUES
 * Getting collisions working
 * Solved by: switching to a different collision technique and experimenting
 */
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
        MouseState mouse;
        KeyboardState keyboard;

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
            player.texture = Content.Load<Texture2D>("drill");

            world.Build();
        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update the current state of the mouse and keyboard
            mouse = Mouse.GetState();
            keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Space)) player.position = new Vector2(100, 20);
            player.HandleInput(mouse, keyboard);

            // If mouse is clicked, dig out the specified tile

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                Tile tile = world.GetTile(mouse.Position.ToVector2());
                if (tile != null) tile.IsDug = true;
            }

            player.Update();
            player.CollideWithWorld(world);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            world.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
