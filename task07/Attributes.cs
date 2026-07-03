using System.Reflection;

namespace task07
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class DisplayNameAttribute : Attribute
    {
        public string DisplayName { get; }
        public DisplayNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class VersionAttribute : Attribute
    {
        public int Major { get; }
        public int Minor { get; }
        public VersionAttribute(int major, int minor)
        {
            (Major, Minor) = (major, minor);
        }
    }
    [DisplayName("Пример класса")]
    [Version(1, 0)]
    public class SampleClass
    {
        [DisplayName("Числовое свойство")]
        public int Number { get; set; }

        [DisplayName("Тестовый метод")]
        public void TestMethod() { }
    }
    public static class ReflectionHelper
    {
        public static IEnumerable<string> PrintTypeInfo(Type type)
        {
            var result = new List<string>();

            var displayAttr = type.GetCustomAttribute<DisplayNameAttribute>();
            result.Add(displayAttr != null
                ? $"Class display name: {displayAttr.DisplayName}"
                : "Class display name: (none)");

            var versionAttr = type.GetCustomAttribute<VersionAttribute>();
            result.Add(versionAttr != null
                ? $"Version: {versionAttr.Major}.{versionAttr.Minor}"
                : "Version: (none)");

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            result.Add("Methods:");
            foreach (var m in methods)
            {
                var attr = m.GetCustomAttribute<DisplayNameAttribute>();
                result.Add(attr != null
                    ? $"  {m.Name} (DisplayName: {attr.DisplayName})"
                    : $"  {m.Name}");
            }

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            result.Add("Properties:");
            foreach (var p in properties)
            {
                var attr = p.GetCustomAttribute<DisplayNameAttribute>();
                result.Add(attr != null
                    ? $"  {p.Name} (DisplayName: {attr.DisplayName})"
                    : $"  {p.Name}");
            }
            return result;
        }
    }
}
