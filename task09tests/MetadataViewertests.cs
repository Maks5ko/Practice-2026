using System.Reflection;
using Xunit;
using MetadataViewer;
using FileSystemCommands;
using CommandLib;

namespace task09tests
{
    public class MetadataViewerTests
    {
        private static string GetAssemblyPath()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var dllPath = Path.Combine(baseDir, "..", "..", "..", "..", "FileSystemCommands", "bin", "Debug", "net10.0", "FileSystemCommands.dll");
            if (File.Exists(dllPath))
                return dllPath;
            return Path.Combine(baseDir, "FileSystemCommands.dll");
        }

        [Fact]
        public void PrintAssemblyInfo_ShouldDisplayCorrectInfo()
        {
            var assembly = Assembly.LoadFrom(GetAssemblyPath());

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Program.PrintAssemblyInfo(assembly);
                var output = sw.ToString();

                Assert.NotEmpty(output);
                Assert.Contains("Информация о сборке", output);
                Assert.Contains("FileSystemCommands", output);
            }
        }

        [Fact]
        public void PrintAllTypes_ShouldDisplayClasses()
        {
            var assembly = Assembly.LoadFrom(GetAssemblyPath());

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Program.PrintAllTypes(assembly);
                var output = sw.ToString();

                Assert.NotEmpty(output);
                Assert.Contains("Классы", output);
                Assert.Contains("DirectorySizeCommand", output);
                Assert.Contains("FindFilesCommand", output);
            }
        }

        [Fact]
        public void DirectorySizeCommand_ShouldHaveAttributes()
        {
            var type = typeof(DirectorySizeCommand);
            var displayAttr = type.GetCustomAttribute<DisplayNameAttribute>();
            var versionAttr = type.GetCustomAttribute<VersionAttribute>();

            Assert.NotNull(displayAttr);
            Assert.Equal("Команда вычисления размера директории", displayAttr.DisplayName);

            Assert.NotNull(versionAttr);
            Assert.Equal(1, versionAttr.Major);
            Assert.Equal(0, versionAttr.Minor);
        }

        [Fact]
        public void FindFilesCommand_ShouldHaveAttributes()
        {
            var type = typeof(FindFilesCommand);
            var displayAttr = type.GetCustomAttribute<DisplayNameAttribute>();
            var versionAttr = type.GetCustomAttribute<VersionAttribute>();

            Assert.NotNull(displayAttr);
            Assert.Equal("Команда поиска файлов по маске", displayAttr.DisplayName);

            Assert.NotNull(versionAttr);
            Assert.Equal(1, versionAttr.Major);
            Assert.Equal(0, versionAttr.Minor);
        }

        [Fact]
        public void PrintTypeInfo_ForDirectorySizeCommand_ShouldDisplayCorrectInfo()
        {
            var type = typeof(DirectorySizeCommand);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Program.PrintTypeInfo(type);
                var output = sw.ToString();

                Assert.NotEmpty(output);
                Assert.Contains("Класс: FileSystemCommands.DirectorySizeCommand", output);
                Assert.Contains("Execute", output);
                Assert.DoesNotContain("CalculateDirectorySize", output);
            }
        }

        [Fact]
        public void PrintTypeInfo_ForFindFilesCommand_ShouldDisplayCorrectInfo()
        {
            var type = typeof(FindFilesCommand);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Program.PrintTypeInfo(type);
                var output = sw.ToString();

                Assert.NotEmpty(output);
                Assert.Contains("Класс: FileSystemCommands.FindFilesCommand", output);
                Assert.Contains("Execute", output);
            }
        }
    }
}
