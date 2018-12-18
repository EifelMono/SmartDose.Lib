using System.Xml.Linq;
using System.Linq;
using System.Xml;

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var nuget= "../";

void DotNet(string args)
{
   StartProcess("dotnet", new  ProcessSettings {
      Arguments= args
   });
}

void BuildCsproj(string root, string csproj)
{
   csproj= System.IO.Path.Combine(root,csproj);
   var artifacts= System.IO.Path.Combine(root, "Artifacts");

   EnsureDirectoryExists(artifacts);
   CleanDirectory(artifacts);

   DotNetCoreBuild(csproj, new DotNetCoreBuildSettings {
      Configuration= configuration,
      OutputDirectory= artifacts,
   });
   DotNetCorePack(csproj);

   foreach(var file in System.IO.Directory.GetFiles(artifacts, "*.nupkg"))
      CopyFiles(file, nuget);
}

Task("RowaMore")
.Does(() => {
   BuildCsproj("./RowaMore", "./RowaMore.csproj");
});

Task("SmartDose.WcfLib")
.Does(() => {
   BuildCsproj("./SmartDose.WcfLib", "./SmartDose.WcfLib.csproj");
});

Task("SmartDose.WcfMasterData10000")
.Does(() => {
   BuildCsproj("./SmartDose.WcfMasterData10000", "./SmartDose.WcfMasterData10000.csproj");
});

Task("SmartDose.WcfMasterData9002")
.Does(() => {
   BuildCsproj("./SmartDose.WcfMasterData9002", "./SmartDose.WcfMasterData9002.csproj");
});


Task("Default")
.IsDependentOn("RowaMore")
.IsDependentOn("SmartDose.WcfLib")
.IsDependentOn("SmartDose.WcfMasterData10000")
.IsDependentOn("SmartDose.WcfMasterData9002")
.Does(() => {
   Information("Building");
});

RunTarget(target);
