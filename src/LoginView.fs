namespace CounterApp

open System
open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Avalonia.Media

open LoginController
open LoginValidation
open LoginTypes
open DataStore

module Login =

    let trim (str: String) = str.Trim()

    let AuthMethodStrings = ["Browser"; "Key Pair"; "Password"]


    let loginButtonState isValid = 
        if isValid then [ 
            Button.background "green"
            Button.foreground "white"
        ]
        else [
            Button.isEnabled false
        ]

    let view () = Component.create("LoginView", fun ctx ->
        let loginStore: IDataStore<LoginDetails> = LoginStore()
        let initalLoginDetails = loginStore.load "defaultLogin" |> Option.defaultValue emptyLoginDetails

        let loginDetails = ctx.useState initalLoginDetails
        let selectedAuth = ctx.useState AuthMethodStrings[0]
        let validDetails = ctx.useState false


        let createTextBox 
            (title: string)
            (fillerText: string)
            (contents: string)
            updateFunc
            : IView list  = 
            [
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.text title
                    TextBox.margin (Thickness(0, 10, 0, 0))
                ]
                TextBox.create [
                    TextBox.watermark fillerText
                    TextBox.text contents
                    TextBox.onTextChanged (fun contents -> 
                        updateFunc (trim contents)
                        let isValid = ValidateDetails loginDetails.Current = []
                        validDetails.Set(isValid)
                    )
                ]
            ]

        // TODO These need to update the auth field on the loginState

        // TODO this needs to use a password text entry
        let passwordAuthView = 
            (createTextBox
                "Password" 
                "Snowflake Password" 
                loginDetails.Current.role
                (fun contents -> loginDetails.Set({loginDetails.Current with role = contents})))

        // TODO this should use a file picker dialog
        let keyPairAuthView = 
            (createTextBox
                "Key File Path" 
                "Path to Key File" 
                loginDetails.Current.role
                (fun contents -> loginDetails.Set({loginDetails.Current with role = contents})))

        let authView (): IView list = 
            [
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.fontWeight FontWeight.Bold
                    TextBlock.text "Authetication"
                    TextBox.margin (Thickness(0, 20, 0, 0))
                ];
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.text "Type"
                    TextBox.margin (Thickness(0, 10, 0, 0))
                ];
                ComboBox.create [
                    ComboBox.dataItems AuthMethodStrings
                    ComboBox.onSelectedItemChanged (fun contents -> (selectedAuth.Set(string contents)))
                    ComboBox.selectedItem (string selectedAuth.Current)
                ]
            ]
            @ 
            match (string selectedAuth.Current) with 
            | "Browser" -> []
            | "Key Pair" -> keyPairAuthView
            | "Password" -> passwordAuthView
            | other -> failwith $"Value {other} should be impossible"


        StackPanel.create [
            StackPanel.orientation Orientation.Vertical
            StackPanel.margin 50
            StackPanel.width 400
            StackPanel.spacing 5.0
            StackPanel.children (
                (createTextBox 
                    "Username" 
                    "Firstname.Lastname" 
                    loginDetails.Current.username 
                    (fun contents -> loginDetails.Set({loginDetails.Current with username = contents}))) 
                @
                (createTextBox 
                    "Account" 
                    "Account name" 
                    loginDetails.Current.account
                    (fun contents -> loginDetails.Set({loginDetails.Current with account = contents})))
                @
                (createTextBox 
                    "Role" 
                    "Snowflake Role" 
                    loginDetails.Current.role
                    (fun contents -> loginDetails.Set({loginDetails.Current with role = contents})))
                @
                (createTextBox 
                    "Database" 
                    "Snowflake Database" 
                    loginDetails.Current.database
                    (fun contents -> loginDetails.Set({loginDetails.Current with database = contents})))
                @
                (createTextBox 
                    "Schema" 
                    "Snowflake Schema" 
                    loginDetails.Current.schema
                    (fun contents -> loginDetails.Set({loginDetails.Current with schema = contents})))
                @
                (createTextBox 
                    "Warehouse" 
                    "Snowflake Warehouse" 
                    loginDetails.Current.warehouse
                    (fun contents -> loginDetails.Set({loginDetails.Current with warehouse = contents})))
                @ 
                authView ()
                @ 
                [
                    Button.create ([
                        Button.content "Login"
                        Button.onClick(fun _ ->
                            let login = loginDetails.Current
                            Connect loginDetails.Current |> ignore
                            loginStore.write "defaultLogin" login
                        )
                    ]
                    @ 
                    loginButtonState (validDetails.Current)
                    )
                ]
            )
        ]
    )
