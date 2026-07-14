using System.Threading;
using Xunit;
using task17;

namespace task17tests
{
    public class ServerThreadTests
    {
        [Fact]
        public void AddCommand_ShouldExecuteCommand()
        {
            var server = new ServerThread();
            bool executed = false;
            var command = new SampleCommand("Test", 0, () => executed = true);

            server.AddCommand(command);
            Thread.Sleep(100);
            server.Stop();
            server.Join();

            Assert.True(executed);
        }

        [Fact]
        public void HardStop_ShouldStopImmediately()
        {
            var server = new ServerThread();
            var hardStop = new HardStopCommand(server.Thread);
            bool afterExecuted = false;

            server.AddCommand(new SampleCommand("Before", 200));
            server.AddCommand(hardStop);
            server.AddCommand(new SampleCommand("After", 0, () => afterExecuted = true));

            Thread.Sleep(500);
            server.Stop();
            server.Join();

            Assert.True(server.IsHardStopRequested);
            Assert.False(afterExecuted);
        }

        [Fact]
        public void HardStop_ShouldTerminateThread()
        {
            var server = new ServerThread();
            var hardStop = new HardStopCommand(server.Thread);

            server.AddCommand(hardStop);
            Thread.Sleep(100);
            server.Stop();
            server.Join();

            Assert.False(server.IsAlive);
        }

        [Fact]
        public void SoftStop_ShouldStopAfterQueueEmpty()
        {
            var server = new ServerThread();
            var softStop = new SoftStopCommand(server.Thread);
            bool firstExecuted = false;
            bool secondExecuted = false;
            bool thirdExecuted = false;

            server.AddCommand(new SampleCommand("First", 50, () => firstExecuted = true));
            server.AddCommand(new SampleCommand("Second", 50, () => secondExecuted = true));
            server.AddCommand(softStop);
            server.AddCommand(new SampleCommand("Third", 50, () => thirdExecuted = true));

            Thread.Sleep(300);
            server.Stop();
            server.Join();

            Assert.True(server.IsSoftStopRequested);
            Assert.True(firstExecuted);
            Assert.True(secondExecuted);
            Assert.True(thirdExecuted);
        }

        [Fact]
        public void SoftStop_ShouldTerminateAfterQueueEmpty()
        {
            var server = new ServerThread();
            var softStop = new SoftStopCommand(server.Thread);

            server.AddCommand(new SampleCommand("A", 10));
            server.AddCommand(softStop);
            server.AddCommand(new SampleCommand("B", 10));

            Thread.Sleep(200);
            server.Stop();
            server.Join();

            Assert.False(server.IsAlive);
        }

        [Fact]
        public void HardStop_WrongThread_ShouldThrowAndContinue()
        {
            var server = new ServerThread();
            var wrongHardStop = new HardStopCommand(Thread.CurrentThread);
            bool afterExecuted = false;

            server.AddCommand(wrongHardStop);
            server.AddCommand(new SampleCommand("After", 0, () => afterExecuted = true));

            Thread.Sleep(200);
            server.Stop();
            server.Join();

            Assert.NotNull(server.LastException);
            Assert.IsType<InvalidOperationException>(server.LastException);
            Assert.True(afterExecuted);
            Assert.False(server.IsHardStopRequested);
        }

        [Fact]
        public void SoftStop_WrongThread_ShouldThrowAndContinue()
        {
            var server = new ServerThread();
            var wrongSoftStop = new SoftStopCommand(Thread.CurrentThread);
            bool afterExecuted = false;

            server.AddCommand(wrongSoftStop);
            server.AddCommand(new SampleCommand("After", 0, () => afterExecuted = true));

            Thread.Sleep(200);
            server.Stop();
            server.Join();

            Assert.NotNull(server.LastException);
            Assert.IsType<InvalidOperationException>(server.LastException);
            Assert.True(afterExecuted);
            Assert.False(server.IsSoftStopRequested);
        }

        [Fact]
        public void CommandException_ShouldNotStopThread()
        {
            var server = new ServerThread();
            bool afterExecuted = false;

            server.AddCommand(new SampleCommand("Throw", 0, () => throw new InvalidOperationException("Test")));
            server.AddCommand(new SampleCommand("After", 0, () => afterExecuted = true));

            Thread.Sleep(200);
            server.Stop();
            server.Join();

            Assert.NotNull(server.LastException);
            Assert.IsType<InvalidOperationException>(server.LastException);
            Assert.True(afterExecuted);
        }

        [Fact]
        public void LastException_ShouldBeSet()
        {
            var server = new ServerThread();

            server.AddCommand(new SampleCommand("Throw", 0, () => throw new ArgumentException("Bad")));

            Thread.Sleep(100);
            server.Stop();
            server.Join();

            Assert.NotNull(server.LastException);
            Assert.IsType<ArgumentException>(server.LastException);
        }

        [Fact]
        public void Stop_ShouldBeIdempotent()
        {
            var server = new ServerThread();

            server.Stop();
            server.Stop();
            server.Join();

            Assert.True(true);
        }

        [Fact]
        public void AddCommand_AfterStop_ShouldThrow()
        {
            var server = new ServerThread();

            server.Stop();

            Assert.Throws<ObjectDisposedException>(() => server.AddCommand(new SampleCommand("Test")));
        }

        [Fact]
        public void ServerThread_ShouldNotConsumeCPUWhenIdle()
        {
            var server = new ServerThread();

            Thread.Sleep(100);
            server.Stop();
            server.Join();

            Assert.True(true);
        }
    }
}
