using System;
using System.Collections.Generic;
using System.Text;

namespace task18
{
    public class SoftStop : ICommand
    {
        private readonly int _targetThreadId;
        public SoftStop(int targetThreadId) => _targetThreadId = targetThreadId;
        public void Execute()
        {
            if (Thread.CurrentThread.ManagedThreadId != _targetThreadId)
                throw new InvalidOperationException("SoftStop executed in wrong thread");
        }
    }
}
