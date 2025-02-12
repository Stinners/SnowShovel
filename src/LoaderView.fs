namespace SnowShovel

open System
open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Platform.Storage

open Snowflake.Data.Client

module LoaderView = 
    let view (conn: SnowflakeDbConnection) = Component.create("LoaderView", fun ctx ->

        let topLevel = TopLevel.GetTopLevel ctx.control

        let useFiles () =
            async {
              let options = FilePickerOpenOptions(
                  Title = "Open Text File",
                  AllowMultiple = false
              ) 
                 
              Console.WriteLine("Opening File Picker")
              let! result = topLevel.StorageProvider.OpenFilePickerAsync(options) |> Async.AwaitTask
              Console.WriteLine("Done with File Picker")
              result |> ignore
            } 

        useFiles () |> Async.StartImmediate

                    
        TextBlock.create [
            TextBlock.dock Dock.Top
            TextBlock.text "Connected!"
            TextBox.margin (Thickness(0, 10, 0, 0))
        ]
    )
