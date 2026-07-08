using System.Linq;
using System.Reflection;
using CommandLib;

namespace MetadataViewer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Использование: MetadataViewer <путь_к_dll>");
                Console.WriteLine("Пример: MetadataViewer FileSystemCommands.dll");
                return;
            }

            string dllPath = args[0];
            if (!File.Exists(dllPath))
            {
                Console.WriteLine($"Файл не найден: {dllPath}");
                return;
            }

            try
            {
                Assembly assembly = Assembly.LoadFrom(dllPath);
                PrintAssemblyInfo(assembly);
                PrintAllTypes(assembly);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        public static void PrintAssemblyInfo(Assembly assembly)
        {
            Console.WriteLine("Информация о сборке");
            Console.WriteLine($"Имя: {assembly.FullName}");
            Console.WriteLine($"Расположение: {assembly.Location}");
            Console.WriteLine($"Версия: {assembly.GetName().Version}");
            Console.WriteLine();
        }

        public static void PrintAllTypes(Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .OrderBy(t => t.Name)
                .ToList();

            Console.WriteLine($"Классы ({types.Count})");
            Console.WriteLine();

            foreach (var type in types)
            {
                PrintTypeInfo(type);
                Console.WriteLine(new string('-', 60));
            }
        }

        public static void PrintTypeInfo(Type type)
        {
            Console.WriteLine($"Класс: {type.FullName}");
            Console.WriteLine($"Пространство имён: {type.Namespace}");

            var classAttrs = type.GetCustomAttributes();
            if (classAttrs.Any())
            {
                Console.WriteLine("Атрибуты класса:");
                foreach (var attr in classAttrs)
                {
                    string attrName = attr.GetType().Name;
                    if (attr is DisplayNameAttribute display) Console.WriteLine($"  - {attrName}: {display.DisplayName}");
                    else if (attr is VersionAttribute version) Console.WriteLine($"  - {attrName}: {version.Major}.{version.Minor}");
                    else Console.WriteLine($"  - {attrName}");
                }
            }

            var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Any())
            {
                Console.WriteLine("Конструкторы:");
                foreach (var ctor in constructors)
                {
                    var parameters = ctor.GetParameters();
                    string paramStr = parameters.Length > 0 ? string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}")) : "без параметров";
                    Console.WriteLine($"  - {type.Name}({paramStr})");
                }
            }

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .OrderBy(m => m.Name);
            if (methods.Any())
            {
                Console.WriteLine("Методы:");
                foreach (var method in methods)
                {
                    var methodAttr = method.GetCustomAttribute<DisplayNameAttribute>();
                    string attrInfo = methodAttr != null ? $" [DisplayName: {methodAttr.DisplayName}]" : "";

                    var parameters = method.GetParameters();
                    string paramStr = parameters.Length > 0
                        ? string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}")) : "без параметров";

                    Console.WriteLine($"  - {method.ReturnType.Name} {method.Name}({paramStr}){attrInfo}");
                }
            }
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (properties.Any())
            {
                Console.WriteLine("Свойства:");
                foreach (var prop in properties)
                {
                    var propAttr = prop.GetCustomAttribute<DisplayNameAttribute>();
                    string attrInfo = propAttr != null ? $" [DisplayName: {propAttr.DisplayName}]" : "";
                    Console.WriteLine($"  - {prop.PropertyType.Name} {prop.Name}{attrInfo}");
                }
            }

            Console.WriteLine();
        }
    }
}
