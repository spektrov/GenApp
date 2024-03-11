namespace GenApp.Templates.Resources.StaticTemplates;
public static class SolutionFileContent
{
    public static string Value(string appNamespace)
    {
        string slnContent =
            @"Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.8.34525.116
MinimumVisualStudioVersion = 10.0.40219.1
Project(""{{PROJECT-GUID}}"") = ""{APP-NAMESPACE}.BLL"", ""src\{APP-NAMESPACE}.BLL\{APP-NAMESPACE}.BLL.csproj"", ""{{BLL-PROJECT-GUID}}""
EndProject
Project(""{{PROJECT-GUID}}"") = ""{APP-NAMESPACE}.DAL"", ""src\{APP-NAMESPACE}.DAL\{APP-NAMESPACE}.DAL.csproj"", ""{{DAL-PROJECT-GUID}}""
EndProject
Project(""{{PROJECT-GUID}}"") = ""{APP-NAMESPACE}.API"", ""src\TemplateSolution.API\{APP-NAMESPACE}.API.csproj"", ""{API-PROJECT-GUID}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""SolutionItems"", ""SolutionItems"", ""{A9BB38AD-B0A7-4547-87E7-3745DFD4433C}""
	ProjectSection(SolutionItems) = preProject
		.editorconfig = .editorconfig
	EndProjectSection
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{{BLL-PROJECT-GUID}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{{BLL-PROJECT-GUID}}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{{BLL-PROJECT-GUID}}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{{BLL-PROJECT-GUID}}.Release|Any CPU.Build.0 = Release|Any CPU
		{{DAL-PROJECT-GUID}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{{DAL-PROJECT-GUID}}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{{DAL-PROJECT-GUID}9}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{{DAL-PROJECT-GUID}}.Release|Any CPU.Build.0 = Release|Any CPU
		{{API-PROJECT-GUID}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{{API-PROJECT-GUID}}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{{API-PROJECT-GUID}}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{{API-PROJECT-GUID}}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {{SLN-GUID}}
	EndGlobalSection
EndGlobal
";
        slnContent = slnContent.Replace("{BLL-PROJECT-GUID}", Guid.NewGuid().ToString().ToUpper());
        slnContent = slnContent.Replace("{DAL-PROJECT-GUID}", Guid.NewGuid().ToString().ToUpper());
        slnContent = slnContent.Replace("{API-PROJECT-GUID}", Guid.NewGuid().ToString().ToUpper());
        slnContent = slnContent.Replace("{SLN-GUID}", Guid.NewGuid().ToString().ToUpper());
        slnContent = slnContent.Replace("{PROJECT-GUID}", Guid.NewGuid().ToString().ToUpper());
        slnContent = slnContent.Replace("{APP-NAMESPACE}", appNamespace);

        return slnContent;
    }
}
