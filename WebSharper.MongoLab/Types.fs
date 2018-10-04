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

[<AutoOpen; JavaScript>]
module Types =

    type Database (name) =
        member internal x.Name = name

    type Constraint =
        | Constraint of string * Condition

        | And of Constraint * Constraint
        | Or  of Constraint * Constraint

        with static member internal ToString =
                function
                | Some a ->
                    let rec helper =
                        function
                        | Constraint (fieldName, condition) ->
                            Condition.ToString fieldName condition
                        | And (a, b) ->
                            "{$and:[" + helper a + "," + helper b + "]}"
                        | Or (a, b) ->
                            "{$or:["  + helper a + "," + helper b + "]}"

                    helper a
                | _ ->
                    "{}"

    and Condition =
        | EqualsTo    of obj
        | NotEqualTo  of obj
        | GreaterThan of obj
        | LessThan    of obj

        | And of Condition * Condition
        | Or  of Condition * Condition

        with static member internal ToString fieldName condition =
                match condition with
                | EqualsTo a ->
                    "{" + fieldName + ":" + Json.Stringify a + "}"
                | NotEqualTo a ->
                    "{" + fieldName + ":{$ne:" + Json.Stringify a + "}}"
                | GreaterThan a ->
                    "{" + fieldName + ":{$gt:" + Json.Stringify a + "}}"
                | LessThan a ->
                    "{" + fieldName + ":{$lt:" + Json.Stringify a + "}}"
                | And (a, b) ->
                    "{$and:[" + Condition.ToString fieldName a + "," + Condition.ToString fieldName b + "]}"
                | Or (a, b) ->
                    "{$or:["  + Condition.ToString fieldName a + "," + Condition.ToString fieldName b + "]}"

    type SortType =
        | Ascending
        | Descending

    type Collection<'a> (name) =
        member internal x.Name = name

        member val internal Database : Database option = None with get, set
        member val internal Constraint = None with get, set

        member val internal Sorts : (string * SortType) list = [] with get, set

        static member internal ToString (collection : Collection<'a>) =
            "/databases/" + collection.Database.Value.Name + "/collections/" + collection.Name + "?q=" + Constraint.ToString collection.Constraint
    
    type PushableCollection<'a> internal (name, database) as this =
        inherit Collection<'a> (name)

        do this.Database <- Some database
