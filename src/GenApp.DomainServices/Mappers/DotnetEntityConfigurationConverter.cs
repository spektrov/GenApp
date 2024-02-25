using AutoMapper;
using GenApp.Domain.Constants;
using GenApp.Domain.Extensions;
using GenApp.Domain.Models;

namespace GenApp.DomainServices.Mappers;
public class DotnetEntityConfigurationConverter : ITypeConverter<SqlTableConfigurationModel, DotnetEntityConfigurationModel>
{
    public DotnetEntityConfigurationModel Convert(
        SqlTableConfigurationModel source, DotnetEntityConfigurationModel destination, ResolutionContext context)
    {
        return new DotnetEntityConfigurationModel
        {
            EntityName = source.TableName.ToPascalCase(),
            Properties = source.Columns
            .Select(column => new DotnetPropertyConfigurationModel
            {
                Name = !column.IsPrimaryKey ? column.ColumnName.ToPascalCase() : NameConstants.Id,
                Type = PropertyTypeMapper.Map(source.DbmsType, column.ColumnType),
                NotNull = column.NotNull,
                IsId = column.IsPrimaryKey,
            }),
        };
    }
}