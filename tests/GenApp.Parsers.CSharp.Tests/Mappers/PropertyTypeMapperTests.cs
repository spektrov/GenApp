using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Parsers.Abstractions.Constants;
using GenApp.Parsers.CSharp.Mappers;

namespace GenApp.Parsers.CSharp.Tests.Mappers;
public class PropertyTypeMapperTests
{
    [Theory]
    [InlineData(DbmsType.POSTGRESQL, PostgreSqlTypes.Bool, DotnetTypes.Bool)]
    [InlineData(DbmsType.POSTGRESQL, PostgreSqlTypes.Timestamp, DotnetTypes.DateTime)]
    [InlineData(DbmsType.MSSQLSERVER, MSSqlServerTypes.Money, DotnetTypes.Decimal)]
    [InlineData(DbmsType.MSSQLSERVER, MSSqlServerTypes.Text, DotnetTypes.String)]
    [InlineData(DbmsType.MYSQL, MySqlTypes.Mediumtext, DotnetTypes.String)]
    [InlineData(DbmsType.MYSQL, MySqlTypes.Float, DotnetTypes.Float)]
    public void Map_should_return_dotnet_type_by_dbms_and_sql_type(DbmsType dbmsType, string sqlType, string dotnetType)
    {
        var actual = PropertyTypeMapper.Map(dbmsType, sqlType);

        actual.Should().BeEquivalentTo(dotnetType);
    }

    [Fact]
    public void Map_should_throw_exception_if_dbms_not_provided()
    {
        DbmsType dbmsType = DbmsType.MYSQL;
        var sqlType = "some_123";

        Assert.Throws<ArgumentException>(() => PropertyTypeMapper.Map(dbmsType, sqlType));
    }
}
