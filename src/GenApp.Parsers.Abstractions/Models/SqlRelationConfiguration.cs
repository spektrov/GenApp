namespace GenApp.Parsers.Abstractions.Models;
public class SqlRelationConfiguration
{
    public string SourceTable { get; set; }

    public string TargetTable { get; set; }

    public IEnumerable<string> SourceColumns { get; set; }

    public IEnumerable<string> TargetColumns { get; set; }

    public bool IsOneToOne { get; set; }

    public bool IsRequired { get; set; }

    public bool HasManyFKToOneTable { get; set; }
}