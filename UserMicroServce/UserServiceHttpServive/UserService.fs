namespace Generic.SuaveRestAPi.Rest
open Suave
open Newtonsoft.Json
open Newtonsoft.Json.Serialization
open Suave.Successful
open Suave.Operators
open Suave.Http

[<AutoOpen>]
module RestService=
    open Suave.RequestErrors
    open Suave.Filters
    
    type RestResource<'a>={
      GetAll:unit-> 'a seq
      //GetByUserName: unit-> 'a seq
      Create: 'a->'a
    }
      //string -> RestResource<'a> -> Webpart
   
    let JSON resource = 
        let newtonJsonSerializerSettings = new JsonSerializerSettings()
        newtonJsonSerializerSettings.ContractResolver <- new CamelCasePropertyNamesContractResolver()

        JsonConvert.SerializeObject(resource,newtonJsonSerializerSettings)
        |> OK
        >=> Writers.setMimeType "application/json; charset=utf-8"

    let fromJson<'a> json = 
        JsonConvert.DeserializeObject(json, typeof<'a>):?> 'a //que?

    let getResourceFromRequest<'a> (request:HttpRequest)=
        let getString rawRequestValue = 
            System.Text.Encoding.UTF8.GetString(rawRequestValue)
        request.rawForm |> getString |> fromJson<'a>

    let rest resourceName resource =
        let resourcePath = "/" + resourceName
        let getAll = warbler(fun _-> resource.GetAll ()|> JSON)
       
        path resourcePath >=> choose[
            GET >=> getAll
            POST >=> request  ( getResourceFromRequest >> resource.Create>>JSON)
            ]

    
   

   
    