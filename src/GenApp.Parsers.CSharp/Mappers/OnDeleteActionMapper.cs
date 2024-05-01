using GenApp.Parsers.Abstractions.Constants;

namespace GenApp.Parsers.CSharp.Mappers;
public static class OnDeleteActionMapper
{
    public static string Map(string? sqlOnDeleteAction)
    {
        return sqlOnDeleteAction switch
        {
            SqlOnDeleteActions.Cascade => DotnetOnDeleteActions.Cascade,
            SqlOnDeleteActions.SetNull => DotnetOnDeleteActions.SetNull,
            SqlOnDeleteActions.SetDefault => DotnetOnDeleteActions.SetDefault,
            SqlOnDeleteActions.NoAction => DotnetOnDeleteActions.NoAction,
            SqlOnDeleteActions.Restrict => DotnetOnDeleteActions.NoAction,
            _ => DotnetOnDeleteActions.NoAction,
        };
    }
}
