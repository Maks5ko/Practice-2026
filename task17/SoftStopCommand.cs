using System.Threading;

namespace task17
{
    public class SoftStopCommand : ICommand
    {
        public Thread TargetThread { get; }

        public SoftStopCommand(Thread targetThread)
        {
            TargetThread = targetThread ?? throw new ArgumentNullException(nameof(targetThread));
        }
        public bool IsCompleted => true;
        public void Execute()
        {
            if (Thread.CurrentThread != TargetThread) throw new InvalidOperationException("SoftStop может быть выполнен только в целевом потоке.");
        }
    }
}
