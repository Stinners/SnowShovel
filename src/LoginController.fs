namespace SnowShovel 

open System.Text
open Snowflake.Data.Client

open LoginTypes
open Utils
open Validators

module LoginController = 

    // Other validations to add:
    // Username is an email 
    // Account contains a '-'
    let public ValidateDetails (details: LoginDetails): string list = 
        [] 
        |> validateStr details.username notEmpty "Username is empty" 
        |> validateStr details.username isEmail "Username is not an email"
        |> validateStr details.account  notEmpty "Account is Empty"


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
