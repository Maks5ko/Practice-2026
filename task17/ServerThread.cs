using System.Collections.Concurrent;
using System.Threading;

namespace task17
{
    public class ServerThread
    {
        private readonly Thread _thread;
        private readonly BlockingCollection<ICommand> _commandQueue;
        private volatile bool _isHardStopRequested;
        private volatile bool _isSoftStopRequested;
        private Exception _lastException;

        public Thread Thread => _thread;
        public bool IsAlive => _thread.IsAlive;
        public bool IsHardStopRequested => _isHardStopRequested;
        public bool IsSoftStopRequested => _isSoftStopRequested;
        public Exception LastException => _lastException;

        public ServerThread()
        {
            _commandQueue = new BlockingCollection<ICommand>(new ConcurrentQueue<ICommand>());
            _thread = new Thread(ProcessCommands);
            _thread.Start();
        }

        public void AddCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            _commandQueue.Add(command);
        }

        public void Stop()
        {
            try
            {
                _commandQueue.CompleteAdding();
            }
            catch (ObjectDisposedException) { }
        }

        public void Join() => _thread.Join();

        private void ProcessCommands()
        {
            try
            {
                foreach (var command in _commandQueue.GetConsumingEnumerable())
                {
                    if (_isHardStopRequested)
                        break;

                    try
                    {
                        command.Execute();

                        if (command is HardStopCommand)
                        {
                            _isHardStopRequested = true;
                            break;
                        }
                        else if (command is SoftStopCommand)
                        {
                            _isSoftStopRequested = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        _lastException = ex;
                        Console.WriteLine($"Исключение в команде: {ex.Message}");
                    }

                    if (_isSoftStopRequested && _commandQueue.Count == 0) break;
                }
            }
            catch (OperationCanceledException) { }
            finally
            {
                _commandQueue.Dispose();
            }
        }
    }
}
