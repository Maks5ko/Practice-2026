using System.Threading;
using System.Threading.Tasks;

namespace task14
{
    public static class DefiniteIntegral
    {
        public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
        {
            if (threadsNumber <= 0) throw new ArgumentOutOfRangeException(nameof(threadsNumber), "Число потоков должно быть положительным.");
            if (step <= 0) throw new ArgumentOutOfRangeException(nameof(step), "Шаг должен быть положительным.");
            if (b <= a) throw new ArgumentException("Правая граница должна быть больше левой.");

            if (step >= (b - a)) return 0;

            int totalSteps = (int)Math.Ceiling((b - a) / step);
            if (totalSteps == 0) return 0;

            double total = 0;
            Barrier barrier = new Barrier(threadsNumber + 1);

            Thread[] threads = new Thread[threadsNumber];

            for (int i = 0; i < threadsNumber; i++)
            {
                int threadIndex = i;
                threads[i] = new Thread(() =>
                    {
                        int startStep = threadIndex * totalSteps / threadsNumber;
                        int endStep = (threadIndex + 1) * totalSteps / threadsNumber;

                        if (startStep >= endStep)
                        {
                            barrier.SignalAndWait();
                            return;
                        }

                        double localSum = 0;
                        double h = step;

                        double x = a + startStep * h;
                        double fx = function(x);

                        for (int s = startStep; s < endStep; s++)
                        {
                            double nextX = a + (s + 1) * h;
                            double nextFx = function(nextX);
                            localSum += (fx + nextFx) * h / 2.0;
                            fx = nextFx;
                        }
                        double oldTotal, newTotal;
                        do
                        {
                            oldTotal = total;
                            newTotal = oldTotal + localSum;
                        } while (Interlocked.CompareExchange(ref total, newTotal, oldTotal) != oldTotal);
                        barrier.SignalAndWait();
                    });

                threads[i].Start();
            }
            barrier.SignalAndWait();
            barrier.Dispose();
            return total;
        }
    }
}
