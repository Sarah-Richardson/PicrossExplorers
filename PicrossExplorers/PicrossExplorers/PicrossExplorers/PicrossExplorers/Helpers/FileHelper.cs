using PicrossExplorers.SpriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

namespace PicrossExplorers.Helpers
{
    public class FileHelper
    {
        public const string LEVEL_PATH = @"./Levels";
        public const string IMAGE_PATH = @"./Images";

        public void SaveLevel(string levelData, string fileName)
        {            
            File.WriteAllText(Path.Combine(LEVEL_PATH, fileName), levelData);            
        }

        public void LoadSolved(Level level)
        {
            string path = Path.Combine(LEVEL_PATH, level.FileName + ".json");
            if (File.Exists(path))
            {
                SolvedInformation info = JsonConvert.DeserializeObject<SolvedInformation>(File.ReadAllText(path));
                info.HasBeenSolvedLock = false;
                level.SolvedInfo = info;
            }
        }

        public string[] LoadLevelFromPath(string path)
        {
            string[] data = null;
            string levelData = File.ReadAllText(path);
            char[] split = { ',' };
            data = levelData.Split(split);
            return data;
        }

        public string[] LoadLevel(string fileName)
        {
            string[] data = null;
            string levelData = File.ReadAllText(Path.Combine(LEVEL_PATH, fileName));
            char[] split = { ',' };
            data = levelData.Split(split);
            return data;
        }

        public string GetNewLevelFileName()
        {
            string rtn = "";
            string[] files = Directory.GetFiles(FileHelper.LEVEL_PATH, "*.txt");
            string fn = files.OrderBy(path => Int32.Parse(Path.GetFileNameWithoutExtension(path))).LastOrDefault();
            int lastLevel = 0;
            if(int.TryParse(Path.GetFileNameWithoutExtension(fn), out lastLevel))
            {
                lastLevel++;
                rtn = lastLevel.ToString();
            }

            return rtn;
        }

        public void SaveSolvedInformation(SolvedInformation solvedInfo, string fileName)
        {            
            string solvedData = JsonConvert.SerializeObject(solvedInfo);
            File.WriteAllText(Path.Combine(LEVEL_PATH, fileName + ".json"), solvedData);         
        }

        public string GetImageFileName()
        {
            string fileName = "";
            string[] files = Directory.GetFiles(FileHelper.IMAGE_PATH, "*.*");
            if ((null != files) && (files.Length > 0))
            {
                fileName = Path.GetFileName(files[0]);
            }
            return fileName;
        }
    }
}
