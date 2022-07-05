using PicrossExplorers.SpriteHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicrossExplorers.Helpers
{
    public class CalculateClues
    {

        private int[] endXPositions = { 9, 19, 29, 39, 49, 59, 69, 79, 89, 99 };
        private int[] endYPositions = { 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };

        public List<Clue> GetXClues(string[] data)
        {
            List<Clue> xClues = new List<Clue>();
            int position = 0;
            Clue clue = new Clue();
            do
            {                
                string currentValue = data[position];
                if (currentValue == "1") // start of run
                {
                    int countOfRun = FollowRunX(ref position, data, clue);                 
                }                
                if (endXPositions.Contains(position))
                {
                    if (clue.Clues.Count==0)
                    {
                        clue.Clues.Add(0);
                    }
                    xClues.Add(clue);
                    clue = new Clue();
                }
                position++;
            }
            while (position < 100);
            return xClues;            
        }

        public List<Clue> GetYClues(string[] data)
        {
            List<Clue> yClues = new List<Clue>();
            int position = 0;
            int x = 0;
            Clue clue = new Clue();
            bool finished = false;
            do
            {
                string currentValue = data[position];
                if (currentValue == "1") // start of run
                {
                    int countOfRun = FollowRunY(ref position, data, clue);
                }
                if (endYPositions.Contains(position))
                {
                    if (clue.Clues.Count == 0)
                    {
                        clue.Clues.Add(0);
                    }
                    yClues.Add(clue);
                    clue = new Clue();
                    if (position == 99)
                    {
                        finished = true;
                    }
                    else
                    {
                        x = x + 1;
                        position = x;
                    }
                }
                else
                {
                    position = position + 10;
                }
            }
            while (!finished);
            return yClues;
        }

        private int FollowRunX(ref int position, string[] data, Clue clue)
        {
            int countOfRun = 0;
            string value = data[position]; //value is one to start of with
            while (!IsEndOfRunX(position, data))
            {                
                value = data[position];
                position++;
                countOfRun++;
            }            
            if ((data[position] == "1") && (endXPositions.Contains(position))) countOfRun++;
            clue.Clues.Add(countOfRun);
            return countOfRun;
        }

        private bool IsEndOfRunX(int position, string[] data)
        {
            bool isEnd = false;
            if((data[position]=="0") || (endXPositions.Contains(position)))
            {
                isEnd = true; // zero is the end of a run
            }
            return isEnd;
        }

        private int FollowRunY(ref int position, string[] data, Clue clue)
        {
            int countOfRun = 0;
            string value = data[position]; //value is one to start of with
            while (!IsEndOfRunY(position, data))
            {
                value = data[position];
                position = position + 10;
                countOfRun++;
            }
            if ((data[position] == "1") && (endYPositions.Contains(position))) countOfRun++;
            clue.Clues.Add(countOfRun);
            return countOfRun;
        }

        private bool IsEndOfRunY(int position, string[] data)
        {
            bool isEnd = false;
            if ((data[position] == "0") || (endYPositions.Contains(position)))
            {
                isEnd = true; // zero is the end of a run
            }
            return isEnd;
        }

    }
}
