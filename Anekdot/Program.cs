using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace AnekdotApp
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Для старта нажмите Enter...");
            Console.ReadLine();

            Stopwatch timer = new Stopwatch();
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
                Console.WriteLine($"{pathToStories}\n");

                Console.WriteLine("Выберете изображение для фона...(Нажмите Enter)");
                Console.ReadLine();
                if (fileDialogImage.ShowDialog() == DialogResult.OK)
                    pathToBackground = fileDialogImage.FileName;
                else
                    continue;
                Console.WriteLine($"{pathToBackground}\n");

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

                Console.WriteLine($"{pathToSave}\n");
                Console.WriteLine(new string('_', 60));
                Console.WriteLine("Рендерим...");

                //  Запускаем таймер
                timer.Start();
                //  Создаем картинки
                Poster poster = new Poster();

                foreach (string story in allStories)
                {
                    poster.Create(story, pathToBackground);
                    poster.SaveRender(pathToSave + $@"\{poster.Counter}.png");
                }
                timer.Stop();

                Console.WriteLine();
                Console.WriteLine("Готово!");
                Console.WriteLine($"Созданно {poster.Counter} картинок, за {timer.Elapsed.Seconds} сек...");
                // Сбрасываем таймер
                timer.Reset();

                Console.WriteLine("Для продолжения нажмите Enter, для выхода q...");
                if (Console.ReadKey().Key == ConsoleKey.Q)
                    return;
            }
        }
    }
}
