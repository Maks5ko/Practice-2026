using System.Linq;
using System.Threading;
using Xunit;
using task18;

namespace task18tests
{
    public class RoundRobinSchedulerTests
    {
        [Fact]
        public void Add_And_HasCommand_Works()
        {
            var scheduler = new RoundRobinScheduler();
            Assert.False(scheduler.HasCommand());

            var cmd = new LongRunningCommand("A", 3);
            scheduler.Add(cmd);
            Assert.True(scheduler.HasCommand());
        }

        [Fact]
        public void Select_Returns_Commands_In_RoundRobin_Order()
        {
            var scheduler = new RoundRobinScheduler();
            var cmdA = new LongRunningCommand("A", 5);
            var cmdB = new LongRunningCommand("B", 5);
            var cmdC = new LongRunningCommand("C", 5);

            scheduler.Add(cmdA);
            scheduler.Add(cmdB);
            scheduler.Add(cmdC);

            var first = scheduler.Select();
            Assert.Same(cmdA, first);

            var second = scheduler.Select();
            Assert.Same(cmdB, second);

            var third = scheduler.Select();
            Assert.Same(cmdC, third);

            var fourth = scheduler.Select();
            Assert.Same(cmdA, fourth);
        }

        [Fact]
        public void Select_Removes_Completed_Commands()
        {
            var scheduler = new RoundRobinScheduler();
            var shortCmd = new LongRunningCommand("Short", 1);
            var longCmd = new LongRunningCommand("Long", 3);

            scheduler.Add(shortCmd);
            scheduler.Add(longCmd);

            shortCmd.Execute();
            var selected = scheduler.Select();
            Assert.Same(longCmd, selected);

            scheduler.Select();
            Assert.True(scheduler.HasCommand());

            longCmd.Execute();
            longCmd.Execute();
            longCmd.Execute();
            scheduler.HasCommand();
            Assert.False(scheduler.HasCommand());
        }

        [Fact]
        public void Scheduler_Is_ThreadSafe()
        {
            var scheduler = new RoundRobinScheduler();
            const int commandCount = 100;
            const int stepsPerCommand = 10;

            var threads = new Thread[4];
            for (int t = 0; t < threads.Length; t++)
            {
                threads[t] = new Thread(() =>
                {
                    for (int i = 0; i < commandCount / threads.Length; i++)
                    {
                        var cmd = new LongRunningCommand($"Cmd-{t}-{i}", stepsPerCommand);
                        scheduler.Add(cmd);
                    }
                });
                threads[t].Start();
            }
            foreach (var th in threads) th.Join();
            int totalExecuted = 0;
            while (scheduler.HasCommand())
            {
                var cmd = scheduler.Select();
                if (cmd != null)
                {
                    cmd.Execute();
                    totalExecuted++;
                }
            }
            Assert.Equal(commandCount * stepsPerCommand, totalExecuted);
        }
            
        [Fact]
        public void Select_WhenNoCommands_ReturnsNull()
        {
            var scheduler = new RoundRobinScheduler();
            var result = scheduler.Select();
            Assert.Null(result);
        }

        [Fact]
        public void HasCommand_ReturnsFalse_AfterAllCommandsCompleted()
        {
            var scheduler = new RoundRobinScheduler();
            var cmd = new LongRunningCommand("A", 2);
            scheduler.Add(cmd);

            cmd.Execute(); 
            cmd.Execute(); 

            Assert.False(scheduler.HasCommand());
        }
    }
}
