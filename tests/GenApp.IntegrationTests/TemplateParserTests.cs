using GenApp.Templates.Abstractions;
using GenApp.Templates.Parser;
using GenApp.Templates.Resources.DTOs;
using GenApp.Templates.Resources.Models;

namespace GenApp.IntegrationTests;
public class TemplateParserTests
{
    private readonly ITemplateParser _sut;

    public TemplateParserTests()
    {
        _sut = new TemplateParser();
    }

    [Fact]
    public async Task ParseAsync_should_parse_database_context_content()
    {
        var model = new DatabaseContextModel
        {
            Namespace = $"Template.DAL",
            Usings = new[]
            {
                "Microsoft.EntityFrameworkCore",
                "Template.DAL.Entities",
            },
            Sets = new List<DbSetDto>
            {
                new()
                {
                    EntityName = "StudentEntity",
                    CollectionName = "Students",
                },
                new()
                {
                    EntityName = "ProfessorEntity",
                    CollectionName = "ProfessorEntity",
                },
            }
        };

        var expected =
            @"using Microsoft.EntityFrameworkCore;
using Template.DAL.Entities;

namespace Template.DAL;
public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<StudentEntity> Students { get; set; }
    
    public DbSet<ProfessorEntity> ProfessorEntity { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Bootstrap).Assembly);
    }
}";

        var content = await _sut.ParseAsync(model, CancellationToken.None);

        content.Should().BeEquivalentTo(expected);
    }
}
