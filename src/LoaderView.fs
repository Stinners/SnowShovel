namespace SnowShovel

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL

open Snowflake.Data.Client

module LoaderView = 
    let view () = Component.create("LoaderView", fun ctx ->
        let loadedFile: IWritable<string option> = ctx.useState None

        let pickFile () = 
            let dialog = Window()

            let updateSelectedFile filePath = 
                loadedFile.Set (Some filePath)
                dialog.Close()

            dialog.Height <- 300
            dialog.Width <- 200
            dialog.Content <- (FilePicker.view updateSelectedFile)

            dialog.Show () 


        Button.create [
            Button.content "Select File"
            Button.onClick (fun _ -> pickFile ())
        ]
    )
