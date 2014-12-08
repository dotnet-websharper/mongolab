namespace IntelliFactory.WebSharper.MongoLab

open IntelliFactory.WebSharper

[<AutoOpen; JavaScript>]
module Variables =
    
    let BaseUrl = ref "https://api.mongolab.com/api/1"

    let Key = ref ""
    