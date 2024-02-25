﻿namespace GenApp.Templates.Resources.StaticTemplates;
public static class SolutionFileContent
{
    public static string Value(string appNamespace)
    {
        string slnContent =
            @"Microsoft Visual Studio Solution File, Format Version 12.00
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""{APP-NAMESPACE}"", ""src\{APP-NAMESPACE}.Domain\{APP-NAMESPACE}.Domain.csproj"", ""{YOUR-PROJECT-GUID}""
EndProject
Global
    GlobalSection(SolutionConfigurationPlatforms) = preSolution
        Debug|Any CPU = Debug|Any CPU
        Release|Any CPU = Release|Any CPU
    EndGlobalSection
    GlobalSection(ProjectConfigurationPlatforms) = postSolution
        {YOUR-PROJECT-GUID}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
        {YOUR-PROJECT-GUID}.Debug|Any CPU.Build.0 = Debug|Any CPU
        {YOUR-PROJECT-GUID}.Release|Any CPU.ActiveCfg = Release|Any CPU
        {YOUR-PROJECT-GUID}.Release|Any CPU.Build.0 = Release|Any CPU
    EndGlobalSection
    GlobalSection(SolutionProperties) = preSolution
        HideSolutionNode = FALSE
    EndGlobalSection
EndGlobal
";
        slnContent = slnContent.Replace("{YOUR-PROJECT-GUID}", Guid.NewGuid().ToString().ToUpper());
        slnContent = slnContent.Replace("{APP-NAMESPACE}", appNamespace);

        return slnContent;
    }
}
