using System.Reflection;
using Xunit;
using task07;

public class AttributeReflectionTests
{
    [Fact]
    public void Class_HasDisplayNameAttribute()
    {
        var type = typeof(SampleClass);
        var attribute = type.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Пример класса", attribute.DisplayName);
    }

    [Fact]
    public void Method_HasDisplayNameAttribute()
    {
        var method = typeof(SampleClass).GetMethod("TestMethod");
        var attribute = method.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Тестовый метод", attribute.DisplayName);
    }

    [Fact]
    public void Property_HasDisplayNameAttribute()
    {
        var prop = typeof(SampleClass).GetProperty("Number");
        var attribute = prop.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Числовое свойство", attribute.DisplayName);
    }

    [Fact]
    public void Class_HasVersionAttribute()
    {
        var type = typeof(SampleClass);
        var attribute = type.GetCustomAttribute<VersionAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Major);
        Assert.Equal(0, attribute.Minor);
    }
    [Fact]
    public void PrintTypeInfo_ReturnsCorrectInfo()
    {
        var type = typeof(SampleClass);
        var info = ReflectionHelper.PrintTypeInfo(type).ToList();

        Assert.Contains("Class display name: Пример класса", info);
        Assert.Contains("Version: 1.0", info);
        Assert.Contains("Methods:", info);
        Assert.Contains("  TestMethod (DisplayName: Тестовый метод)", info);
        Assert.Contains("Properties:", info);
        Assert.Contains("  Number (DisplayName: Числовое свойство)", info);
    }
}
