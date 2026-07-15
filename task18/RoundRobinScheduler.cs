using System.Collections.Generic;
using System.Threading;

namespace task18
{
    public class RoundRobinScheduler : IScheduler
    {
        private readonly List<ICommand> _commands = new List<ICommand>();
        private int _currentIndex = 0;
        private readonly object _lock = new object();

        public void Add(ICommand cmd)
        {
            if (cmd == null) throw new System.ArgumentNullException(nameof(cmd));
            lock (_lock)
            {
                _commands.Add(cmd);
            }
        }
        public bool HasCommand()
        {
            lock (_lock)
            {
                _commands.RemoveAll(c => c.IsCompleted);
                return _commands.Count > 0;
            }
        }
        public ICommand Select()
        {
            lock (_lock)
            {
                _commands.RemoveAll(c => c.IsCompleted);
                if (_commands.Count == 0) return null;
                if (_currentIndex >= _commands.Count) _currentIndex = 0;

                var selected = _commands[_currentIndex];
                _currentIndex = (_currentIndex + 1) % _commands.Count;
                return selected;
            }
        }
    }
}
