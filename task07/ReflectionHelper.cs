using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace task07
{
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
