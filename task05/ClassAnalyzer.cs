using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
namespace task05
{
    public class ClassAnalyzer
    {
        private Type _type;
        public ClassAnalyzer(Type type)
        {
            _type = type;
        }
        public IEnumerable<string> GetPublicMethods()
        {
            return _type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Select(m => m.Name).Distinct();
        }
        public IEnumerable<string> GetMethodParams(string methodname)
        {
            if (string.IsNullOrEmpty(methodname))
                return Enumerable.Empty<string>();

            var method = _type.GetMethod(methodname, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            if (method == null)
                return Enumerable.Empty<string>();

            var paramInfos = method.GetParameters();
            var result = paramInfos.Select(p => $"{p.ParameterType.Name} {p.Name}").ToList();
            result.Add($"returns {method.ReturnType.Name}");
            return result;
        }
        public IEnumerable<string> GetAllFields()
        {
            return _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Select(f => f.Name);
        }
        public IEnumerable<string> GetProperties()
        {
            return _type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Select(p => p.Name);
        }
        public bool HasAttribute<T>() where T : Attribute
        {
            return _type.GetCustomAttributes(typeof(T), true).Any();
        }
    }
}
