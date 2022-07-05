using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PicrossExplores.SpriteHelpers;
using System.Collections.Generic;

namespace PicrossExplorers
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<BasicCell> cells = null;
        BoardHelper hlp = new BoardHelper();

        int mouseX = 0;
        int mouseY = 0;
        int cellIndex = 0;
        int cellIndexX = 0;
        int cellIndexY = 0;


        SpriteFont font;
        float minutes = 0;

        public Game1()
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
            cells = SpriteBuilder.BuildCells(Content);
            // TODO: use this.Content to load your game content here

            font = Content.Load<SpriteFont>("font");
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
            MouseState state = Mouse.GetState();
            mouseX = state.X;
            mouseY = state.Y;

            cellIndexX = ((mouseX - 50) / 32);
            cellIndexY = ((mouseY - 50) / 32);
            cellIndex = cellIndexY * 10 + cellIndexX;

            if((mouseX>=50) && (mouseX<=352) &&(mouseY>=50) &&(mouseY<=352))
            {
                if ((cellIndex < 100) && (cellIndex >= 0)) // sanity check make sure we are over a cell
                {
                    cells[cellIndex].CellState = BasicCell.State.HoveredOver;

                    if (state.LeftButton == ButtonState.Pressed)
                    {
                        cells[cellIndex].CellState = BasicCell.State.ClickedOn;
                    }
                }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

       
            minutes += (float)gameTime.ElapsedGameTime.TotalMinutes;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.IsMouseVisible = true;
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            hlp.DrawBoard(spriteBatch, cells);


            spriteBatch.DrawString(font, "X:" + mouseX.ToString(), new Vector2(300, 20), Color.White);
            spriteBatch.DrawString(font, "Y:" + mouseY.ToString(), new Vector2(350, 20), Color.White);

            spriteBatch.DrawString(font, "CX:" + cellIndexX.ToString(), new Vector2(500, 20), Color.White);
            spriteBatch.DrawString(font, "CY:" + cellIndexY.ToString(), new Vector2(550, 20), Color.White);
            spriteBatch.DrawString(font, "Index:" + cellIndex.ToString(), new Vector2(600, 20), Color.White);

            spriteBatch.DrawString(font, "Time:" + minutes.ToString("0.00"), new Vector2(200, 20), Color.White);

            spriteBatch.End();


            base.Draw(gameTime);

        }
    }
}
