using System;
using System.Windows.Forms;

namespace AnekdotApp
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Для старта нажмите Enter...");
            Console.ReadLine();

            string pathToBackground="";
            string pathToStories="";
            string pathToSave="";
            char separator;

            var fileDialogText = new OpenFileDialog()
            {
                Filter = "Text | *.txt"
            };
            var fileDialogImage = new OpenFileDialog()
            {
                Filter = "Image | *.jpg; *.png; *.bmp"
            };
            var saveDialog = new FolderBrowserDialog();

            while (true)
            {
                //  Получение путей к файлам
                Console.WriteLine("Выберете текстовый файл...(Нажмите Enter)");
                Console.ReadLine();
                if (fileDialogText.ShowDialog() == DialogResult.OK)
                    pathToStories = fileDialogText.FileName;
                else
                    continue;
                Console.WriteLine();

                Console.WriteLine("Выберете изображение для фона...(Нажмите Enter)");
                Console.ReadLine();
                if (fileDialogImage.ShowDialog() == DialogResult.OK)
                    pathToBackground = fileDialogImage.FileName;
                else
                    continue;
                Console.WriteLine();

                Console.WriteLine("Введите разделитель для текстового файла:");
                separator = Console.ReadLine().ToCharArray()[0];
                Console.WriteLine();

                // Загружаем текст из файла
                Anekdot anekdot = new Anekdot(pathToStories, separator);
                var allStories = anekdot.GetAllStories();

                Console.WriteLine("Укажите папку для сохраниния изображений...(Нажмите Enter)");
                Console.ReadLine();
                if (saveDialog.ShowDialog() == DialogResult.OK)
                    pathToSave = saveDialog.SelectedPath;

                Console.WriteLine(new string('_', 60));
                Console.WriteLine("Рендерим...");

                //  Создаем картинки
                Poster poster = new Poster();

                for (int i = 0; i < allStories.Count; i++)
                {
                    poster.Create(allStories[i], pathToBackground);
                    poster.SaveRender(pathToSave + $@"\{i+1}.jpg");
                }
                Console.WriteLine();
                Console.WriteLine("Готово!");

                Console.WriteLine("Для продолжения нажмите Enter, для выхода q...");
                if (Console.ReadKey().Key == ConsoleKey.Q)
                    return;
            }
        }
    }
}
