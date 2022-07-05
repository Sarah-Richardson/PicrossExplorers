using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PicrossExplores.SpriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicrossExplorers.Helpers
{
    public class LevelDesigner
    {
        public bool CreateLevelFromImage(string fileName, ref Texture2D colourTexture, ref Texture2D blackAndWhiteTexture, ref List<BasicCell> previewBoardCells, GraphicsDeviceManager graphics, ContentManager content)
        {
            bool success = false;
            try
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    ImageHelper imgHlp = new ImageHelper();
                    System.Drawing.Image image = imgHlp.LoadImage(fileName); //load image from image directory
                    image = imgHlp.ResizeImage(image, 200, 300); //resize image 200 x 300 
                    colourTexture = imgHlp.GetTextureFromImage(graphics.GraphicsDevice, ((System.Drawing.Bitmap)image));
                    System.Drawing.Image imageBw = imgHlp.ConvertImageToBlackAndWhite(image);
                    blackAndWhiteTexture = imgHlp.GetTextureFromImage(graphics.GraphicsDevice, ((System.Drawing.Bitmap)imageBw));
                    System.Drawing.Image picrossImage = imgHlp.ResizeImage(imageBw, 10, 10);
                    previewBoardCells = SpriteBuilder.BuildCells(content);
                    int count = 0;
                    for (int x = 0; x < ((System.Drawing.Bitmap)picrossImage).Width; x++)
                    {
                        for (int y = 0; y < ((System.Drawing.Bitmap)picrossImage).Height; y++)
                        {
                            System.Drawing.Color clr = ((System.Drawing.Bitmap)picrossImage).GetPixel(x, y);
                            int red = clr.R;
                            int green = clr.G;
                            int blue = clr.B;
                            if ((red == 0) && (green == 0) && (blue == 0))
                            {
                                previewBoardCells[count].CellState = BasicCell.State.Background;
                            }
                            else
                            {
                                previewBoardCells[count].CellState = BasicCell.State.Foreground;
                            }
                            count++;
                        }
                    }                    
                    success = true;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                success = false;
            }
            return success;
        }

    }
}
