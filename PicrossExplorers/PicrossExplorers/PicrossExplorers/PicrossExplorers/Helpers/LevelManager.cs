using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PicrossExplorers.SpriteHelpers;
using PicrossExplores.SpriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicrossExplorers.Helpers
{
    public class LevelManager
    {
        private bool startedLoading = false;

        public List<Level> LoadAllLevels(GraphicsDeviceManager graphics, ContentManager content)
        {
            FileHelper hlp = new FileHelper();
            CalculateClues calc = new CalculateClues();
            List<Level> levels = new List<Level>();            
            var fileEntries = Directory.GetFiles(FileHelper.LEVEL_PATH, "*.txt").OrderBy(path => Int32.Parse(Path.GetFileNameWithoutExtension(path)));
            foreach (string fileName in fileEntries)
            {
                Level level = new Level(content);
                LoadLevelDesign(level, fileName, hlp, calc, content, graphics);
                hlp.LoadSolved(level);
                levels.Add(level);
            }
            return levels;
        }

        public void LoadLevelDesign(Level level, string fileName, FileHelper hlp, CalculateClues calc, ContentManager content, GraphicsDeviceManager graphics)
        {
            if (startedLoading == false)
            {
                startedLoading = true;
                string[] data = hlp.LoadLevelFromPath(fileName);
                level.data = data;
                level.XClues = calc.GetXClues(data);
                level.YClues = calc.GetYClues(data);                
                level.FileName = Path.GetFileNameWithoutExtension(fileName);

                //level.TextureThumbnail = content.Load<Texture2D>(level.FileName);

                FileStream fileStream = new FileStream(Path.Combine(FileHelper.LEVEL_PATH,level.FileName + ".png"), FileMode.Open);
                level.TextureThumbnail = Texture2D.FromStream(graphics.GraphicsDevice, fileStream);
                fileStream.Dispose();
                
                for (int i = 0; i < 100; i++)
                {
                    level.Cells[i].CellState = BasicCell.State.Normal;
                }
                startedLoading = false;
            }
        }

        public void SaveLevelDesign(List<BasicCell> cells, System.Drawing.Image image, string levelFileName)
        {
            ImageHelper imgHlp = new ImageHelper();
            imgHlp.SaveImage(image, levelFileName + ".png");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                if (cells[i].CellState == BasicCell.State.Foreground)
                {
                    sb.Append("1,");
                }
                else
                {
                    sb.Append("0,");
                }
            }
            FileHelper hlp = new FileHelper();
            hlp.SaveLevel(sb.ToString(), levelFileName + ".txt");
        }

        #region debug load level information

        public void KeyboardLoadLevel(KeyboardState keyBoardState, ref List<BasicCell> cells)
        {
            if (keyBoardState.IsKeyDown(Keys.NumPad1)) 
            {
                LoadLevelDesign(cells, "1.txt");
            }
            if (keyBoardState.IsKeyDown(Keys.NumPad2))
            {
                LoadLevelDesign(cells, "2.txt");
            }
            if (keyBoardState.IsKeyDown(Keys.NumPad3))
            {
                LoadLevelDesign(cells, "3.txt");
            }
            if (keyBoardState.IsKeyDown(Keys.NumPad4))
            {
                LoadLevelDesign(cells, "4.txt");
            }
            if (keyBoardState.IsKeyDown(Keys.NumPad5))
            {
                LoadLevelDesign(cells, "5.txt");
            }
            if (keyBoardState.IsKeyDown(Keys.NumPad6))
            {
                LoadLevelDesign(cells, "6.txt");
            }
            if (keyBoardState.IsKeyDown(Keys.NumPad7))
            {
                LoadLevelDesign(cells, "7.txt");
            }
            if (keyBoardState.IsKeyDown(Keys.NumPad8))
            {
                LoadLevelDesign(cells, "8.txt");
            }
            if (keyBoardState.IsKeyDown(Keys.NumPad9))
            {
                LoadLevelDesign(cells, "9.txt");
            }
            if (keyBoardState.IsKeyDown(Keys.NumPad0))
            {
                LoadLevelDesign(cells, "10.txt");
            }
        }

        public void LoadLevelDesign(List<BasicCell> cells, string fileName)
        {
            if (startedLoading == false)
            {
                startedLoading = true;
                FileHelper hlp = new FileHelper();
                string[] data = hlp.LoadLevel(fileName);
                for (int i = 0; i < 100; i++)
                {
                    string co = data[i];
                    if (co == "1")
                    {
                        cells[i].CellState = BasicCell.State.Foreground;
                    }
                    else
                    {
                        cells[i].CellState = BasicCell.State.Background;
                    }
                }
                startedLoading = false;
            }
        }

        #endregion
    }
}
