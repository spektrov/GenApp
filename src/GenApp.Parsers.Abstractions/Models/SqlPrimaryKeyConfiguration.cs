namespace GenApp.Parsers.Abstractions.Models;
public class SqlPrimaryKeyConfiguration
{
    required public IEnumerable<string> SourceColumns { get; set; }

    public bool HasPK => SourceColumns.Any();

    public bool IsComposite => SourceColumns.Count() > 1;
}