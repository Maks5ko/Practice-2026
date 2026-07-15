using ScottPlot;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using task18;

namespace task18research
{
    class Program
    {
        static void Main(string[] args)
        {
            const int commandCount = 50;
            const int totalWorkPerCommand = 2000;
            int[] quantumSizes = { 1, 2, 5, 10, 20, 50, 100, 150, 200 };
            const int repeats = 10;

            var results = new List<ExperimentResult>();

            Console.WriteLine("Исследование влияния кванта на производительность планировщика");
            Console.WriteLine($"Команд: {commandCount}, работа на команду: {totalWorkPerCommand}\n");

            foreach (int quantum in quantumSizes)
            {
                double totalTimeMs = 0;

                for (int r = 0; r < repeats; r++)
                {
                    var commands = new List<ICommand>();
                    for (int i = 0; i < commandCount; i++)
                    {
                        commands.Add(new WorkCommand($"Cmd{i}", totalWorkPerCommand, quantum));
                    }

                    var scheduler = new RoundRobinScheduler();
                    foreach (var cmd in commands) scheduler.Add(cmd);

                    var sw = Stopwatch.StartNew();

                    while (scheduler.HasCommand())
                    {
                        var cmd = scheduler.Select();
                        if (cmd != null) cmd.Execute();
                    }

                    sw.Stop();
                    totalTimeMs += sw.Elapsed.TotalMilliseconds;
                }

                double avgTime = totalTimeMs / repeats;
                results.Add(new ExperimentResult { Quantum = quantum, TimeMs = avgTime });
                Console.WriteLine($"Квант: {quantum}, среднее время: {avgTime:F2} мс");
            }

            SaveResults(results);
            PlotResults(results);

            Console.WriteLine("\nГрафик сохранён как plot.png, данные в results.csv");
        }

        static void SaveResults(List<ExperimentResult> results)
        {
            using (var writer = new StreamWriter("results.csv"))
            {
                writer.WriteLine("Quantum;TimeMs");
                foreach (var r in results) writer.WriteLine($"{r.Quantum};{r.TimeMs.ToString(CultureInfo.InvariantCulture)}");
            }
        }

        static void PlotResults(List<ExperimentResult> results)
        {
            var plt = new Plot();
            double[] x = results.Select(r => (double)r.Quantum).ToArray();
            double[] y = results.Select(r => r.TimeMs).ToArray();

            var scatter = plt.Add.Scatter(x, y);
            scatter.LineWidth = 2;
            scatter.MarkerSize = 10;

            plt.Title("Зависимость времени выполнения от размера кванта");
            plt.XLabel("Размер кванта (единиц работы)");
            plt.YLabel("Общее время выполнения, мс");

            plt.SavePng("plot.png", 800, 600);
        }
    }

    class ExperimentResult
    {
        public int Quantum { get; set; }
        public double TimeMs { get; set; }
    }
}
