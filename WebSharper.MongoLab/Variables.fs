namespace WebSharper.MongoLab

open WebSharper

[<AutoOpen; JavaScript>]
module Variables =
    
    let BaseUrl = ref "https://api.mongolab.com/api/1"

    let Key = ref ""
    
