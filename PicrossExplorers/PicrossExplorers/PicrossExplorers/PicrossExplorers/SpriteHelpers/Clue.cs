using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicrossExplorers.SpriteHelpers
{
    public class Clue
    {
        public int Position { get; set; } = 0;
        public List<int> Clues { get; set; } = new List<int>();

        public Clue() { }
    }
}
