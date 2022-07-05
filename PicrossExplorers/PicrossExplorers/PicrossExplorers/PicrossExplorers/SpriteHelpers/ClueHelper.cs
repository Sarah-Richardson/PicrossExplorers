using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicrossExplorers.SpriteHelpers
{
    public class ClueHelper
    {

        public void DrawClues(SpriteBatch spriteBatch, SpriteFont font, List<Clue> xClues, List<Clue> yClues, int xPositionBoard, int yPositionBoard)
        {
            DrawXClues(spriteBatch, font, xClues, xPositionBoard, yPositionBoard);
            DrawYClues(spriteBatch, font, yClues, xPositionBoard, yPositionBoard);
        }

        private void DrawXClues(SpriteBatch spriteBatch, SpriteFont font, List<Clue> xClues, int xPositionBoard, int yPositionBoard)
        {
            if (null != xClues)
            {
                int cluePositionY = yPositionBoard + 9;
                foreach (Clue c in xClues)
                {
                    int cluePositionX = xPositionBoard - 27;
                    for (int jj = c.Clues.Count - 1; jj >= 0; jj--)
                    {
                        spriteBatch.DrawString(font, " " + c.Clues[jj].ToString(), new Vector2(cluePositionX, cluePositionY), Color.Black);
                        cluePositionX = cluePositionX - 24;
                    }
                    cluePositionY = cluePositionY + 30;
                }
            }
        }

        private void DrawYClues(SpriteBatch spriteBatch, SpriteFont font, List<Clue> yClues, int xPositionBoard, int yPositionBoard)
        {
            if (null != yClues)
            {
                int cluePositionX = xPositionBoard + 3;
                foreach (Clue c in yClues)
                {
                    int cluePositionY = yPositionBoard - 27;
                    for (int jj = c.Clues.Count - 1; jj >= 0; jj--)
                    {
                        spriteBatch.DrawString(font, " " + c.Clues[jj].ToString(), new Vector2(cluePositionX, cluePositionY), Color.Black);
                        cluePositionY = cluePositionY - 24;
                    }
                    cluePositionX = cluePositionX + 30;
                }
            }
        }

    }
}
