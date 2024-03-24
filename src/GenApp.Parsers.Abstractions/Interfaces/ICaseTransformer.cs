namespace GenApp.Parsers.Abstractions.Interfaces;
public interface ICaseTransformer
{
    string ToCamelCase(string str);

    string ToCamelUnderscoreCase(string str);

    string ToSnakeCase(string str);

    string ToPascalCase(string str);

    string ToPlural(string str);
}