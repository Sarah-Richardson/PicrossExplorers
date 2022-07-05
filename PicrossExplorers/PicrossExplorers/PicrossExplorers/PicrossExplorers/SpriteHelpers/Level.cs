using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PicrossExplores.SpriteHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicrossExplorers.SpriteHelpers
{
    public class Level
    {
        public SolvedInformation SolvedInfo { get; set; } = new SolvedInformation();

        public string FileName { get; set; }
        public bool Hover { get; set; }

        public List<Clue> XClues { get; set; } = new List<Clue>();
        public List<Clue> YClues { get; set; } = new List<Clue>();

        public Texture2D TextureThumbnail { get; set; }
        public List<BasicCell> Cells { get; set; } = null;

        public string[] data = null;

        public Level(ContentManager content)
        {
            Cells = SpriteBuilder.BuildCells(content);
        }

        public bool Solved()
        {
            bool solved = true;
            int position = 0;
            foreach(BasicCell cell in Cells)
            {
                if(cell.CellState == BasicCell.State.ClickedOn)
                {
                    if(data[position]=="0")
                    {
                        solved = false;
                        break;
                    }
                }
                else
                {
                    if (data[position] == "1")
                    {
                        solved = false;
                        break;
                    }
                }
                position++;
            }
            return solved;
        }
    }
}
