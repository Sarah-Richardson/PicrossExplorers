using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicrossExplores.SpriteHelpers
{

    public class BasicCell
    {
        public const int SPRITE_WIDTH = 30;
        public const int SPRITE_HEIGHT = 30;

        public bool FilledIn { get; set; } = false; // if filledin is true then it is part of the image

        public bool MouseLeftButtonIsDown { get; set; } = false;
        public bool MouseRightButtonIsDown { get; set; } = false;

        public enum State
        {
            Normal = 1,
            Cross = 2,
            ClickedOn = 3,
            NotSure = 4,            
            Foreground = 5,
            Background = 6,
        };

        public State CellState { get; set; }
        public Texture2D textureCell { get; set; }        
        public int X { get; set; }
        public int Y { get; set; }

        public BasicCell(int x, int y, Texture2D texture, State state)
        {
            X = x;
            Y = y;
            CellState = state;
            textureCell = texture;
        }

    }
}
