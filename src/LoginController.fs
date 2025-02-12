namespace CounterApp 

open System.Text
open Snowflake.Data.Client

open LoginTypes

module LoginController = 
    let add (field: string) (value: string) (builder: StringBuilder) = 
        if value <> "" then 
            builder.Append($"{field}={value};") |> ignore
        builder


    let public Connect (details: LoginDetails): SnowflakeDbConnection = 

        // Note that passing the context role, database in the connection string doesn't seem 
        // to work, so we ignore those fields here and set them after we have the connection
        let connString = 
            StringBuilder()
            |> add "account" details.account
            |> add "authenticator" "externalbrowser"
            |> add "proxyHost" details.proxy
            |> add "useProxy" (if details.proxy = "" then "true" else "")
            |> _.ToString()
            
        printfn "Connection String: %s" connString
        let connection = new SnowflakeDbConnection(ConnectionString = connString)
        connection.Open()
        printfn "Connection Successful"

        connection 

module LoginValidation = 
    let validate (input: string) (validator: string -> bool) (message: string) (errors: string list)  = 
        if not (validator input) then message :: errors else errors

    let notEmpty str = str <> ""

    // Other validations to add:
    // Username is an email 
    // Account contains a '-'

    let public ValidateDetails (details: LoginDetails): string list = 
        [] 
        |> validate details.username notEmpty "Username is empty" 
        |> validate details.account  notEmpty "Account is Empty"
