using System;
using System.IO;

namespace TestFiles_3
{
    class Program
    {
        static void Main(string[] args)
        {
            double totalSize = 0;
            double totalSizeAfterDel = 0;

            DirectoryInfo dirInf;

            string dirpath = null;

            do
            {
                Console.WriteLine("Введите корректный путь до каталога - ");
                dirpath = Console.ReadLine();

                if ((isDirectoryNameValid(dirpath) == true) & Directory.Exists(dirpath))
                {
                    break;
                }
                else
                    Console.WriteLine("Введен некорректный путь!");
            } while (true);

            dirInf = new DirectoryInfo(dirpath);
            totalSize = sizeOfFolder(dirpath, ref totalSize);

            Console.WriteLine("Весь объем вложенных файлов и каталогов до удаления - " + totalSize);

            DeleteDirectorires(dirpath);

            totalSize = sizeOfFolder(dirpath, ref totalSizeAfterDel);

            Console.WriteLine("Весь объем вложенных файлов и каталогов после удаления - " + totalSize);
        }

        static void DeleteDirectorires(string path)
        {
            DirectoryInfo dirInf = new DirectoryInfo(path);
            try
            {
                foreach (var directory in dirInf.GetDirectories())
                {
                    if (Directory.GetLastAccessTime(directory.FullName).Minute > TimeSpan.FromMinutes(30).TotalMinutes)
                    {
                        directory.Delete(true);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Косяк - " + e);
            }
        }
        static double sizeOfFolder(string folder, ref double totalSize)
        {
            try
            {
                DirectoryInfo dirInf = new DirectoryInfo(folder);
                DirectoryInfo[] dirArray = dirInf.GetDirectories();
                FileInfo[] fileInf = dirInf.GetFiles();

                foreach (FileInfo file in fileInf)
                {
                    totalSize = totalSize + file.Length;
                }

                foreach (DirectoryInfo df in dirArray)
                {
                    sizeOfFolder(df.FullName, ref totalSize);
                }

                return totalSize;
            }
            catch (Exception e)
            {
                Console.WriteLine("Что - то пошло не так... " + e);
                return 0;
            }
        }
        static bool isDirectoryNameValid(string dirName)
        {
            if ((dirName == null) || (dirName.IndexOfAny(Path.GetInvalidPathChars()) != -1))
                return false;
            try
            {
                return true;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }
    }
}
