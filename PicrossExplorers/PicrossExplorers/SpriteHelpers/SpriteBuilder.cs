using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicrossExplores.SpriteHelpers
{
    public static class SpriteBuilder
    {
        public static List<BasicCell> BuildCells(Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            List<BasicCell> cells = new List<BasicCell>();
            for (int i = 0; i < 100; i++)
            {
                cells.Add(new BasicCell(0, 0, cm.Load<Texture2D>("CellSheet"), BasicCell.State.Normal));
            }
            return cells;
        }
    }
}
