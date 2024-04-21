using GenApp.Domain.Models;
using GenApp.Parsers.Abstractions.Interfaces;
using GenApp.Parsers.Abstractions.Models;
using GenApp.Parsers.CSharp.Interfaces;

namespace GenApp.Parsers.CSharp.Mappers;
public class DotnetRelationMapper(ICaseTransformer caseTransformer) : IDotnetRelationMapper
{
    public DotnetEntityRelationModel? Map(SqlRelationConfiguration? sqlRelation)
    {
        if (sqlRelation is null) return null;

        var relation = new DotnetEntityRelationModel
        {
            SourceEntity = ToDotnetName(sqlRelation.SourceTable),
            TargetEntity = ToDotnetName(sqlRelation.TargetTable),
            IsOneToOne = sqlRelation.IsOneToOne,
            IsRequired = sqlRelation.IsRequired,
        };

        return relation;
    }

    public DotnetEntityRelationModel? MapReverted(DotnetEntityRelationModel? dotnetRelation)
    {
        if (dotnetRelation is null) return null;

        var relation = new DotnetEntityRelationModel
        {
            SourceEntity = ToDotnetName(dotnetRelation.TargetEntity),
            TargetEntity = ToDotnetName(dotnetRelation.SourceEntity),
            IsOneToOne = dotnetRelation.IsOneToOne,
            IsRequired = dotnetRelation.IsRequired && dotnetRelation.IsOneToOne,
            IsReverted = true,
        };

        return relation;
    }

    private string ToDotnetName(string tableName)
    {
        var singular = caseTransformer.ToSignular(tableName);
        var pascal = caseTransformer.ToPascalCase(singular);

        return pascal;
    }
}
