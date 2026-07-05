using System;
using System.IO;
using System.Linq;
using CommandLib;

namespace FileSystemCommands
{
    public class DirectorySizeCommand : ICommand
    {
        private readonly string _directoryPath;

        public DirectorySizeCommand(string directoryPath)
        {
            _directoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));
        }

        public void Execute()
        {
            if (!Directory.Exists(_directoryPath))
            {
                Console.WriteLine($"Директория '{_directoryPath}' не найдена");
                return;
            }

            var size = GetDirectorySize(_directoryPath);
            Console.WriteLine($"Размер директории: {size} байт");
        }

        public long GetDirectorySize(string path)
        {
            var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            return files.Sum(file =>
            {
                try
                {
                    return new FileInfo(file).Length;
                }
                catch
                {
                    return 0;
                }
            });
        }
    }
    public class FindFilesCommand : ICommand
    {
        private readonly string _directoryPath;
        private readonly string _searchPattern;

        public FindFilesCommand(string directoryPath, string searchPattern)
        {
            _directoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));
            _searchPattern = searchPattern ?? throw new ArgumentNullException(nameof(searchPattern));
        }

        public void Execute()
        {
            if (!Directory.Exists(_directoryPath))
            {
                Console.WriteLine($"Директория '{_directoryPath}' не найдена");
                return;
            }

            var files = Directory.GetFiles(_directoryPath, _searchPattern, SearchOption.AllDirectories);
            Console.WriteLine($"Найдено {files.Length} файлов по маске '{_searchPattern}':");
            foreach (var file in files)
            {
                Console.WriteLine($"{file}");
            }
        }
        public string[] GetFiles()
        {
            if (!Directory.Exists(_directoryPath))
            {
                return Array.Empty<string>();
            }
            return Directory.GetFiles(_directoryPath, _searchPattern, SearchOption.AllDirectories);
        }
    }
}
