// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}
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
                (fun result _ _ ->
                     ok (result :?> 'a array))
            )
            |> ignore
        )
    
    let Count (collection : Collection<_>) =
        Async.FromContinuations (fun (ok, _, _) ->
            JQuery.GetJSON(
                !BaseUrl + Collection<_>.ToString collection + "&c=true&apiKey=" + !Key,
                null,
                (fun result _ _ ->
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
                        (fun _ _ _ -> ok true),
                    Headers = New [
                        "Content-Type" => "application/json"
                    ]
                )
            )
            |> ignore
        )
