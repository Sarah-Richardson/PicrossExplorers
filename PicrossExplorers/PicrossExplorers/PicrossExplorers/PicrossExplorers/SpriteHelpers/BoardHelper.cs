using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicrossExplores.SpriteHelpers
{
    public class BoardHelper
    {

        public void DrawBoard(SpriteBatch spriteBatch, List<BasicCell> cells, int startPositionX, int startPositionY, int widthOfSprite, int heightOfSprite)
        {
            int index = 0;
            int x = 0;
            int y = 0;

            for (int numberOfYCells = 0; numberOfYCells < 10; numberOfYCells++)
            {
                y = startPositionY + (numberOfYCells * widthOfSprite);
                for (int numberOfXCells = 0; numberOfXCells < 10; numberOfXCells++)
                {
                    x = startPositionX + (numberOfXCells * widthOfSprite);
                    int startPosOfSpriteCell = (((int)cells[index].CellState) - 1) * heightOfSprite; // cell state determines which sprite to draw
                    spriteBatch.Draw(cells[index].textureCell, new Vector2(x, y), new Rectangle(startPosOfSpriteCell, 0, widthOfSprite, heightOfSprite), Color.White);

                    cells[index].X = x;
                    cells[index].Y = y;

                    index++;
                }
                x = 0;
            }
        }

    }
}
