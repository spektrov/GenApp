using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.Services;

namespace GenApp.DomainServices.Tests.Services;
public class ConnectionDetailsProviderTests
{
    private readonly Fixture _fixture;

    private readonly IConnectionDetailsProvider _sut;

    public ConnectionDetailsProviderTests()
    {
        _fixture = new Fixture();

        _sut = new ConnectionDetailsProvider();
    }

    [Theory]
    [InlineData(DbmsType.POSTGRESQL, "CoolApp", "postgres", "coolapp")]
    [InlineData(DbmsType.MYSQL, "Student", "admin", "student")]
    [InlineData(DbmsType.MSSQLSERVER, "Student", "admin", "student")]
    public void Get_should_return_conn_details_for_dbms(
        DbmsType dbmsType, string appName, string expectedUser, string expectedDbName)
    {
        var model = _fixture.Build<ApplicationDataModel>()
            .With(x => x.AppName, appName)
            .With(x => x.DbmsType, dbmsType)
            .Create();

        var actual = _sut.Get(model);

        actual.User.Should().Be(expectedUser);
        actual.DbName.Should().Be(expectedDbName);
    }
}
