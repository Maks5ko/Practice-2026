using System.Threading;

namespace task17
{
    public class HardStopCommand : ICommand
    {
        public Thread TargetThread { get; }

        public HardStopCommand(Thread targetThread)
        {
            TargetThread = targetThread ?? throw new ArgumentNullException(nameof(targetThread));
        }
        public void Execute()
        {
            if (Thread.CurrentThread != TargetThread) throw new InvalidOperationException("HardStop может быть выполнен только в целевом потоке.");
        }
    }
}
