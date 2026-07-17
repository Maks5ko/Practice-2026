namespace task18
{
    public class TestCommand : ICommand
    {
        private readonly int _id;
        private int _counter;
        private readonly int _maxExecutions;

        public TestCommand(int id, int maxExecutions = 3)
        {
            _id = id;
            _maxExecutions = maxExecutions;
            _counter = 0;
        }

        public bool IsCompleted => _counter >= _maxExecutions;

        public void Execute()
        {
            if (IsCompleted) return;
            _counter++;
            Console.WriteLine($"Поток {_id} вызов {_counter}");
        }
    }
}
