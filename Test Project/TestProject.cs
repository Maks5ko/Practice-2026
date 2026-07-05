using System;
using System.IO;
using System.Linq;
using Xunit;
using FileSystemCommands;

namespace task08tests
{
    public class FileSystemCommandsTests: IDisposable
    {
        private readonly string _testDir;

        public FileSystemCommandsTests()
        {
            _testDir = Path.Combine(Path.GetTempPath(), "TestDir_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDir);
        }
        public void Dispose()
        {
            if (Directory.Exists(_testDir))
            {
                try
                {
                    Directory.Delete(_testDir, true);
                }
                catch { }
            }
        }
        [Fact]
        public void DirectorySizeCommand_ShouldCalculateCorrectSize()
        {
            File.WriteAllText(Path.Combine(_testDir, "test1.txt"), "Hello");
            File.WriteAllText(Path.Combine(_testDir, "test2.txt"), "World");
            var subDir = Path.Combine(_testDir, "SubDir");
            Directory.CreateDirectory(subDir);
            File.WriteAllText(Path.Combine(subDir, "test3.txt"), "12345");
            var command = new DirectorySizeCommand(_testDir);
            long size = command.GetDirectorySize(_testDir);
            Assert.Equal(15, size);
        }

        [Fact]
        public void DirectorySizeCommand_WhenDirectoryDoesNotExist_RunWithoutErrors()
        {
            var command = new DirectorySizeCommand(Path.Combine(_testDir, "NonExistent"));
            var exception = Record.Exception(() => command.Execute());
            Assert.Null(exception);
        }
        [Fact]
        public void DirectorySizeCommand_EmptyDirectory_ShouldReturnZero()
        {
            var command = new DirectorySizeCommand(_testDir);
            long size = command.GetDirectorySize(_testDir);
            Assert.Equal(0, size);
        }
        [Fact]
        public void FindFilesCommand_ShouldFindMatchingFiles()
        {
            var testDir = Path.Combine(_testDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
            File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

            var command = new FindFilesCommand(testDir, "*.txt");
            command.Execute();

            string[] files = command.GetFiles();
            Assert.Single(files);
            Assert.EndsWith(".txt", files[0]);
        }
            
        [Fact]
        public void FindFilesCommand_WhenDirectoryDoesNotExist_ShouldNotThrow()
        {
            var command = new FindFilesCommand(Path.Combine(_testDir, "NonExistent"), "*.txt");
            var exception = Record.Exception(() => command.Execute());
            Assert.Null(exception);
        }

        [Fact]
        public void FindFilesCommand_NoMatches_ReturnsZero()
        {
            File.WriteAllText(Path.Combine(_testDir, "file1.log"), "Log");

            var command = new FindFilesCommand(_testDir, "*.txt");

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                command.Execute();
                var output = sw.ToString();

                Assert.Contains("Найдено 0 файлов по маске '*.txt'", output);
            }
        }
    }
}
