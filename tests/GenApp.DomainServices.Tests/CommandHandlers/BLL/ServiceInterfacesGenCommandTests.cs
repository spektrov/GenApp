using System.IO.Compression;
using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.BLL;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Tests.CommandHandlers.BLL;
public class ServiceInterfacesGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public ServiceInterfacesGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new ServiceInterfacesGenCommand(_fileGenService);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_with_correct_parameters()
    {
        var model = _fixture.Build<ApplicationDataModel>()
            .With(x => x.AppName, "CoolApp")
            .With(x => x.DbmsType, DbmsType.MYSQL)
            .With(x => x.Entities, new List<DotnetEntityConfigurationModel>
            {
                new()
                {
                    EntityName = "Student",
                    HasId = true,
                    Table = new SqlTableInfoModel() { Name = "student", KeyName = "student_id" },
                    IdType = DotnetTypes.Int,
                    Properties = new List<DotnetPropertyConfigurationModel>
                    {
                        new() { Name = "Id", Type = DotnetTypes.Int, IsId = true },
                        new() { Name = "Name", Type = DotnetTypes.String },
                    }
                }
            })
            .Create();

        var interfaceModel = new ServiceInterfaceModel
        {
            Namespace = $"{model.AppName}.BLL.Interfaces",
            InterfaceName = $"IStudentService",
            ModelName = $"StudentModel",
            CommandModelName = $"StudentCommandModel",
            KeyType = DotnetTypes.Int,
            Usings = new List<string>
            {
                $"{model.AppName}.BLL.CommandModels",
                $"{model.AppName}.BLL.DomainModels",
                $"{model.AppName}.BLL.Models",
            }.Order(),
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Any<string>(),
                Arg.Is<ServiceInterfaceModel>(x =>
                    x.Namespace == interfaceModel.Namespace &&
                    x.Usings!.SequenceEqual(interfaceModel.Usings) &&
                    x.InterfaceName == interfaceModel.InterfaceName &&
                    x.ModelName == interfaceModel.ModelName &&
                    x.CommandModelName == interfaceModel.CommandModelName &&
                    x.KeyType == interfaceModel.KeyType),
                Arg.Any<CancellationToken>());
    }
}
