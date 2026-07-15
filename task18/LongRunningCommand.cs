namespace task18
{
    public class LongRunningCommand : ICommand
    {
        private readonly int _totalSteps;
        private int _currentStep;
        private readonly string _name;

        public LongRunningCommand(string name, int steps = 5)
        {
            _name = name;
            _totalSteps = steps;
            _currentStep = 0;
        }

        public bool IsCompleted => _currentStep >= _totalSteps;

        public void Execute()
        {
            if (IsCompleted) return;
            _currentStep++;
            Console.WriteLine($"Command '{_name}' step {_currentStep}/{_totalSteps} executed.");
        }
    }
}
