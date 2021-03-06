﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

/* ISSUES
 * Getting collisions working
 * Solved by: Switching to a hybrid collision technique
 * 
 * Getting stuck on a surface when sliding along it
 * Solved by: Replacing the ifs with else if because it was only checking to see if it was to the left
 * 
 * Player is able to dig right and left even if there is not a tile below
 * Solved by: Checking for a tile below
 * 
 * Player can sit directly in the middle of a block and not move down
 * Solved by: Increasing the intensity of the attraction to the tile when digging
 * 
 * Player sometimes phases through tiles when tiles have been removed around it
 * Solved by: Checking all tiles in world instead of just close ones
 * 
 * Movement is too jagged and hard to control
 * Solved by: Removing delays when digging blocks
 * 
 * Sparks look like wotsits
 * Solved by: Orientating them to face their direction of travel
 * 
 * When resetting the game, the background and walls would not be positioned correctly
 * Solved by: Resetting the wall positions
 */
namespace GLOBALWARMINGDENIAL
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GlobalWarmingDenial : Game
    {
        // The player stats
        public int hull = 100;
        public int depth = 0;
        public int money = 0;
        public bool dead = false;

        // When this is at 0, the player will self-heal
        public int healCooldown = 0;

        public SpriteFont courier;
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Random rnd = new Random();

        // The background, left and right walls
        InfiniteScroller background;
        InfiniteScroller leftWall;
        InfiniteScroller rightWall;

        // The scary fire wall
        public FireWall fire;
        public int fireCloseness;

        // The UI at the top-left of the screen
        public HUD hud;

        public Player player;

        // Current mouse and keyboard state
        MouseState mouse;
        KeyboardState keyboard;

        public World world;
        public Vector2 camera = new Vector2(0, 0);
        
        // The camera translation is the camera + the randomly generated offset
        public Vector2 cameraTranslation = new Vector2(0, 0);
        public Vector2 offset = new Vector2(0, 0);

        public Effects effects;
        public Sounds sounds;
        
        // This background is rendered when the player dies. deadBackgroundAlpha controls how transparent it is.
        public Texture2D deadBackground;
        private float deadBackgroundAlpha = 0f;
        public int absoluteFireDistance;

        public GlobalWarmingDenial()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 720;
            
            // Center the window on the screen
            this.Window.Position = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 - 640, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2 - 360);
            graphics.ApplyChanges();
            base.Initialize();
        }

        // Read money from a simple txt file in the CWD
        public int loadMoney()
        {
            if (File.Exists("money.txt"))
            {
                StreamReader inputFile = new StreamReader("money.txt"); 
                int money = Convert.ToInt32(inputFile.ReadLine());
                inputFile.Close();
                return money;
            }

            // If there is no money saved, just return 0
            return 0;
        }

        // Write the money to a simple txt file in the CWD
        public void saveMoney(int money)
        {
            StreamWriter outputFile = new StreamWriter("money.txt"); 
            outputFile.WriteLine(money);
            outputFile.Close();
        }

        // Reset and set the game back up again.
        public void Reset()
        {
            camera = new Vector2(0, 0);
            cameraTranslation = new Vector2(0, 0);
            depth = 0;
            background.Reset();
            leftWall.Reset();
            rightWall.Reset();
            fire.position.Y = -1000;
            deadBackgroundAlpha = 0;
            hull = 100;
            dead = false;
            player.position.Y = 0;
            player.velocity = new Vector2(0, 0);
            player.previousHitbox = player.GetHitbox();
            world = new World(this);
            money = loadMoney();
            world.Load(Content);
        }

        // Loads all the elements
        protected override void LoadContent()
        {
            effects = new Effects(this);

            deadBackground = Content.Load<Texture2D>("deadBackground");
            courier = Content.Load<SpriteFont>("Courier");

            hud = new HUD(this);
            hud.tv = Content.Load<Texture2D>("hud");
            hud.bar = Content.Load<Texture2D>("bar");

            fire = new FireWall(this);
            fire.texture = Content.Load<Texture2D>("fire");

            background = new InfiniteScroller(this, 0, 2, 720);
            background.texture = Content.Load<Texture2D>("background");

            Texture2D wall = Content.Load<Texture2D>("wall");

            leftWall = new InfiniteScroller(this, -50, 3, 720);
            rightWall = new InfiniteScroller(this, GraphicsDevice.Viewport.Width - wall.Width + 50, 3, 720);
            leftWall.texture = wall;
            rightWall.texture = wall;

            world = new World(this);
            world.Load(Content);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            player = new Player(this);
            player.texture = Content.Load<Texture2D>("drill2");
            player.animations = new AnimationManager();
            player.animations.Load("Drill_Idle", Content, 55, 60);
            player.animations.Load("Drill_Dig", Content, 61, 78);
            player.animations.Play("Drill_Idle");

            background.texture = Content.Load<Texture2D>("background");

            sounds = new Sounds();
            sounds.Load(Content);

            Reset();
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

            player.HandleInput(mouse, keyboard);

            // Reset the game if we are dead and you hit space
            if (keyboard.IsKeyDown(Keys.Space) && dead) Reset();


            // These only happen if we're still alive
            if (!dead)
            {
                player.Update();
                fire.Update();

                // Set the depth
                depth = (int)(player.position.Y / 100);
                player.CollideWithWorld(world);

                if (healCooldown > 0) healCooldown--;
                // Heal the player if the cooldown is at 0
                if (healCooldown == 0 && hull < 100) hull++;
            } else
            {
                player.position.Y += 20;
            }

            world.Update();


            // Make it so the fire can't get too far away
            absoluteFireDistance = (int)(fire.position - player.position).Length();
            if (absoluteFireDistance > 2000)
            {
                fire.position.Y = player.position.Y - 1800;
            }

            // Move camera to center player
            float centerOfScreen = GraphicsDevice.Viewport.Height / 5;
            camera.Y += (centerOfScreen - camera.Y - player.position.Y) / 10f;

            // Work out how much the camera shakes based on how far it is from the fire
            fireCloseness = 1000 - ((int)(fire.position - player.position).Length());


            if (fireCloseness < 0) fireCloseness = 0;

            // Make the fire sound louder depending on the distance of the fire
            sounds.AdjustFireSoundVolume(fireCloseness / 100f);

            // If we're close enough to the fire to shake, reduce the hull
            if (fireCloseness > 100)
            {
                hull--;
                healCooldown = 100; // Disable healing for 100 updates
            }

            // Use the shakefactor and a random number to throw some shake into the camera
            offset = new Vector2((float)rnd.NextDouble() * fireCloseness / 100, (float)rnd.NextDouble() * fireCloseness / 100);
            cameraTranslation = camera + offset;

            effects.Update();

            if (hull <= 0) Die();

            base.Update(gameTime);
        }

        // Called when the player dies
        public void Die ()
        {
            dead = true;
            saveMoney(money);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            background.Draw(spriteBatch);
            world.Draw(spriteBatch);
            if (!dead) player.Draw(spriteBatch);
            leftWall.Draw(spriteBatch);
            rightWall.Draw(spriteBatch);
            fire.Draw(spriteBatch);
            effects.Draw(spriteBatch);

            if (dead) {
                deadBackgroundAlpha += 0.01f;
                spriteBatch.Draw(deadBackground, new Vector2(0, 0), new Color(Color.White, deadBackgroundAlpha));
            }

            hud.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
