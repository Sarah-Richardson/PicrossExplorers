using System;

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
