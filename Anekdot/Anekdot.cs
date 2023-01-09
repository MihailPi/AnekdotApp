using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AnekdotApp
{
    public class Anekdot
    {
        private List<string> _allStories;
        private string[] _allTextFromAllFile;
        private readonly string _path;
        private readonly char _separator;
        public int Count { get; private set; }

        public Anekdot(string pathToFolder, char separator)
        {
            _path = pathToFolder;
            _separator = separator;
            GetStoryFromFile();
            //  Кол-во анекдотов
            Count = _allTextFromAllFile.Length;
        }

        public string GetOneStory(int numberOfStory)
        {
            try
            {
                if (numberOfStory <= 0)
                    throw new Exception("Нет анекдота с таким номером");
                else
                    return _allTextFromAllFile[numberOfStory - 1];
            }
            catch (Exception)
            {
                throw new Exception("Нет анекдота с таким номером");
            }
        }

        public List<string> GetAllStories()
        {
            _allStories = new List<string>();
            foreach (var story in _allTextFromAllFile)
                _allStories.Add(story);

            return _allStories;
        }

        private void GetStoryFromFile()
        {
            string dataFromStream = string.Empty;
            //  Считываем все строки из всех файлов
            using (var sr = new StreamReader(_path, Encoding.UTF8))
            {
                dataFromStream += sr.ReadToEnd();
            }
            _allTextFromAllFile = dataFromStream.Split(_separator);
        }
    }
}
