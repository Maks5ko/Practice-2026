namespace task17
{
    public class SampleCommand : ICommand
    {
        private readonly string _name;
        private readonly int _delayMs;
        private readonly Action _onExecute;

        public SampleCommand(string name, int delayMs = 0, Action onExecute = null)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _delayMs = delayMs;
            _onExecute = onExecute;
        }

        public void Execute()
        {
            if (_delayMs > 0) Thread.Sleep(_delayMs);
            _onExecute?.Invoke();
            Console.WriteLine($"Выполнена команда: {_name}");
        }
    }
}
