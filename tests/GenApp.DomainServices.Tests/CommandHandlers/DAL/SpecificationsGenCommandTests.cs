using System.IO.Compression;
using GenApp.Domain.Constants;
using GenApp.Domain.Enums;
using GenApp.Domain.Interfaces;
using GenApp.Domain.Models;
using GenApp.DomainServices.CommandHandlers.DAL;
using GenApp.Templates.Resources.Models;

namespace GenApp.DomainServices.Tests.CommandHandlers.DAL;
public class SpecificationsGenCommandTests
{
    private readonly Fixture _fixture;
    private readonly IFileGenService _fileGenService;
    private readonly ZipArchive _archive;
    private readonly ApplicationDataModel _applicationDataModel;

    private readonly IGenCommand _sut;

    public SpecificationsGenCommandTests()
    {
        _fixture = new Fixture();
        _fileGenService = Substitute.For<IFileGenService>();
        _archive = Substitute.For<IArchiveGenService>().CreateArchive();
        _applicationDataModel = SetupSingleEntityModel();

        _sut = new SpecificationsGenCommand(_fileGenService);
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_base_Specification()
    {
        var model = new SpecificationModel
        {
            Namespace = $"{_applicationDataModel.AppName}.DAL.Specifications",
            Usings = new List<string>
            {
                "System.Linq.Expressions",
                $"{_applicationDataModel.AppName}.DAL.Interfaces",
                $"{_applicationDataModel.AppName}.DAL.Specifications.Orerators",
            }.Order(),
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("Specification.cs")),
                Arg.Is<SpecificationModel>(x =>
                    x.Namespace == model.Namespace &&
                    x.Usings.SequenceEqual(model.Usings)),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_AndSpecification()
    {
        var model = new AndSpecificationModel
        {
            Namespace = $"{_applicationDataModel.AppName}.DAL.Specifications.Orerators",
            Usings = new List<string>
            {
                "System.Linq.Expressions",
                $"{_applicationDataModel.AppName}.DAL.Interfaces",
            }.Order(),
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("AndSpecification.cs")),
                Arg.Is<AndSpecificationModel>(x =>
                    x.Namespace == model.Namespace &&
                    x.Usings.SequenceEqual(model.Usings)),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_OperationExtensions()
    {
        var model = new OperationExtensionsModel
        {
            Namespace = $"{_applicationDataModel.AppName}.DAL.Specifications.Orerators",
            Usings = new List<string>
            {
                "System.Linq.Expressions",
                $"{_applicationDataModel.AppName}.DAL.Interfaces",
            }.Order(),
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("OperationExtensions.cs")),
                Arg.Is<OperationExtensionsModel>(x =>
                    x.Namespace == model.Namespace &&
                    x.Usings.SequenceEqual(model.Usings)),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_TrueSpecification()
    {
        var model = new TrueSpecificationModel
        {
            Namespace = $"{_applicationDataModel.AppName}.DAL.Specifications.Orerators",
            Usings = new List<string>
            {
                "System.Linq.Expressions",
                $"{_applicationDataModel.AppName}.DAL.Interfaces",
            }.Order(),
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Is<string>(x => x.Contains("TrueSpecification.cs")),
                Arg.Is<TrueSpecificationModel>(x =>
                    x.Namespace == model.Namespace &&
                    x.Usings.SequenceEqual(model.Usings)),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_SearchSpecifications()
    {
        var model = new EntitySearchSpecificationModel
        {
            Namespace = $"{_applicationDataModel.AppName}.DAL.Specifications.StudentSpecifications",
            SpecificationName = $"SearchStudentByName",
            EntityName = $"StudentEntity",
            KeyType = DotnetTypes.Int,
            PropertyName = "Name",
            Usings = new List<string>
            {
                "System.Linq.Expressions",
                "Microsoft.EntityFrameworkCore",
                $"{_applicationDataModel.AppName}.DAL.Entities",
            }.Order(),
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Any<string>(),
                Arg.Is<EntitySearchSpecificationModel>(x =>
                    x.Namespace == model.Namespace &&
                    x.SpecificationName == model.SpecificationName &&
                    x.EntityName == model.EntityName &&
                    x.KeyType == model.KeyType &&
                    x.PropertyName == model.PropertyName &&
                    x.Usings.SequenceEqual(model.Usings)),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_FilterSpecifications()
    {
        var nameModel = new EntityFilterSpecificationModel
        {
            Namespace = $"{_applicationDataModel.AppName}.DAL.Specifications.StudentSpecifications",
            SpecificationName = $"FindStudentByName",
            EntityName = $"StudentEntity",
            KeyType = DotnetTypes.Int,
            PropertyName = "Name",
            PropertyType = DotnetTypes.String,
            IsNullable = false,
            Usings = new List<string>
            {
                "System.Linq.Expressions",
                $"{_applicationDataModel.AppName}.DAL.Entities",
            }.Order(),
        };

        var yearModel = new EntityFilterSpecificationModel
        {
            Namespace = $"{_applicationDataModel.AppName}.DAL.Specifications.StudentSpecifications",
            SpecificationName = $"FindStudentByYear",
            EntityName = $"StudentEntity",
            KeyType = DotnetTypes.Int,
            PropertyName = "Year",
            PropertyType = DotnetTypes.Int,
            IsNullable = true,
            Usings = new List<string>
            {
                "System.Linq.Expressions",
                $"{_applicationDataModel.AppName}.DAL.Entities",
            }.Order(),
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Any<string>(),
                Arg.Is<EntityFilterSpecificationModel>(x =>
                    x.Namespace == nameModel.Namespace &&
                    x.SpecificationName == nameModel.SpecificationName &&
                    x.EntityName == nameModel.EntityName &&
                    x.KeyType == nameModel.KeyType &&
                    x.PropertyName == nameModel.PropertyName &&
                    x.PropertyType == nameModel.PropertyType &&
                    x.IsNullable == nameModel.IsNullable &&
                    x.Usings.SequenceEqual(nameModel.Usings)),
                Arg.Any<CancellationToken>());

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Any<string>(),
                Arg.Is<EntityFilterSpecificationModel>(x =>
                    x.Namespace == yearModel.Namespace &&
                    x.SpecificationName == yearModel.SpecificationName &&
                    x.EntityName == yearModel.EntityName &&
                    x.KeyType == yearModel.KeyType &&
                    x.PropertyName == yearModel.PropertyName &&
                    x.PropertyType == yearModel.PropertyType &&
                    x.IsNullable == yearModel.IsNullable &&
                    x.Usings.SequenceEqual(yearModel.Usings)),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_RangeSpecifications()
    {
        var model = new EntityRangeSpecificationModel
        {
            Namespace = $"{_applicationDataModel.AppName}.DAL.Specifications.StudentSpecifications",
            SpecificationName = $"RangeStudentByYear",
            EntityName = $"StudentEntity",
            KeyType = DotnetTypes.Int,
            PropertyName = "Year",
            PropertyType = DotnetTypes.Int,
            IsNullable = true,
            Usings = new List<string>
            {
                "System.Linq.Expressions",
                $"{_applicationDataModel.AppName}.DAL.Entities",
                $"{_applicationDataModel.AppName}.DAL.Models",
            }.Order(),
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Any<string>(),
                Arg.Is<EntityRangeSpecificationModel>(x =>
                    x.Namespace == model.Namespace &&
                    x.SpecificationName == model.SpecificationName &&
                    x.EntityName == model.EntityName &&
                    x.KeyType == model.KeyType &&
                    x.PropertyName == model.PropertyName &&
                    x.Usings.SequenceEqual(model.Usings)),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_should_call_CreateEntryAsync_for_ByIdSpecifications()
    {
        var model = new EntitySpecificationByIdModel
        {
            Namespace = $"{_applicationDataModel.AppName}.DAL.Specifications.StudentSpecifications",
            SpecificationName = $"FindStudentById",
            EntityName = $"StudentEntity",
            KeyType = DotnetTypes.Int,
            PropertyName = "Id",
            Usings = new List<string>
            {
                "System.Linq.Expressions",
                $"{_applicationDataModel.AppName}.DAL.Entities",
            }.Order(),
        };

        await _sut.ExecuteAsync(_archive, _applicationDataModel, default);

        await _fileGenService
            .Received(1)
            .CreateEntryAsync(
                Arg.Any<ZipArchive>(),
                Arg.Any<string>(),
                Arg.Is<EntitySpecificationByIdModel>(x =>
                    x.Namespace == model.Namespace &&
                    x.SpecificationName == model.SpecificationName &&
                    x.EntityName == model.EntityName &&
                    x.KeyType == model.KeyType &&
                    x.PropertyName == model.PropertyName &&
                    x.Usings.SequenceEqual(model.Usings)),
                Arg.Any<CancellationToken>());
    }

    private ApplicationDataModel SetupSingleEntityModel()
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
                        new() { Name = "Year", Type = DotnetTypes.Int },
                    }
                },
            })
            .Create();

        return model;
    }
}
