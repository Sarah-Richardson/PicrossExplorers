using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PicrossExplorers.Helpers;
using PicrossExplorers.SpriteHelpers;
using PicrossExplores.SpriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;



namespace PicrossExplorers
{
    /// <summary>
    /// Main Game
    /// </summary>
    public class MainGame : Game
    {

        #region private members

        private const bool DEBUG = true;

        private bool hasBeenCreated = false;

        private string imageFileName = "";

        private enum GameState
        {
            StartMenu,
            LevelMenu,
            MakeLevel,
            DrawLevel,
            PlayLevel,
            Exit
        }

        private enum MenuItemHover
        {
            chooseLevel,
            newLevel,
            drawLevel,
            exit
        }

        private MenuItemHover HoveredOverMenuItem;

        private int boardStartPositionX = 350;
        private int boardStartPositionY = 150;

        private int levelStartPositionX = 280;
        private int levelStartPositionY = 20;

        private int thumbnailsPerRow = 4;

        private bool hover = false;
        private bool hoverBack = false;
        private bool hoverSave = false;

        private GameState gameState;

        private MouseState mouseState;
        private MouseState previousMouseState;

        private Texture2D chooseLevel;
        private Texture2D newLevel;
        private Texture2D drawLevel;
        private Texture2D exit;
        private Texture2D logo;
        private Texture2D back;
        private Texture2D menuHoveredOver;
        private Texture2D questionMark;
        private Texture2D levelHoveredOver;
        private Texture2D win;
        private Texture2D pattern;

        private Texture2D gold;
        private Texture2D silver;
        private Texture2D bronze;

        private Level chosenLevel;

        private Vector2 chooseLevelPosition;
        private Vector2 newLevelPosition;
        private Vector2 drawLevelPosition;
        private Vector2 exitPosition;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D CellHoveredOver;

        private BoardHelper boardHlp = new BoardHelper();
        private ClueHelper clueHlp = new ClueHelper();
        private LevelManager levelManager = new LevelManager();

        private List<Level> levels = null;

        private int mouseX = 0;
        private int mouseY = 0;

        private int currentCellIndex = 0;
        private Rectangle boardRectangle = new Rectangle();

        private SpriteFont font;
        private float timeTakenToSolve = 0;

        private Texture2D save;
        private Texture2D colourTexture;
        private Texture2D blackAndWhiteTexture;        
        private List<BasicCell> previewBoardCells;

        private LevelDesigner designer = new LevelDesigner();
        FileHelper fileHlp = new FileHelper();

        private int delayCount = 300;

        #endregion

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            gameState = GameState.StartMenu;
            graphics.PreferredBackBufferWidth = 820;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 500;   // set this value to the desired height of your window
            graphics.ApplyChanges();
            graphics.IsFullScreen = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            chooseLevelPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 125, 160);
            newLevelPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 125, 240);
            drawLevelPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 125, 320);
            exitPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 125, 400);

            levels = levelManager.LoadAllLevels(graphics, Content);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {            
            spriteBatch = new SpriteBatch(GraphicsDevice);         
            CellHoveredOver = Content.Load<Texture2D>("CellHoveredOver");
            font = Content.Load<SpriteFont>("font");
            boardRectangle = new Rectangle(boardStartPositionX, boardStartPositionY, 300, 300);
            chooseLevel = Content.Load<Texture2D>("chooseLevel");
            newLevel = Content.Load<Texture2D>("newLevel");
            drawLevel = Content.Load<Texture2D>("drawLevel");
            exit = Content.Load<Texture2D>("exit");
            logo = Content.Load<Texture2D>("logo");
            menuHoveredOver = Content.Load<Texture2D>("menuHoveredOver");
            levelHoveredOver = Content.Load<Texture2D>("levelHoveredOver");
            win = Content.Load<Texture2D>("win");
            back = Content.Load<Texture2D>("back");
            save = Content.Load<Texture2D>("save");
            pattern = Content.Load<Texture2D>("pattern");
            questionMark = Content.Load<Texture2D>("levelButton");

            gold = Content.Load<Texture2D>("gold");
            silver = Content.Load<Texture2D>("silver");
            bronze = Content.Load<Texture2D>("bronze");

        }

        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            KeyboardState keyBoardState = Keyboard.GetState();

            if (gameState == GameState.StartMenu)
            {
                mouseState = Mouse.GetState();
                MouseHovered(mouseState.X, mouseState.Y);
                if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {
                    MouseClicked(mouseState.X, mouseState.Y);
                }
                previousMouseState = mouseState;
            }

            if (gameState == GameState.LevelMenu)
            {
                mouseState = Mouse.GetState();
                MouseHoveredOverLevel(mouseState.X, mouseState.Y);
                MouseHoveredOverClickedBack(mouseState.X, mouseState.Y);
                if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {
                    MouseClickedLevel(mouseState.X, mouseState.Y);
                }
                if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {
                    MouseClickedBack(mouseState.X, mouseState.Y);
                }
                previousMouseState = mouseState;
            }

            if ((gameState == GameState.PlayLevel) && (null!=chosenLevel))
            {
                mouseState = Mouse.GetState();
                mouseX = mouseState.X;
                mouseY = mouseState.Y;

                MouseHoveredOverClickedBack(mouseState.X, mouseState.Y);
                if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {
                    MouseClickedBack(mouseState.X, mouseState.Y);
                }

                MouseClickedCell(mouseState);

                previousMouseState = mouseState;

                if (!chosenLevel.SolvedInfo.HasBeenSolvedLock)
                {
                    chosenLevel.SolvedInfo.HasBeenSolved = chosenLevel.Solved();                    
                }
                if((chosenLevel.SolvedInfo.HasBeenSolved) && (!chosenLevel.SolvedInfo.HasBeenSolvedLock))
                {
                    chosenLevel.SolvedInfo.HasBeenSolvedLock = true;
                    for (int i = 0; i < 100; i++)
                    {
                        string co = chosenLevel.data[i];
                        if (co == "1")
                        {
                            chosenLevel.Cells[i].CellState = BasicCell.State.Foreground;
                        }
                        else
                        {
                            chosenLevel.Cells[i].CellState = BasicCell.State.Background;
                        }
                    }
                    chosenLevel.SolvedInfo.TimeTakenToSolve = timeTakenToSolve;                    
                    fileHlp.SaveSolvedInformation(chosenLevel.SolvedInfo, chosenLevel.FileName);
                }
                if(chosenLevel.SolvedInfo.HasBeenSolvedLock)
                {                    
                    delayCount--;
                    if(delayCount<0)
                    {
                        delayCount = 300;
                        gameState = GameState.LevelMenu;
                    }
                }
            }

            if (gameState == GameState.MakeLevel)
            {
                mouseState = Mouse.GetState();
                mouseX = mouseState.X;
                mouseY = mouseState.Y;
                MouseHoveredOverClickedBack(mouseState.X, mouseState.Y);
                MouseHoveredOverClickedSave(mouseState.X, mouseState.Y);
                if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {
                    MouseClickedBack(mouseState.X, mouseState.Y);
                }
                if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {
                    MouseClickedSave(mouseState.X, mouseState.Y);
                }
                if(null!=previewBoardCells)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        Rectangle rectangle = new Rectangle(previewBoardCells[i].X, previewBoardCells[i].Y, BasicCell.SPRITE_WIDTH, BasicCell.SPRITE_HEIGHT);
                        if (rectangle.Contains(mouseX, mouseY))
                        {
                            if (mouseState.LeftButton == ButtonState.Pressed)
                            {
                                if (previewBoardCells[i].MouseLeftButtonIsDown == false)
                                {
                                    previewBoardCells[i].MouseLeftButtonIsDown = true;
                                    if (previewBoardCells[i].CellState == BasicCell.State.Foreground)
                                    {
                                        previewBoardCells[i].CellState = BasicCell.State.Background;
                                    }
                                    else if (previewBoardCells[i].CellState == BasicCell.State.Background)
                                    {
                                        previewBoardCells[i].CellState = BasicCell.State.Foreground;
                                    }
                                }
                            }
                            if (mouseState.LeftButton == ButtonState.Released)
                            {
                                previewBoardCells[i].MouseLeftButtonIsDown = false;
                            }

                            if (mouseState.RightButton == ButtonState.Released)
                            {
                                previewBoardCells[i].MouseRightButtonIsDown = false;
                            }
                            currentCellIndex = i;
                        }
                        else
                        {
                            previewBoardCells[i].MouseLeftButtonIsDown = false;
                            previewBoardCells[i].MouseRightButtonIsDown = false;
                        }
                    }
                }

                previousMouseState = mouseState;

                if (!hasBeenCreated)
                {
                    imageFileName = fileHlp.GetImageFileName();
                    hasBeenCreated = designer.CreateLevelFromImage(imageFileName, ref colourTexture, ref blackAndWhiteTexture, ref previewBoardCells, graphics, Content);
                }

            }

            if (gameState == GameState.DrawLevel)
            {
                mouseState = Mouse.GetState();
                mouseX = mouseState.X;
                mouseY = mouseState.Y;
                MouseHoveredOverClickedBack(mouseState.X, mouseState.Y);
                MouseHoveredOverClickedSave(mouseState.X, mouseState.Y);
                if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {
                    MouseClickedBack(mouseState.X, mouseState.Y);
                }
                if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {
                    MouseClickedSave(mouseState.X, mouseState.Y);
                }

                if (null != previewBoardCells)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        Rectangle rectangle = new Rectangle(previewBoardCells[i].X, previewBoardCells[i].Y, BasicCell.SPRITE_WIDTH, BasicCell.SPRITE_HEIGHT);
                        if (rectangle.Contains(mouseX, mouseY))
                        {
                            if (mouseState.LeftButton == ButtonState.Pressed)
                            {
                                if (previewBoardCells[i].MouseLeftButtonIsDown == false)
                                {
                                    previewBoardCells[i].MouseLeftButtonIsDown = true;
                                    if (previewBoardCells[i].CellState == BasicCell.State.Foreground)
                                    {
                                        previewBoardCells[i].CellState = BasicCell.State.Background;
                                    }
                                    else if (previewBoardCells[i].CellState == BasicCell.State.Background)
                                    {
                                        previewBoardCells[i].CellState = BasicCell.State.Foreground;
                                    }
                                }
                            }
                            if (mouseState.LeftButton == ButtonState.Released)
                            {
                                previewBoardCells[i].MouseLeftButtonIsDown = false;
                            }

                            if (mouseState.RightButton == ButtonState.Released)
                            {
                                previewBoardCells[i].MouseRightButtonIsDown = false;
                            }
                            currentCellIndex = i;
                        }
                        else
                        {
                            previewBoardCells[i].MouseLeftButtonIsDown = false;
                            previewBoardCells[i].MouseRightButtonIsDown = false;
                        }
                    }
                }


                previousMouseState = mouseState;

                if (!hasBeenCreated)
                {
                    previewBoardCells = SpriteBuilder.BuildCells(Content);
                    for (int i = 0; i < 100; i++)
                    {
                        previewBoardCells[i].CellState = BasicCell.State.Foreground;
                    }
                    hasBeenCreated = true;
                }

                levelManager.KeyboardLoadLevel(keyBoardState, ref previewBoardCells);
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            timeTakenToSolve += (float)gameTime.ElapsedGameTime.TotalMinutes;

            base.Update(gameTime);
        }

        private void MouseClickedCell(MouseState mouseState)
        {
            for (int i = 0; i < 100; i++)
            {
                Rectangle rectangle = new Rectangle(chosenLevel.Cells[i].X, chosenLevel.Cells[i].Y, BasicCell.SPRITE_WIDTH, BasicCell.SPRITE_HEIGHT);
                if (rectangle.Contains(mouseX, mouseY))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (chosenLevel.Cells[i].MouseLeftButtonIsDown == false)
                        {
                            chosenLevel.Cells[i].MouseLeftButtonIsDown = true;
                            if (chosenLevel.Cells[i].CellState == BasicCell.State.Normal)
                            {
                                chosenLevel.Cells[i].CellState = BasicCell.State.ClickedOn;
                            }
                            else if (chosenLevel.Cells[i].CellState == BasicCell.State.ClickedOn)
                            {
                                chosenLevel.Cells[i].CellState = BasicCell.State.Normal;
                            }
                        }
                    }
                    if (mouseState.LeftButton == ButtonState.Released)
                    {
                        chosenLevel.Cells[i].MouseLeftButtonIsDown = false;
                    }

                    if (mouseState.RightButton == ButtonState.Pressed)
                    {
                        if (chosenLevel.Cells[i].MouseRightButtonIsDown == false)
                        {
                            chosenLevel.Cells[i].MouseRightButtonIsDown = true;
                            if (chosenLevel.Cells[i].CellState == BasicCell.State.Normal)
                            {
                                chosenLevel.Cells[i].CellState = BasicCell.State.Cross;
                            }
                            else if (chosenLevel.Cells[i].CellState == BasicCell.State.Cross)
                            {
                                chosenLevel.Cells[i].CellState = BasicCell.State.Normal;
                            }
                        }
                    }

                    if (mouseState.RightButton == ButtonState.Released)
                    {
                        chosenLevel.Cells[i].MouseRightButtonIsDown = false;
                    }
                    currentCellIndex = i;
                }
                else
                {
                    chosenLevel.Cells[i].MouseLeftButtonIsDown = false;
                    chosenLevel.Cells[i].MouseRightButtonIsDown = false;
                }
            }
        }

        private void MouseClickedLevel(int x, int y)
        {
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);
            if (gameState == GameState.LevelMenu)
            {
                int countOfThumbnails = 1;
                int thumbnailPositionX = 0;
                int thumbnailPositionY = 50;

                foreach (Level level in levels)
                {
                    Rectangle levelRect = new Rectangle(levelStartPositionX + thumbnailPositionX, levelStartPositionY + thumbnailPositionY, 125, 125);

                    if (mouseClickRect.Intersects(levelRect))
                    {
                        chosenLevel = level;
                        chosenLevel.SolvedInfo.HasBeenSolved = false;
                        timeTakenToSolve = 0;
                        chosenLevel.SolvedInfo.TimeTakenToSolve = 0;
                        gameState = GameState.PlayLevel;
                    }
                    thumbnailPositionX += 130;
                    if ((countOfThumbnails % thumbnailsPerRow) == 0)
                    {
                        thumbnailPositionY += 150;
                        thumbnailPositionX = 0;
                    }
                    countOfThumbnails++;
                }
            }
        }

        private void MouseClickedBack(int x, int y)
        {
            if ((gameState == GameState.LevelMenu) || (gameState == GameState.PlayLevel) || (gameState == GameState.MakeLevel) || (gameState == GameState.DrawLevel))
            {
                Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);
                Rectangle mouseClickedBack = new Rectangle(15,400, 250, 64);
                if (mouseClickRect.Intersects(mouseClickedBack))
                {
                    if ((gameState == GameState.LevelMenu) || (gameState == GameState.MakeLevel) || (gameState == GameState.DrawLevel))
                    {
                        gameState = GameState.StartMenu;
                    }
                    else if (gameState == GameState.PlayLevel) 
                    {
                        gameState = GameState.LevelMenu;
                        levels = levelManager.LoadAllLevels(graphics, Content);
                    }
                }
            }
        }

        private void MouseHoveredOverClickedSave(int x, int y)
        {
            if ((gameState == GameState.LevelMenu) || (gameState == GameState.PlayLevel) || (gameState == GameState.MakeLevel) || (gameState == GameState.DrawLevel))
            {
                Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);
                Rectangle mouseClickedBack = new Rectangle(500, 400, 250, 64);
                if (mouseClickRect.Intersects(mouseClickedBack))
                {
                    hoverSave = true;
                }
                else
                {
                    hoverSave = false;
                }
            }
        }

        private void MouseClickedSave(int x, int y)
        {
            if ((gameState == GameState.LevelMenu) || (gameState == GameState.PlayLevel) || (gameState == GameState.MakeLevel))
            {
                Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);
                Rectangle mouseClickedBack = new Rectangle(500, 400, 250, 64);
                if (mouseClickRect.Intersects(mouseClickedBack))
                {                    
                    ImageHelper imgHlp = new ImageHelper();
                    System.Drawing.Image image = imgHlp.LoadImage(imageFileName);
                    image = imgHlp.ResizeImage(image, 100, 100);
                    string levelFileName = fileHlp.GetNewLevelFileName();
                    levelManager.SaveLevelDesign(previewBoardCells, image, levelFileName);
                    hasBeenCreated = false;
                    gameState = GameState.StartMenu;
                }
            }
        }

        private void MouseHoveredOverClickedBack(int x, int y)
        {
            if ((gameState == GameState.LevelMenu) || (gameState == GameState.PlayLevel) || (gameState == GameState.MakeLevel) || (gameState == GameState.DrawLevel))
            {
                Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);
                Rectangle mouseClickedBack = new Rectangle(15, 400, 250, 64);
                if (mouseClickRect.Intersects(mouseClickedBack))
                {
                    hoverBack = true;
                }
                else
                {
                    hoverBack = false;
                }
            }
        }

        private void MouseHoveredOverLevel(int x, int y)
        {
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);
            if (gameState == GameState.LevelMenu)
            {
                int countOfThumbnails = 1;
                int thumbnailPositionX = 0;
                int thumbnailPositionY = 50;
                foreach (Level level in levels)
                {
                    Rectangle levelRect = new Rectangle(levelStartPositionX + thumbnailPositionX, levelStartPositionY + thumbnailPositionY, 125, 125);

                    if (mouseClickRect.Intersects(levelRect))
                    {
                        level.Hover = true;
                    }
                    else
                    {
                        level.Hover = false;
                    }
                    thumbnailPositionX += 130;
                    if ((countOfThumbnails % thumbnailsPerRow) == 0)
                    {
                        thumbnailPositionY += 150;
                        thumbnailPositionX = 0;
                    }
                    countOfThumbnails++;
                }

            }
        }

        private void MouseClicked(int x, int y)
        {
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);
            if (gameState == GameState.StartMenu)
            {
                Rectangle chooseLevelRect = new Rectangle((int)chooseLevelPosition.X, (int)chooseLevelPosition.Y, 250, 64);
                Rectangle newLevelRect = new Rectangle((int)newLevelPosition.X, (int)newLevelPosition.Y, 250, 64);
                Rectangle drawLevelRect = new Rectangle((int)drawLevelPosition.X, (int)drawLevelPosition.Y, 250, 64);
                Rectangle exitRect = new Rectangle((int)exitPosition.X, (int)exitPosition.Y, 250, 64);
                if (mouseClickRect.Intersects(chooseLevelRect))
                {
                    gameState = GameState.LevelMenu;
                }
                else if (mouseClickRect.Intersects(newLevelRect)) 
                {
                    gameState = GameState.MakeLevel;
                }
                else if (mouseClickRect.Intersects(drawLevelRect)) 
                {
                    gameState = GameState.DrawLevel;
                }
                else if (mouseClickRect.Intersects(exitRect)) 
                {
                    Exit();
                }
            }
        }

        private void MouseHovered(int x, int y)
        {
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);
            if (gameState == GameState.StartMenu)
            {
                Rectangle chooseLevelRect = new Rectangle((int)chooseLevelPosition.X, (int)chooseLevelPosition.Y, 250, 64);
                Rectangle newLevelRect = new Rectangle((int)newLevelPosition.X, (int)newLevelPosition.Y, 250, 64);
                Rectangle drawLevelRect = new Rectangle((int)drawLevelPosition.X, (int)drawLevelPosition.Y, 250, 64);
                Rectangle exitRect = new Rectangle((int)exitPosition.X, (int)exitPosition.Y, 250, 64);
                if (mouseClickRect.Intersects(chooseLevelRect))
                {
                    hover = true;
                    HoveredOverMenuItem = MenuItemHover.chooseLevel;
                }
                else if (mouseClickRect.Intersects(newLevelRect))
                {
                    hover = true;
                    HoveredOverMenuItem = MenuItemHover.newLevel;
                }
                else if (mouseClickRect.Intersects(drawLevelRect))
                {
                    hover = true;
                    HoveredOverMenuItem = MenuItemHover.drawLevel;
                }
                else if (mouseClickRect.Intersects(exitRect))
                {
                    hover = true;
                    HoveredOverMenuItem = MenuItemHover.exit;                    
                }
                else
                {
                    hover = false;
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.IsMouseVisible = true;
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            if (gameState == GameState.StartMenu)
            {
                spriteBatch.Draw(pattern, new Vector2(0, 0), Color.White);
                spriteBatch.Draw(chooseLevel, chooseLevelPosition, new Rectangle(0, 0, 250, 64), Color.White);
                spriteBatch.Draw(newLevel, newLevelPosition, new Rectangle(0, 0, 250, 64), Color.White);
                spriteBatch.Draw(drawLevel, drawLevelPosition, new Rectangle(0, 0, 250, 64), Color.White);
                spriteBatch.Draw(exit, exitPosition, new Rectangle(0, 0, 250, 64), Color.White);
                spriteBatch.Draw(logo, new Vector2 (35,50), Color.White);

                if (hover)
                {
                    if (HoveredOverMenuItem == MenuItemHover.chooseLevel)
                    {
                        spriteBatch.Draw(menuHoveredOver, chooseLevelPosition, new Rectangle(0, 0, 250, 64), Color.White);
                    }
                    else if (HoveredOverMenuItem == MenuItemHover.newLevel)
                    {
                        spriteBatch.Draw(menuHoveredOver, newLevelPosition, new Rectangle(0, 0, 250, 64), Color.White);
                    }
                    else if (HoveredOverMenuItem == MenuItemHover.drawLevel)
                    {
                        spriteBatch.Draw(menuHoveredOver, drawLevelPosition, new Rectangle(0, 0, 250, 64), Color.White);
                    }
                    else if (HoveredOverMenuItem == MenuItemHover.exit)
                    {
                        spriteBatch.Draw(menuHoveredOver, exitPosition, new Rectangle(0, 0, 250, 64), Color.White);
                    }
                }
            }

            if (gameState == GameState.LevelMenu)
            {
                int countOfThumbnails = 1;
                int thumbnailPositionX = 0;
                int thumbnailPositionY = 50;

                spriteBatch.Draw(back, new Vector2(15, 400), new Rectangle(0, 0, 250, 64), Color.White);
                if (hoverBack)
                {
                    spriteBatch.Draw(menuHoveredOver, new Vector2(15, 400), new Rectangle(0, 0, 250, 64), Color.White);                    
                }

                foreach (Level level in levels)
                {
                    if (level.SolvedInfo.HasBeenSolved)
                    {
                        spriteBatch.Draw(questionMark, new Vector2(levelStartPositionX + thumbnailPositionX, levelStartPositionY + thumbnailPositionY), Color.White);
                        spriteBatch.Draw(level.TextureThumbnail, new Vector2(levelStartPositionX + thumbnailPositionX + 13, levelStartPositionY + thumbnailPositionY + 13), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(questionMark, new Vector2(levelStartPositionX + thumbnailPositionX, levelStartPositionY + thumbnailPositionY), Color.White);
                    }
                    if (level.Hover)
                    {
                        spriteBatch.Draw(levelHoveredOver, new Vector2(levelStartPositionX + thumbnailPositionX, levelStartPositionY + thumbnailPositionY), Color.White);
                        spriteBatch.DrawString(font, "Time:" + level.SolvedInfo.TimeTakenToSolve.ToString("0.00"), new Vector2(50, 80), Color.Black);

                        if ((level.SolvedInfo.HasBeenSolved) && (level.SolvedInfo.TimeTakenToSolve <= 1))
                        {
                            spriteBatch.Draw(gold, new Vector2(50, 120), Color.White);
                        }
                        else if ((level.SolvedInfo.HasBeenSolved) && (level.SolvedInfo.TimeTakenToSolve <= 2))
                        {
                            spriteBatch.Draw(silver, new Vector2(50, 120), Color.White);
                        }
                        else if ((level.SolvedInfo.HasBeenSolved) && (level.SolvedInfo.TimeTakenToSolve <= 3))
                        {
                            spriteBatch.Draw(bronze, new Vector2(50, 120), Color.White);
                        }
                    }
                    thumbnailPositionX += 130;
                    if ((countOfThumbnails % thumbnailsPerRow) == 0)
                    {
                        thumbnailPositionY += 150;
                        thumbnailPositionX = 0;
                    }
                    countOfThumbnails++;
                }
            }

            if ((gameState == GameState.PlayLevel) && (null!=chosenLevel))
            {
                boardHlp.DrawBoard(spriteBatch, chosenLevel.Cells, boardStartPositionX, boardStartPositionY, BasicCell.SPRITE_WIDTH, BasicCell.SPRITE_HEIGHT);
                clueHlp.DrawClues(spriteBatch, font, chosenLevel.XClues, chosenLevel.YClues, boardStartPositionX, boardStartPositionY);

                if (boardRectangle.Contains(mouseX, mouseY))
                {
                    spriteBatch.Draw(CellHoveredOver, new Vector2(chosenLevel.Cells[currentCellIndex].X, chosenLevel.Cells[currentCellIndex].Y), Color.White);
                }

                if (!chosenLevel.SolvedInfo.HasBeenSolvedLock)
                {
                    spriteBatch.DrawString(font, "Time:" + timeTakenToSolve.ToString("0.00"), new Vector2(200, 20), Color.Black);
                }

                if(chosenLevel.SolvedInfo.HasBeenSolvedLock)
                {
                   // spriteBatch.Draw(win, new Vector2(400, 200), Color.White);
                }

                spriteBatch.Draw(back, new Vector2(15, 400), new Rectangle(0, 0, 250, 64), Color.White);
                if (hoverBack)
                {
                    spriteBatch.Draw(menuHoveredOver, new Vector2(15, 400), new Rectangle(0, 0, 250, 64), Color.White);
                }

            }

            if(gameState == GameState.MakeLevel)
            {
                spriteBatch.Draw(back, new Vector2(15, 400), new Rectangle(0, 0, 250, 64), Color.White);
                spriteBatch.Draw(save, new Vector2(500, 400), new Rectangle(0, 0, 250, 64), Color.White);
                if (hoverBack)
                {
                    spriteBatch.Draw(menuHoveredOver, new Vector2(15, 400), new Rectangle(0, 0, 250, 64), Color.White);
                }
                if(hoverSave)
                {
                    spriteBatch.Draw(menuHoveredOver, new Vector2(500, 400), new Rectangle(0, 0, 250, 64), Color.White);
                }

                if(null!=colourTexture)
                {
                    spriteBatch.Draw(colourTexture, new Vector2(20, 30), Color.White);

                }
                if(null!=blackAndWhiteTexture)
                {
                    spriteBatch.Draw(blackAndWhiteTexture, new Vector2(240, 30), Color.White);
                }
                if(null!=previewBoardCells)
                {
                    boardHlp.DrawBoard(spriteBatch, previewBoardCells, 460, 30, BasicCell.SPRITE_WIDTH, BasicCell.SPRITE_HEIGHT);
                }
            }

            if (gameState == GameState.DrawLevel)
            {
                spriteBatch.Draw(back, new Vector2(15, 400), new Rectangle(0, 0, 250, 64), Color.White);
                spriteBatch.Draw(save, new Vector2(500, 400), new Rectangle(0, 0, 250, 64), Color.White);
                if (hoverBack)
                {
                    spriteBatch.Draw(menuHoveredOver, new Vector2(15, 400), new Rectangle(0, 0, 250, 64), Color.White);
                }
                if (hoverSave)
                {
                    spriteBatch.Draw(menuHoveredOver, new Vector2(500, 400), new Rectangle(0, 0, 250, 64), Color.White);
                }
                if (null != previewBoardCells)
                {
                    boardHlp.DrawBoard(spriteBatch, previewBoardCells, 150, 30, BasicCell.SPRITE_WIDTH, BasicCell.SPRITE_HEIGHT);
                }
            }


            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
