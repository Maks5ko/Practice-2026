using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLib;

namespace CommandRunner
{
    class CommandRunner
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Использование: CommandRunner <path_to_dll>");
                Console.WriteLine("Пример   : CommandRunner FileSystemCommands.dll");
                return;
            }

            string dllPath = args[0];

            if (!File.Exists(dllPath))
            {
                Console.WriteLine($"Файл '{dllPath}' не найден.");
                return;
            }

            try
            {
                var assembly = Assembly.LoadFrom(dllPath);
                Console.WriteLine($"Загружена сборка: {assembly.FullName}");

                var commandTypes = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && typeof(ICommand).IsAssignableFrom(t));

                Console.WriteLine($"Найдено команд: {commandTypes.Count()}");
                Console.WriteLine();

                foreach (var type in commandTypes)
                {
                    Console.WriteLine($"Команда: {type.Name}");

                    object instance = null;

                    var constructors = type.GetConstructors();

                    foreach (var ctor in constructors)
                    {
                        var parameters = ctor.GetParameters();
                        try
                        {
                            var paramValues = new object[parameters.Length];
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                var paramType = parameters[i].ParameterType;
                                if (paramType == typeof(string))
                                    paramValues[i] = ".";
                                else if (paramType == typeof(int))
                                    paramValues[i] = 0;
                                else if (paramType.IsValueType)
                                    paramValues[i] = Activator.CreateInstance(paramType);
                                else
                                    paramValues[i] = null;
                            }
                            instance = ctor.Invoke(paramValues);
                            break;
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    if (instance == null)
                    {
                        try
                        {
                            instance = Activator.CreateInstance(type);
                        }
                        catch
                        {
                            Console.WriteLine($"Не удалось создать экземпляр {type.Name} с параметрами по умолчанию");
                            continue;
                        }
                    }

                    if (instance is ICommand command)
                    {
                        Console.WriteLine("  Выполнение...");
                        command.Execute();
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.WriteLine($"Стек вызовов: {ex.StackTrace}");
            }

            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
