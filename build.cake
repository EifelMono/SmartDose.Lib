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


Task("RowaLog")
.Does(() => {
   BuildCsproj("./RowaLog", "./RowaLog/RowaLog.csproj");
});

Task("RowaMore")
.Does(() => {
   BuildCsproj("./RowaMore", "./RowaMore.csproj");
});


Task("RowaWcfConvert")
.Does(() => {
   DotNet("tool install --global Project2015To2017.Migrate2017.Tool");
   DotNet("migrate-2017 migrate ./RowaWcf/RowaWcf/RowaWcf.csproj");
});

// Set Version from FileVersion
// <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
// <Version>x.x.x</Version>
Task("RowaWcfFixPack")
.Does(() => {
   var root= "./RowaWcf";
   var csproj= System.IO.Path.Combine(root, "./RowaWcf/RowaWcf.csproj");
   var xDoc = XDocument.Load(csproj);
   var XProject = xDoc.Descendants("Project").FirstOrDefault();
   var XProjectPropertyGroup = XProject.Descendants("PropertyGroup").FirstOrDefault();
   var version= "0.0.0";
   var fileVersion= "0.0.0";
   var generatePackageOnBuild= "false";
   var isRowaLog= false;
   foreach(var x in XProjectPropertyGroup.Descendants("FileVersion"))
      fileVersion= x.Value;  
   foreach(var x in XProjectPropertyGroup.Descendants("Version"))
   {
      x.Value= fileVersion;
      version= fileVersion;
   }
   foreach(var x in XProjectPropertyGroup.Descendants("GeneratePackageOnBuild"))
   {
      x.Value= "true";
      generatePackageOnBuild= x.Value; 
   }
   foreach(var x in XProject.Descendants("PackageReference"))
         if (x.Attribute("Include")!= null  && ((string)x.Attribute("Include")) == "RowaLog")
            isRowaLog= true;
   foreach(var x in xDoc.Descendants("TargetFramework"))
      x.Value= "net471";

   if (version== "0.0.0")
      XProjectPropertyGroup.Add(new XElement("Version", fileVersion));
   if (generatePackageOnBuild.ToLower() != "true")
      XProjectPropertyGroup.Add(new XElement("GeneratePackageOnBuild", true));

   if (!isRowaLog) 
   {
      var XItemGroup= new  XElement("ItemGroup");
      XProject.Add(XItemGroup);
      XItemGroup.Add(new XElement("PackageReference", new XAttribute("Include", "RowaLog"), new XAttribute("Version", "1.3.0.8")));
      XItemGroup.Add(new XElement("PackageReference", new XAttribute("Include", "System.ServiceModel.Duplex"), new XAttribute("Version","4.5.3")));
      XItemGroup.Add(new XElement("PackageReference", new XAttribute("Include", "System.ServiceModel.Http"), new XAttribute("Version","4.5.3")));
      XItemGroup.Add(new XElement("PackageReference", new XAttribute("Include", "System.ServiceModel.NetTcp"), new XAttribute("Version","4.5.3")));
      XItemGroup.Add(new XElement("PackageReference", new XAttribute("Include", "System.ServiceModel.Security"), new XAttribute("Version","4.5.3")));
   }
   xDoc.Save(csproj);
});

Task("RowaWcf")
.IsDependentOn("RowaWcfFixPack")
.Does(() => {
   var artifacts= "./RowaWcf/Artifacts";
   var root= "./RowaWcf";
   var csproj= System.IO.Path.Combine(root, "./RowaWcf/RowaWcf.csproj");
   EnsureDirectoryExists(artifacts);
   CleanDirectory(artifacts);

   // NuGetUpdate(csproj, new NuGetUpdateSettings {
   //    ConfigFile= "NuGet.Config"
   // });
   NuGetRestore(csproj, new NuGetRestoreSettings {
      ConfigFile= "NuGet.Config"
   });
   DotNetCoreBuild(csproj, new DotNetCoreBuildSettings {
      Configuration= configuration,
      OutputDirectory= artifacts,
   });
   DotNetCorePack(csproj);

   foreach(var file in System.IO.Directory.GetFiles(artifacts, "*.nupkg"))
      CopyFiles(file, nuget);
});


Task("MasterData10000")
.Does(() => {
   BuildCsproj("./MasterData10000", "./MasterData10000.csproj");
});

Task("Default")
.IsDependentOn("RowaLog")
.IsDependentOn("RowaWcf")
.IsDependentOn("RowaMore")
.IsDependentOn("MasterData10000")
.Does(() => {
   Information("Building");
});

RunTarget(target);
