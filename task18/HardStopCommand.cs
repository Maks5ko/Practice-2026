namespace task18
{
    public class HardStop : ICommand
    {
        private readonly int _targetThreadId;
        public HardStop(int targetThreadId) => _targetThreadId = targetThreadId;
        public void Execute()
        {
            if (Thread.CurrentThread.ManagedThreadId != _targetThreadId)
                throw new InvalidOperationException("HardStop executed in wrong thread");
        }
    }
}
