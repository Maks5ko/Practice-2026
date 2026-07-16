using System.Collections.Concurrent;
using System.Threading;
using task17;
using task19;
namespace task18
{
    public class ServerThread
    {
        private readonly Thread _thread;
        private readonly BlockingCollection<ICommand> _commandQueue;
        private readonly IScheduler _scheduler;
        private volatile bool _isHardStopRequested;
        private volatile bool _isSoftStopRequested;
        private Exception _lastException;

        public Thread Thread => _thread;
        public bool IsAlive => _thread.IsAlive;
        public bool IsHardStopRequested => _isHardStopRequested;
        public bool IsSoftStopRequested => _isSoftStopRequested;
        public Exception LastException => _lastException;

        public ServerThread(IScheduler scheduler)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
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
                while (!_isHardStopRequested)
                {
                    if (_scheduler.HasCommand())
                    {
                        var cmd = _scheduler.Select();
                        if (cmd != null)
                        {
                            try
                            {
                                cmd.Execute();
                                if (cmd is HardStopCommand)
                                {
                                    _isHardStopRequested = true;
                                    break;
                                }
                                else if (cmd is SoftStopCommand) _isSoftStopRequested = true;    
                            }
                            catch (Exception ex)
                            {
                                _lastException = ex;
                                Console.WriteLine($"Исключение в команде: {ex.Message}");
                            }
                        }
                        continue;
                    }
                    if (_commandQueue.TryTake(out var newCommand, 10)) _scheduler.Add(newCommand);
                    else Thread.Sleep(1);
                    
                    if (_isSoftStopRequested && _commandQueue.Count == 0 && !_scheduler.HasCommand()) break;
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