namespace task18
{
    public class WorkCommand : ICommand
    {
        private readonly int _totalWork;
        private readonly int _workPerTick;
        private int _completed;
        private readonly string _name;

        public WorkCommand(string name, int totalWork, int workPerTick)
        {
            _name = name;
            _totalWork = totalWork;
            _workPerTick = workPerTick;
            _completed = 0;
        }

        public bool IsCompleted => _completed >= _totalWork;

        public void Execute()
        {
            if (IsCompleted) return;
            int remaining = _totalWork - _completed;
            int toDo = Math.Min(_workPerTick, remaining);

            long sum = 0;
            for (int i = 0; i < 10000 * toDo; i++)
            {
                sum += i;
            }
            if (sum < 0) sum = -sum;

            _completed += toDo;
        }
    }
}
