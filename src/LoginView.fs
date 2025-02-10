namespace CounterApp

open System
open System.Runtime.CompilerServices
open System.Timers
open Avalonia
open Avalonia.Animation
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Avalonia.FuncUI
open Avalonia.Media

module Login =

    type AuthMethod = ExternalBrowser
                    | KeyPair of keyFilePath : string
                    | Password of password : string

    type LoginDetails =
        { username: string
          server: string
          database: string
          role: string
          warehouse: string
          proxy: string option
          auth: AuthMethod
        }

    let initState =
        { username = ""
          server = ""
          database = ""
          role = ""
          warehouse = ""
          proxy = Option.None
          auth = ExternalBrowser
        }


    let view () = Component.create("LoginView", fun ctx ->
        let loginDetails = ctx.useState initState

        StackPanel.create [
            StackPanel.orientation Orientation.Vertical
            StackPanel.margin 50
            StackPanel.width 400
            StackPanel.spacing 5.0
            StackPanel.children [
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.text "Foo"
                ]
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.text "Bar"
                ]
                TextBox.create [
                    TextBox.watermark "Username - should be FIRST.LAST@ESR.CRI.NZ"
                    TextBox.text loginDetails.Current.username
                    TextBox.onTextChanged (fun contents -> loginDetails.Set({loginDetails.Current with username = contents}))
                ]
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.text loginDetails.Current.username
                ]
            ]
        ]
    )
