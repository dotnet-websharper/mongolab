#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("WebSharper.MongoLab", "3.0-alpha")
    |> fun bt -> bt.WithFramework bt.Framework.Net40

let main =
    bt.WebSharper.Library("WebSharper.MongoLab")
        .SourcesFromProject()
        .References(fun rb ->
            [
                rb.NuGet("WebSharper").Reference ()
            ])

bt.Solution [

    main

    bt.NuGet.CreatePackage()
        .Description("WebSharper abstractions for MongoLab REST API.")
        .ProjectUrl("https://bitbucket.org/IntelliFactory/websharper.mongolab")
        .Configure(fun configuration ->
            {
                configuration with
                    Authors    = ["IntelliFactory"]
                    Title      = Some "WebSharper.MongoLab"
                    LicenseUrl = Some "https://bitbucket.org/IntelliFactory/websharper.mongolab/src/0ac630dac46384a35fb3b85ef00d91965527cdf8/LICENSE.md"
                
                    RequiresLicenseAcceptance = true
            }
        )
        .Add main
]
|> bt.Dispatch
