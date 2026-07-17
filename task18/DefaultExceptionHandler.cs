using System;
using System.Collections.Generic;
using System.Text;

namespace task18
{
    public class DefaultExceptionHandler : IExceptionHandler
    {
        public void Handle(ICommand command, Exception exception) { }
    }
}
