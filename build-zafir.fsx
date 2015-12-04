#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("Zafir.MongoLab")
        .VersionFrom("Zafir")
        .WithFSharpVersion(FSharpVersion.FSharp30)
        .WithFramework(fun fw -> fw.Net40)

let main =
    bt.Zafir.Library("WebSharper.MongoLab")
        .SourcesFromProject()

bt.Solution [

    main

    bt.NuGet.CreatePackage()
        .Description("WebSharper abstractions for MongoLab REST API.")
        .ProjectUrl("https://bitbucket.org/IntelliFactory/websharper.mongolab")
        .Configure(fun configuration ->
            {
                configuration with
                    Authors    = ["IntelliFactory"]
                    Title      = Some "Zafir.MongoLab"
                    LicenseUrl = Some "https://bitbucket.org/IntelliFactory/websharper.mongolab/src/0ac630dac46384a35fb3b85ef00d91965527cdf8/LICENSE.md"
                
                    RequiresLicenseAcceptance = true
            }
        )
        .Add main
]
|> bt.Dispatch
