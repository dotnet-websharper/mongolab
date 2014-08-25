namespace IntelliFactory.WebSharper.MongoLab

open IntelliFactory.WebSharper

[<AutoOpen; JavaScript>]
module Functions =
    
    open IntelliFactory.WebSharper.JQuery

    let (>-) database (collection : Collection<'a>) = PushableCollection<'a> (collection.Name, database)

    let Where ``constraint`` (collection : Col) =
        Col (collection.Name, collection.Database, Some (``constraint`` ()), collection.Sorts)

    let Sort sorts (collection : Col) =
        Col (collection.Name, collection.Database, collection.Constraint, sorts)
    
    let Fetch collection =
        Async.FromContinuations (fun (ok, _, _) ->
            JQuery.GetJSON(
                !BaseUrl + Col.ToString collection + "&apiKey=" + !Key,
                null,
                fun (result, _) ->
                    ok (As<obj array> result)
            )
            |> ignore
        )
    
    let Count collection =
        Async.FromContinuations (fun (ok, _, _) ->
            JQuery.GetJSON(
                !BaseUrl + Col.ToString collection + "&c=true&apiKey=" + !Key,
                null,
                fun (result, _) ->
                    ok (As<int> result)
            )
            |> ignore
        )
    
    let Push (data : 'a) (collection : PushableCollection<'a>) =
        Async.FromContinuations (fun (ok, _, _) ->
            JQuery.Ajax(
                !BaseUrl + Col.ToString collection + "&apiKey=" + !Key,
                AjaxConfig(
                    Type    = As RequestType.POST,
                    Data    = Json.Stringify data,
                    Success =
                        As (fun (result, _, _) ->
                            ok true
                        ),
                    Headers = New [
                        "Content-Type" => "application/json"
                    ]
                )
            )
            |> ignore
        )