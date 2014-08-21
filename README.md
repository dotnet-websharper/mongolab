# WebSharper.MongoLab

WebSharper abstractions for MongoLab REST API.

## Hello World

```
#!fsharp

async {
    let! users =
        Database "websharper" >- "users"
        |> Fetch

    users
    |> Array.iter (fun user ->
        JavaScript.Log user?name
    )
}
|> Async.Start
```

##The ``Key`` reference cell

In order to be able to use the API, you have to set ``Key`` to your API key before any API call.

```
#!fsharp

Key := "Your API key."
```

## Constraints

We can restrict as in any other query-language with ``Constraint``s and ``Condition``s. Let's count the Hungarian users.

```
#!fsharp

async {
    let! n =
        Database "websharper" >- "users"
        |> Where (fun _ ->
            Some <| Constraint ("location", EqualsTo "Hungary")
        )
        |> Count

    JavaScript.Log ("Number of hungarian users: " + string n)
}
```