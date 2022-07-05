using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Microsoft.Xna.Framework.Graphics;

namespace PicrossExplorers.Helpers
{
    public class ImageHelper
    {
        public const string IMAGE_PATH = @"./Images";

        public Image LoadImage(string fileName)
        {
            Image image = Image.FromFile(Path.Combine(IMAGE_PATH, fileName), true);                        
            return image;
        }

        public void SaveImage(Image image, string fileName)
        {
            string path = Path.Combine(FileHelper.LEVEL_PATH, fileName);
            image.Save(path, ImageFormat.Png);
        }

        public Image ResizeImage(Image image, int width, int height)
        {
            Bitmap newImage = new Bitmap(width, height);
            Rectangle newSize = new Rectangle(0, 0, width, height);
            newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;                
                graphics.CompositingQuality = CompositingQuality.HighQuality;                
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                using (ImageAttributes imageAttributes = new ImageAttributes())
                {
                    imageAttributes.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, newSize, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
                }
            }
            return newImage;
        }

        public Image ConvertImageToBlackAndWhite(Image image)
        {
            using (Graphics graphics = Graphics.FromImage(image)) 
            {
                var gray_matrix = new float[][] {
                new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
                new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
                new float[] { 0,      0,      0,      1, 0 },
                new float[] { 0,      0,      0,      0, 1 }};
                using (ImageAttributes imageAttributes = new ImageAttributes())
                {
                    imageAttributes.SetColorMatrix(new ColorMatrix(gray_matrix));
                    imageAttributes.SetThreshold(0.8f); 
                    Rectangle rectangle = new Rectangle(0, 0, image.Width, image.Height);
                    graphics.DrawImage(image, rectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
                }
            }
            return image;
        }

        public Texture2D GetTextureFromImage(GraphicsDevice graphics, System.Drawing.Bitmap image)
        {
            int[] imgData = new int[image.Width * image.Height];
            Texture2D texture = new Texture2D(graphics, image.Width, image.Height);
            unsafe
            {
                System.Drawing.Imaging.BitmapData origdata = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, image.PixelFormat);
                uint* byteData = (uint*)origdata.Scan0;
                for (int i = 0; i < imgData.Length; i++)
                {
                    byteData[i] = (byteData[i] & 0x000000ff) << 16 | (byteData[i] & 0x0000FF00) | (byteData[i] & 0x00FF0000) >> 16 | (byteData[i] & 0xFF000000);
                }
                System.Runtime.InteropServices.Marshal.Copy(origdata.Scan0, imgData, 0, image.Width * image.Height);
                byteData = null;
                image.UnlockBits(origdata);
            }
            texture.SetData(imgData);
            return texture;
        }

    }
}
