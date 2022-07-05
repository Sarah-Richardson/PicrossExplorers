using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicrossExplorers.SpriteHelpers
{
    [Serializable]
    public class SolvedInformation
    {
        public bool HasBeenSolved { get; set; } = false;
        public bool HasBeenSolvedLock { get; set; } = false;

        public float TimeTakenToSolve { get; set; } = 0;

        public SolvedInformation() { }
    }
}
