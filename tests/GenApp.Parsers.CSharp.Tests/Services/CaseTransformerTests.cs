using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Parsers.CSharp.Services;

namespace GenApp.Parsers.CSharp.Tests.Services;
public class CaseTransformerTests
{
    private readonly ICaseTransformer _sut;

    public CaseTransformerTests()
    {
        _sut = new CaseTransformer();
    }

    [Theory]
    [InlineData("someString", "SomeString")]
    [InlineData("somestring", "Somestring")]
    [InlineData("SomeString", "SomeString")]
    [InlineData("some_string", "SomeString")]
    [InlineData("Some_String", "SomeString")]
    public void ToPascalCase_should_return_pascal(string input, string output)
    {
        var actual = _sut.ToPascalCase(input);

        actual.Should().BeEquivalentTo(output);
    }

    [Theory]
    [InlineData("someString", "someString")]
    [InlineData("somestring", "somestring")]
    [InlineData("SomeString", "someString")]
    [InlineData("some_string", "someString")]
    [InlineData("Some_String", "someString")]
    public void ToCamelCase_should_return_camel(string input, string output)
    {
        var actual = _sut.ToCamelCase(input);

        actual.Should().BeEquivalentTo(output);
    }

    [Theory]
    [InlineData("someString", "_someString")]
    [InlineData("somestring", "_somestring")]
    [InlineData("SomeString", "_someString")]
    [InlineData("some_string", "_someString")]
    [InlineData("Some_String", "_someString")]
    public void ToCamelUnderscoreCase_should_return_camel_underscore(string input, string output)
    {
        var actual = _sut.ToCamelUnderscoreCase(input);

        actual.Should().BeEquivalentTo(output);
    }

    [Theory]
    [InlineData("someString", "some_string")]
    [InlineData("somestring", "somestring")]
    [InlineData("SomeString", "some_string")]
    [InlineData("some_string", "some_string")]
    [InlineData("Some_String", "some_string")]
    public void ToSnakeCase_should_return_snake(string input, string output)
    {
        var actual = _sut.ToSnakeCase(input);

        actual.Should().BeEquivalentTo(output);
    }

    [Theory]
    [InlineData("apple", "apples")]
    [InlineData("apples", "apples")]
    [InlineData("person", "people")]
    [InlineData("red_apple", "red_apples")]
    [InlineData("RedApple", "red_apples")]
    public void ToPlural_should_return_plural(string input, string output)
    {
        var actual = _sut.ToPlural(input);

        actual.Should().BeEquivalentTo(output);
    }

    [Theory]
    [InlineData("apple", "apple")]
    [InlineData("apples", "apple")]
    [InlineData("people", "person")]
    [InlineData("red_apples", "red_apple")]
    [InlineData("RedApples", "red_apple")]
    public void ToSingular_should_return_singular(string input, string output)
    {
        var actual = _sut.ToSignular(input);

        actual.Should().BeEquivalentTo(output);
    }
}
