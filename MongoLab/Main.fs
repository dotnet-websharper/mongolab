namespace IntelliFactory.WebSharper.MongoLab

open IntelliFactory.WebSharper

[<AutoOpen; JavaScript>]
module Main =
    
    open IntelliFactory.WebSharper.JQuery

    let Key = ref ""

    type Database (name) =
        member x.Name : string = name

    type Collection =
        {
            Name       : string
            Database   : Database
        }
        with static member ToString (collection : Collection) =
                "/databases/" + collection.Database.Name + "/collections/" + collection.Name
    
    let (>-) database colName =
        { Name = colName; Database = database }

    let private BaseUrl = "https://api.mongolab.com/api/1"

    let private Request<'a> urlExtension collection =
        Async.FromContinuations (fun (ok, _, _) ->
            JQuery.GetJSON(
                BaseUrl + Collection.ToString collection + "?apiKey=" + !Key + (function | Some extension -> extension | _ -> "") urlExtension,
                null,
                fun (result, _) ->
                    ok (As<'a> result)
            )
            |> ignore
        )

    let Fetch =
        Request<obj array> None
    
    let Count =
        Request<int> (Some "&c=true")
