namespace IntelliFactory.WebSharper.MongoLab

open IntelliFactory.WebSharper

[<AutoOpen; JavaScript>]
module Functions =
    
    open IntelliFactory.WebSharper.JQuery

    let (>-) database (collection : Collection<'a>) = PushableCollection<'a> (collection.Name, database)

    let Where ``constraint`` (collection : Collection<'a>) =
        Collection<'a> (collection.Name, Database = collection.Database, Constraint = Some (``constraint`` ()), Sorts = collection.Sorts)

    let Sort sorts (collection : Collection<'a>) =
        Collection<'a> (collection.Name, Database = collection.Database, Constraint = collection.Constraint, Sorts = sorts)
    
    let Fetch (collection : Collection<'a>) =
        Async.FromContinuations (fun (ok, _, _) ->
            JQuery.GetJSON(
                !BaseUrl + Collection<'a>.ToString collection + "&apiKey=" + !Key,
                null,
                fun (result, _) ->
                    ok (As<'a array> result)
            )
            |> ignore
        )
    
    let Count (collection : Collection<_>) =
        Async.FromContinuations (fun (ok, _, _) ->
            JQuery.GetJSON(
                !BaseUrl + Collection<_>.ToString collection + "&c=true&apiKey=" + !Key,
                null,
                fun (result, _) ->
                    ok (As<int> result)
            )
            |> ignore
        )
    
    let Push (data : 'a) (collection : PushableCollection<'a>) =
        Async.FromContinuations (fun (ok, _, _) ->
            JQuery.Ajax(
                !BaseUrl + Collection<'a>.ToString collection + "&apiKey=" + !Key,
                AjaxConfig(
                    Type    = As RequestType.POST,
                    Data    = Json.Stringify data,
                    Success =
                        As (fun _ ->
                            ok true
                        ),
                    Headers = New [
                        "Content-Type" => "application/json"
                    ]
                )
            )
            |> ignore
        )