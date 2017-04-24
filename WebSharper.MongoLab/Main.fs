namespace WebSharper.MongoLab

open WebSharper
open WebSharper.JavaScript

[<AutoOpen; JavaScript>]
module Functions =
    
    open WebSharper.JQuery

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
#if ZAFIR
                (fun result _ _ ->
#else
                (fun (result, _) ->
#endif
                     ok (result :?> 'a array))
            )
            |> ignore
        )
    
    let Count (collection : Collection<_>) =
        Async.FromContinuations (fun (ok, _, _) ->
            JQuery.GetJSON(
                !BaseUrl + Collection<_>.ToString collection + "&c=true&apiKey=" + !Key,
                null,
#if ZAFIR
                (fun result _ _ ->
#else
                (fun (result, _) ->
#endif
                    ok (result :?> int))
            )
            |> ignore
        )
    
    let Push (data : 'a) (collection : PushableCollection<'a>) =
        Async.FromContinuations (fun (ok, _, _) ->
            JQuery.Ajax(
                !BaseUrl + Collection<'a>.ToString collection + "&apiKey=" + !Key,
                AjaxSettings(
                    Type    = RequestType.POST,
                    Data    = Json.Stringify data,
                    Success =
#if ZAFIR
                        (fun _ _ _ -> ok true),
#else
                        (fun _ -> ok true),
#endif
                    Headers = New [
                        "Content-Type" => "application/json"
                    ]
                )
            )
            |> ignore
        )
