namespace GenApp.Domain.Models;
public class DotnetEntityRelationModel
{
    public string SourceEntity { get; set; }

    public string TargetEntity { get; set; }

    public bool IsOneToOne { get; set; }

    public bool IsRequired { get; set; }

    public bool IsReverted { get; set; }
}
