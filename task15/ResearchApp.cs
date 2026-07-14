using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using task14;
using ScottPlot;

namespace task15
{
    class ResearchApp
    {
        static void Main(string[] args)
        {
            Func<double, double> function = Math.Sin;
            double a = -100, b = 100;
            double targetPrecision = 1e-4;
            double[] steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
            int[] threadCounts = { 1, 2, 4, 8, 16, 32 };
            var results = new List<Result>();

            Console.WriteLine("Исследование оптимальных параметров");
            Console.WriteLine($"Функция: sin(x), отрезок: [{a}, {b}], точность: {targetPrecision}\n");

            double optimalStep = FindOptimalStep(a, b, function, steps, targetPrecision);
            Console.WriteLine($"Оптимальный шаг: {optimalStep}\n");

            foreach (int threads in threadCounts)
            {
                double time = MeasureTime(() => DefiniteIntegral.Solve(a, b, function, optimalStep, threads), 10);
                results.Add(new Result { Threads = threads, TimeMs = time });
                Console.WriteLine($"Потоков: {threads}, время: {time:F2} мс");
            }

            var optimal = results.OrderBy(r => r.TimeMs).First();
            Console.WriteLine($"\nОптимальное число потоков: {optimal.Threads}, время: {optimal.TimeMs:F2} мс");

            double singleThreadTime = MeasureTime(() => SolveSingleThread(a, b, function, optimalStep), 10);
            double speedup = singleThreadTime / optimal.TimeMs;
            double percentDiff = (singleThreadTime - optimal.TimeMs) / singleThreadTime * 100;

            Console.WriteLine("\nСравнение с однопоточным вариантом");
            Console.WriteLine($"Однопоточное время: {singleThreadTime:F2} мс");
            Console.WriteLine($"Многопоточное время: {optimal.TimeMs:F2} мс");
            Console.WriteLine($"Ускорение: {speedup:F2}x");
            Console.WriteLine($"Разница: {percentDiff:F2}%");

            SaveResults(optimalStep, optimal, singleThreadTime, speedup, percentDiff, results);
            PlotResults(results, optimal);
        }

        static double FindOptimalStep(double a, double b, Func<double, double> function, double[] steps, double targetPrecision)
        {
            double reference = DefiniteIntegral.Solve(a, b, function, 1e-7, 8);

            foreach (var step in steps)
            {
                double value = DefiniteIntegral.Solve(a, b, function, step, 8);
                double error = Math.Abs(value - reference);

                Console.WriteLine($"Шаг: {step}, значение: {value:F6}, ошибка: {error:E4}");

                if (error < targetPrecision) return step;
            }

            return steps.Last();
        }

        static double MeasureTime(Action action, int repeats = 10)
        {
            var sw = new Stopwatch();
            double total = 0;

            for (int i = 0; i < repeats; i++)
            {
                sw.Restart();
                action();
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }

            return total / repeats;
        }

        static double SolveSingleThread(double a, double b, Func<double, double> function, double step)
        {
            int totalSteps = (int)Math.Ceiling((b - a) / step);
            if (totalSteps == 0) return 0;

            double h = step;
            double sum = 0;
            double x = a;
            double fx = function(x);

            for (int i = 0; i < totalSteps; i++)
            {
                double nextX = a + (i + 1) * h;
                double nextFx = function(nextX);
                sum += (fx + nextFx) * h / 2.0;
                fx = nextFx;
            }
            return sum;
        }

        static void PlotResults(List<Result> results, Result optimal)
        {
            var plt = new Plot();

            double[] x = results.Select(r => r.TimeMs).ToArray();
            double[] y = results.Select(r => (double)r.Threads).ToArray();

            var scatter = plt.Add.Scatter(x, y);
            scatter.LineWidth = 2;
            scatter.MarkerSize = 10;

            plt.Title("Зависимость времени выполнения от числа потоков");
            plt.XLabel("Время, мс");
            plt.YLabel("Количество потоков");

            plt.Add.Marker(optimal.TimeMs, optimal.Threads, shape: MarkerShape.FilledDiamond, size: 15);

            double marginX = 0.05 * (x.Max() - x.Min());
            if (marginX == 0) marginX = 0.1;
            double xMin = x.Min() - marginX;
            double xMax = x.Max() + marginX;
            double yMin = 0;
            double yMax = results.Max(r => r.Threads) + 2;

            plt.Axes.SetLimits(xMin, xMax, yMin, yMax);

            plt.SavePng("plot.png", 800, 600);
            Console.WriteLine("График сохранён как plot.png");
        }

        static void SaveResults(double step, Result optimal, double singleThreadTime, double speedup, double percentDiff, List<Result> allResults)
        {
            using (var writer = new StreamWriter("results.txt"))
            {
                writer.WriteLine("Результаты исследования");
                writer.WriteLine($"Функция: sin(x)");
                writer.WriteLine($"Отрезок: [-100, 100]");
                writer.WriteLine($"Точность: 1e-4");
                writer.WriteLine();
                writer.WriteLine($"Оптимальный шаг: {step}");
                writer.WriteLine($"Оптимальное число потоков: {optimal.Threads}");
                writer.WriteLine($"Время выполнения (многопоточный): {optimal.TimeMs:F2} мс");
                writer.WriteLine($"Время выполнения (однопоточный): {singleThreadTime:F2} мс");
                writer.WriteLine($"Ускорение: {speedup:F2}x");
                writer.WriteLine($"Разница в процентах: {percentDiff:F2}%");
                writer.WriteLine();
                writer.WriteLine("Все замеры:");
                foreach (var r in allResults) writer.WriteLine($"  Потоков: {r.Threads}, время: {r.TimeMs:F2} мс");
                writer.WriteLine();
                writer.WriteLine($"Дата выполнения: {DateTime.Now}");
            }

            Console.WriteLine("Результаты сохранены в results.txt");
        }
    }

    class Result
    {
        public int Threads { get; set; }
        public double TimeMs { get; set; }
    }
}
