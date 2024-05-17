using System.IO.Compression;
using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.SolutionLevel;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Tests.CommandHandlers.SolutionLevel;
public class ProjectFilesGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ZipArchive _archive;

    private readonly IGenCommand _sut;

    public ProjectFilesGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();

        _sut = new ProjectFilesGenCommand(_fileGenService);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_dal_project()
    {
        var model = _fixture.Build<ApplicationDataModel>()
            .With(x => x.DotnetSdkVersion, 8)
            .With(x => x.AppName, "CoolApp")
            .With(x => x.DbmsType, DbmsType.MYSQL)
            .Create();

        var fileModel = new ProjectFileModel
        {
            Type = ProjectTypes.DllType,
            SdkVersion = "net8.0",
            Packages = new List<PackageDto>
            {
                new() { Name = "Microsoft.EntityFrameworkCore.Relational", Version = "6.0.27" },
                new() { Name = "Pomelo.EntityFrameworkCore.MySql", Version = "6.0.2" },
            }
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x == "src/CoolApp.DAL/CoolApp.DAL.csproj"),
                Arg.Is<ProjectFileModel>(x =>
                    x.Type == fileModel.Type &&
                    x.SdkVersion == fileModel.SdkVersion &&
                    x.Packages!.SequenceEqual(fileModel.Packages, new ReflectionObjectEqualityComparer<PackageDto>())),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_bll_project()
    {
        var model = _fixture.Build<ApplicationDataModel>()
            .With(x => x.DotnetSdkVersion, 8)
            .With(x => x.AppName, "CoolApp")
            .With(x => x.DbmsType, DbmsType.MYSQL)
            .Create();

        var fileModel = new ProjectFileModel
        {
            Type = ProjectTypes.DllType,
            SdkVersion = "net8.0",
            Packages = new List<PackageDto>
            {
                new() { Name = "AutoMapper", Version = "13.0.1" },
            },
            Includes = new List<string>
            {
                $"..\\CoolApp.DAL\\CoolApp.DAL.csproj"
            },
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x == "src/CoolApp.BLL/CoolApp.BLL.csproj"),
                Arg.Is<ProjectFileModel>(x =>
                    x.Type == fileModel.Type &&
                    x.SdkVersion == fileModel.SdkVersion &&
                    x.Packages!.SequenceEqual(fileModel.Packages, new ReflectionObjectEqualityComparer<PackageDto>()) &&
                    x.Includes!.SequenceEqual(fileModel.Includes)),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_api_project()
    {
        var model = _fixture.Build<ApplicationDataModel>()
            .With(x => x.DotnetSdkVersion, 8)
            .With(x => x.AppName, "CoolApp")
            .With(x => x.DbmsType, DbmsType.MYSQL)
            .Create();

        var fileModel = new ProjectFileModel
        {
            Type = ProjectTypes.WebType,
            SdkVersion = "net8.0",
            Packages = new List<PackageDto>
            {
                    new() { Name = "Swashbuckle.AspNetCore", Version = "6.5.0" },
                    new() { Name = "AutoMapper", Version = "13.0.1" },
            },
            Includes = new List<string>
            {
                $"..\\CoolApp.BLL\\CoolApp.BLL.csproj"
            },
        };

        await _sut.ExecuteAsync(_archive, model, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x == "src/CoolApp.API/CoolApp.API.csproj"),
                Arg.Is<ProjectFileModel>(x =>
                    x.Type == fileModel.Type &&
                    x.SdkVersion == fileModel.SdkVersion &&
                    x.Packages!.SequenceEqual(fileModel.Packages, new ReflectionObjectEqualityComparer<PackageDto>()) &&
                    x.Includes!.SequenceEqual(fileModel.Includes)),
                Arg.Any<CancellationToken>());
    }
}
