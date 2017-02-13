namespace Generic.SuaveRestAPI.Repository
open System.Collections.Generic
type User = {
   
    UserName: string
    Email: string
    FirstName: string
    LastName: string
}
module UserRepository=
   
    let private userStore = new Dictionary<string,User>()
    let getUsers ()=
        userStore.Values |> Seq.map(fun u-> u)

    let createUser user = 
        let newUser = {
            UserName = user.UserName
            Email = user.Email
            FirstName = user.FirstName
            LastName = user.LastName
        }
        userStore.Add(newUser.UserName,newUser)
        newUser
        
        

