using System.ComponentModel.DataAnnotations;

namespace GenApp.Templates.Abstractions;
public interface ICsharpClassModel
{
    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; set; }
}
