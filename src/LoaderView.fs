namespace SnowShovel

open System
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Platform.Storage

open Snowflake.Data.Client

module LoaderView = 

    let dialogContents () = Component(fun _ -> 
        StackPanel.create [
            StackPanel.children [ 
                TextBlock.create [
                    TextBlock.text "Model"
                ]
                TextBlock.create [
                    TextBlock.text "View"
                ]
            ]
        ] )

    let view () = Component.create("LoaderView", fun ctx ->


        let dialog = Window()
        dialog.Height <- 300
        dialog.Width <- 200
        dialog.Content <- dialogContents ()

        dialog.Show () 


        Button.create [
            Button.content "Select File"
        ]
    )
