namespace IntelliFactory.WebSharper.MongoLab

open IntelliFactory.WebSharper

[<AutoOpen; JavaScript>]
module Functions =
    
    open IntelliFactory.WebSharper.JQuery

    let (>-) database colName = PushableCollection (colName, database)

    let Where ``constraint`` (collection : Collection) =
        Collection (collection.Name, collection.Database, ``constraint`` (), collection.Sorts)

    let Sort sorts (collection : Collection) =
        Collection (collection.Name, collection.Database, collection.Constraint, sorts)
    
    let Fetch collection =
        Async.FromContinuations (fun (ok, _, _) ->
            JQuery.GetJSON(
                !BaseUrl + Collection.ToString collection + "&apiKey=" + !Key,
                null,
                fun (result, _) ->
                    ok (As<obj array> result)
            )
            |> ignore
        )
    
    let Count collection =
        Async.FromContinuations (fun (ok, _, _) ->
            JQuery.GetJSON(
                !BaseUrl + Collection.ToString collection + "&c=true&apiKey=" + !Key,
                null,
                fun (result, _) ->
                    ok (As<int> result)
            )
            |> ignore
        )

//    let Push data (collection : PushableCollection) =
//        X<Async<unit>>
