//#define FULLSCREEN

using CIS580_final_project.particle;
using CIS580_final_project.util;
using CIS580_final_project.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CIS580_final_project
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        // -------------------- GAME CONFIGURATION --------------------
        public static readonly Color PlaceColor = new Color(0, 0, 255);
        public static readonly Color[] PayloadColors = new Color[] { Color.Red, new Color(0, 255, 0), Color.Blue, Color.Yellow, Color.Magenta, Color.Cyan, Color.White, Color.Gray };
        public static readonly int WorldWidth = 10;
        public static readonly int WorldHeight = 10;
        public static readonly int VehicleCount = 3;
        public static readonly int CameraSpeed = 8;
        // -------------------- ------------------ --------------------

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private GameState gameState;
        private World world;
        private SpriteFont font, winfont;
        private Rail rail;
        private Vector2 railPos;
        private InputHelper input;
        private Random random;
        private bool isDeleting = false;
        private Texture2D payloadTexture;
        private ParticleSystem railParticles;
        private bool showHelp = true;

        private Vector2 mousePos;
        private Vector2 absMousePos;
        private Vector2 mouseTilePos;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
#if FULLSCREEN
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = true;
#else
            graphics.PreferredBackBufferWidth = 1800;
            graphics.PreferredBackBufferHeight = 900;
#endif
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            random = new Random();

            gameState = new GameState();
            gameState.CameraMatrix = Matrix.CreateTranslation(graphics.PreferredBackBufferWidth / 2 - WorldWidth / 2 * World.TileSize, graphics.PreferredBackBufferHeight / 2 - WorldHeight / 2 * World.TileSize, 0);

            input = new InputHelper();

            // create new straight rail
            input.SetKeyboardPressHandler(Keys.NumPad1, () => 
            {
                isDeleting = false;
                rail = new StraightRail(Direction.North, true);
            });

            // create new curved rail
            input.SetKeyboardPressHandler(Keys.NumPad2, () => 
            {
                isDeleting = false;
                rail = new CurvedRail(Direction.North, true);
            });

            // create new buffer rail
            //input.SetKeyboardPressHandler(Keys.NumPad3, () =>
            //{
            //    isDeleting = false;
            //    rail = new BufferRail(Direction.North, true, null);
            //});

            // create new cross rail
            input.SetKeyboardPressHandler(Keys.NumPad4, () =>
            {
                isDeleting = false;
                rail = new CrossRail(Direction.North, true);
            });

            // create escape handler
            input.SetKeyboardPressHandler(Keys.Escape, () =>
            {
                if (rail != null)
                {
                    rail = null;
                } else if (isDeleting)
                {
                    isDeleting = false;
                } else
                {
                    //Exit();
                }
            });

            // generate level handler
            input.SetKeyboardPressHandler(Keys.N, () =>
            {
                world = GenerateWorld(WorldWidth, WorldHeight, VehicleCount);
            });

            // rotate rail
            input.SetKeyboardPressHandler(Keys.R, () =>
            {
                if (rail != null)
                {
                    rail.Orientation = rail.Orientation.Right();
                }
            });

            // delete rail
            input.SetKeyboardPressHandler(Keys.Q, () =>
            {
                rail = null;
                isDeleting = true;
            });

            // left mouse button handler
            input.SetLeftButtonPressHandler(() =>
            {
                int mouseTileX = (int)mouseTilePos.X;
                int mouseTileY = (int)mouseTilePos.Y;
                if (rail != null)
                {
                    Rail oldRail = world.GetRail(mouseTileX, mouseTileY);
                    if (oldRail == null)
                    {
                        world.SetRail(mouseTileX, mouseTileY, rail);
                        railPos = mouseTilePos;
                        if (rail.GetType() == typeof(StraightRail))
                        {
                            rail = new StraightRail(rail.Orientation, true);
                        } 
                        else if (rail.GetType() == typeof(CurvedRail))
                        {
                            rail = new CurvedRail(rail.Orientation, true);
                        }
                        else
                        {
                            rail = null;
                        }
                        railParticles.SpawnPerFrame = 50;
                    }
                }
                else if (isDeleting)
                {
                    Rail deleteRail = world.GetRail(mouseTileX, mouseTileY);
                    if (deleteRail != null && deleteRail.Deletable) {
                        world.SetRail(mouseTileX, mouseTileY, null);
                    }
                }
            });

            // start level handler
            input.SetKeyboardPressHandler(Keys.Space, () =>
            {
                // remove all vehicles
                for (int x = 0; x < world.Width; x++)
                {
                    for (int y = 0; y < world.Height; y++)
                    {
                        Rail rail = world.GetRail(x, y);
                        if (rail != null)
                        {
                            rail.Vehicles.Clear();
                        }
                    }
                }

                // add vehicles
                foreach (KeyValuePair<Rail, Payload> pair in world.StartingRails)
                {
                    pair.Key.Vehicles.Add(new Wagon(0.8f, 3f, Direction.East, pair.Value));
                }
            });

            // help message handler
            input.SetKeyboardPressHandler(Keys.H, () =>
            {
                showHelp = !showHelp;
            });
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // set gamestate's spritebatch
            gameState.SpriteBatch = spriteBatch;

            // setup particle systems
            railParticles = new ParticleSystem(GraphicsDevice, 500, Content.Load<Texture2D>("particle_rail"));

            // load content
            font = Content.Load<SpriteFont>("default_font");
            winfont = Content.Load<SpriteFont>("win_font");
            GrassTile.GrassTexture = Content.Load<Texture2D>("grass");
            StraightRail.RailTexture = Content.Load<Texture2D>("rail");
            CurvedRail.RailTexture = Content.Load<Texture2D>("rail_curved");
            BufferRail.RailTexture = Content.Load<Texture2D>("rail_buffer");
            CrossRail.RailTexture = Content.Load<Texture2D>("rail_cross");
            Wagon.WagonTexture = Content.Load<Texture2D>("wagon");
            payloadTexture = Content.Load<Texture2D>("wagon_payload");
            railParticles.SpawnParticle = (ref Particle particle, Random r) =>
            {
                particle.velocity = RandomVector(random, (float) random.NextDouble() * 60f + 20f);
                particle.acceleration = -particle.velocity / 1.5f;
                particle.position = (railPos * World.TileSize) + particle.velocity / 2 + new Vector2(World.TileSize / 2, World.TileSize / 2);
                particle.color = Color.LightGoldenrodYellow;
                particle.scale = 1f;
                particle.life = (float) random.NextDouble() * 1f + 0.5f;
            };
            railParticles.UpdateParticle = ParticleSystem.DefaultParticleUpdater;

            // generate a level
            world = GenerateWorld(WorldWidth, WorldHeight, VehicleCount);
        }

        public Vector2 RandomVector(Random random, float scale)
        {
            Vector2 vec = new Vector2((float)random.NextDouble() * 2f - 1f, (float)random.NextDouble() * 2f - 1f);
            vec.Normalize();
            return vec * scale;
        }

        public World GenerateWorld(int width, int height, int numPaths)
        {
            World w = new World(width, height);

            // setup world tiles
            for (int x = 0; x < w.Width; x++)
            {
                for (int y = 0; y < w.Height; y++)
                {
                    float g = (float)random.NextDouble() * 0.05f + 0.5f;
                    WorldTile tile = new GrassTile(new Color(0, g, 0));
                    w.SetTile(x, y, tile);
                }
            }

            // generate track
            List<int> usedYVals = new List<int>();
            List<int> usedColorIndices = new List<int>();
            for (int path = 0; path < numPaths; path++)
            {
                int startX = 0;
                int startY = NextNotIn(random, usedYVals, 0, w.Height);
                int endX = w.Width - 1;
                int endY = NextNotIn(random, usedYVals, 0, w.Height);
                Rail start = new BufferRail(Direction.West, false, null);
                Payload payload = new Payload(payloadTexture, PayloadColors[NextNotIn(random, usedColorIndices, 0, PayloadColors.Length)]);
                start.Vehicles.Add(new Wagon(0.8f, 0, Direction.East, payload));
                w.SetRail(startX, startY, start);
                w.SetRail(endX, endY, new BufferRail(Direction.East, false, payload));
                w.StartingRails.Add(start, payload);
            }
            return w;
        }

        public static int NextNotIn(Random random, List<int> list, int min, int max)
        {
            int val;
            while (list.Contains(val = random.Next(min, max)))
            {
                // do nothing
            }
            list.Add(val);
            return val;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // update gamestate
            gameState.GameTime = gameTime;

            KeyboardState kb = Keyboard.GetState();
            input.Update();
            MouseState ms = Mouse.GetState();
            mousePos = new Vector2(ms.X, ms.Y);
            absMousePos = mousePos - new Vector2(gameState.CameraMatrix.Translation.X, gameState.CameraMatrix.Translation.Y);
            mouseTilePos = new Vector2((int)absMousePos.X / World.TileSize, (int)absMousePos.Y / World.TileSize);

            // we want these to update every frame        
            if (kb.IsKeyDown(Keys.A))
            {
                gameState.CameraMatrix = gameState.CameraMatrix.Translate(CameraSpeed, 0, 0);
            } else if (kb.IsKeyDown(Keys.D))
            {
                gameState.CameraMatrix = gameState.CameraMatrix.Translate(-CameraSpeed, 0, 0);
            }
            if (kb.IsKeyDown(Keys.W))
            {
                gameState.CameraMatrix = gameState.CameraMatrix.Translate(0, CameraSpeed, 0);
            } else if (kb.IsKeyDown(Keys.S))
            {
                gameState.CameraMatrix = gameState.CameraMatrix.Translate(0, -CameraSpeed, 0);
            }

            // TODO: Add your update logic here
            world.Update(gameState);
            railParticles.Update(gameTime);
            railParticles.SpawnPerFrame = 0;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            MouseState ms = Mouse.GetState();

            // update gamestate
            gameState.GameTime = gameTime;

            // draw world
            spriteBatch.Begin(transformMatrix: gameState.CameraMatrix, sortMode: SpriteSortMode.FrontToBack);
            world.Draw(gameState, isDeleting, mouseTilePos);
            spriteBatch.End();

            spriteBatch.Begin(transformMatrix: gameState.CameraMatrix);
            if (rail != null && world.GetRail((int) mouseTilePos.X, (int) mouseTilePos.Y) == null)
            {
                //spriteBatch.Draw(rail.GetRailTexture(), mouseTilePos * World.TileSize, null, Color.White, rail.Orientation.GetAngle(), new Vector2(32, 32), 1, SpriteEffects.None, 0);
                rail.Draw(gameState, mouseTilePos.X * World.TileSize, mouseTilePos.Y * World.TileSize, PlaceColor);
            }
            spriteBatch.End();

            railParticles.Draw(gameState.CameraMatrix);

            // draw UI stuff
            spriteBatch.Begin();
            //spriteBatch.DrawString(font, "Mouse pos: (" + mousePos.X + ", " + mousePos.Y + ")", new Vector2(20, 20), Color.White);
            //spriteBatch.DrawString(font, "Absolute mouse pos: (" + absMousePos.X + ", " + absMousePos.Y + ")", new Vector2(20, 40), Color.White);
            //spriteBatch.DrawString(font, "Mouse tile pos: (" + mouseTilePos.X + ", " + mouseTilePos.Y + ")", new Vector2(20, 60), Color.White);

            if (showHelp)
            spriteBatch.DrawString(font, "Welcome to RailPuzzle! The goal of the game is to link each wagon\non the left with its corresponding end position on the right.\n\nControls:\nPlace straight rail: Numpad 1\nPlace curved rail:   Numpad 2\nPlace buffer rail:   Numpad 3\nPlace cross rail:    Numpad 4\nRotate active rail:  R\nDelete mode:         Q\nDeselect rail:       Esc\nRun level:           Space\nGenerate new level:  N\nToggle this message: H", new Vector2(20, 20), Color.White);

            if (world.HasWon())
            {
                spriteBatch.DrawString(winfont, "You Win!", new Vector2(550, 350), Color.Red);
            }

            spriteBatch.End();

            

            base.Draw(gameTime);
        }

       
    }

    public static class Extensions
    {
        public static Matrix Translate(this Matrix m, float dx, float dy, float dz)
        {
            return Matrix.CreateTranslation(m.Translation.X + dx, m.Translation.Y + dy, m.Translation.Z + dz);
        }
    }
}
