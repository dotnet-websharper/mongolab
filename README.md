# WebSharper.MongoLab

WebSharper abstractions for MongoLab REST API.

## Hello World

```
#!fsharp

async {
    let! users =
        Database "websharper" >- Collection "Users"
        |> Fetch

    users
    |> Array.iter (fun user ->
        JavaScript.Log (user?Name + " (" + user?Location + ")")
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

You can restrict queries as in any other query language with ``Constraint``s and ``Condition``s. Let's count the Hungarian users.

```
#!fsharp

async {
    let! n =
        Database "websharper" >- Collection "Users"
        |> Where (fun _ ->
            Constraint ("Location", EqualsTo "Hungary")
        )
        |> Count

    JavaScript.Log ("Number of hungarian users: " + string n)
}
|> Async.Start
```

## Insert

Inserting a document is just as easy as fetching one, and can be done in a type-safe way.

```
#!fsharp

type User =
    {
        Name     : string
        Location : string
    }

Database "websharper" >- Collection<User> "Users"
|> Push {
    Name     = "Sandor Rakonczai"
    Location = "Hungary"
}
|> Async.Ignore
|> Async.Start
```

## Optional "type-safety"

You can explicitly set the collection-type. After ``Collection<'a>``, ``Push`` will look like ``'a -> Async<bool>`` rather than ``obj -> ...``, which is much better.