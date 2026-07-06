using Xunit;
using task05;
public class TestClass
{
    public int PublicField;
    private string _privateField;
    public int Property { get; set; }

    public void Method() { }
    public int MethodWithParams(string param1, double param2, int param3) => 0;
    private void PrivateMethod() { }
}

[Serializable]
public class AttributedClass { }
public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Contains("Method", methods);
    }
    [Fact]
    public void GetMethodParams_ForMethodWithParameters()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var paramsInfo = analyzer.GetMethodParams("MethodWithParams").ToList();

        Assert.Equal(4, paramsInfo.Count);
        Assert.Equal("String param1", paramsInfo[0]);
        Assert.Equal("Double param2", paramsInfo[1]);
        Assert.Equal("Int32 param3", paramsInfo[2]);
        Assert.Equal("returns Int32", paramsInfo[3]);
    }
    [Fact]
    public void GetMethodParams_ForMethodWithoutParameters()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var paramsInfo = analyzer.GetMethodParams("Method").ToList();

        Assert.Single(paramsInfo);
        Assert.Equal("returns Void", paramsInfo[0]);
    }
    [Fact]
    public void GetMethodParams_ForNoMethod()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var result = analyzer.GetMethodParams("NoMethod");
        Assert.Empty(result);
    }
    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
    }
    [Fact]
    public void GetProperties_ReturnsPublicProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties();

        Assert.Contains("Property", properties);
        Assert.DoesNotContain("PublicField", properties);
    }
    [Fact]
    public void HasAttribute_WhenAttribute()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        Assert.True(analyzer.HasAttribute<SerializableAttribute>());
    }
    [Fact]
    public void HasAttribute_WhenNoAttribute()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        Assert.False(analyzer.HasAttribute<SerializableAttribute>());
    }
}
