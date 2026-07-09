using PluginLib;

namespace task10tests
{
    public class FakePlugin : IPlugin
    {
        public string Name => "FakePlugin";
        public void Execute() { }
    }
}
