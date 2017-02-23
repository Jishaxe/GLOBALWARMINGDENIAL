using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/* ISSUES
 * Getting collisions working
 * Solved by: switching to a hybrid collision technique
 * 
 * Getting stuck on a surface when sliding along it
 * Solved by: Replacing the ifs with else if because it was only checking to see if it was to the left
 * 
 * Player is able to dig right and left even if there is not a tile below
 * Solved by: Checking for a tile below
 * 
 * Player can sit directly in the middle of a block and not move down
 * Solved by: Increasing the intensity of the attraction to the tile when digging
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
        public World world;
        MouseState mouse;
        KeyboardState keyboard;
        public Vector2 camera = new Vector2(0, 0);

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
            player = new Player(this);
            player.texture = Content.Load<Texture2D>("drill2");
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
                Tile tile = world.GetTile(mouse.Position.ToVector2() - camera);
                if (tile != null) tile.type = TileType.EMPTY;
            }

            player.Update();
            player.CollideWithWorld(world);
            world.Update();

            float centerOfScreen = GraphicsDevice.Viewport.Height / 5;
            camera.Y += (centerOfScreen - camera.Y - player.position.Y) / 1f;
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
