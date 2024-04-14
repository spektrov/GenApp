using GenApp.Domain.Models;
using GenApp.Parsers.Abstractions.Models;

namespace GenApp.Parsers.CSharp.Interfaces;
public interface IDotnetRelationMapper
{
    DotnetEntityRelationModel? Map(SqlRelationConfiguration? sqlRelation);

    DotnetEntityRelationModel? MapReverted(DotnetEntityRelationModel? dotnetRelation);
}
