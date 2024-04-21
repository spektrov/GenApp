namespace GenApp.Templates.Resources.StaticTemplates;
public static class DockerignoreContent
{
    public static string Value =>
        @"**/.classpath
**/.dockerignore
**/.env
**/.git
**/.gitignore
**/.project
**/.settings
**/.toolstarget
**/.vs
**/.vscode
**/*.*proj.user
**/*.dbmdl
**/*.jfm
**/azds.yaml
**/bin
**/charts
**/node_modules
**/npm-debug.log
**/obj
**/docker-compose.yml
**/Docerfile
**/secrets.dev.yaml
**/values.dev.yaml
LICENSE
README.md
!**/.gitignore
!.git/HEAD
!.git/config
!.git/packed-refs
!.git/refs/heads/**";
}
