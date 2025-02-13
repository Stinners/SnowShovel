namespace SnowShovel 

(* This is a hacked together file picker for using in WSL and other 
   environments where there isn't a system filepicker to use *)

open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types

open System.IO

module FilePicker = 

    let public view (pickFile: string -> unit) = Component(fun ctx -> 
        let focusedLine = ctx.useState -1
        let pwd = ctx.useState (Directory.GetCurrentDirectory())

        let renderLine (path: string) (idx: int) (isDir: bool): IView = 
            let mutable text = Path.GetFileName(path)

            let path =
                if path = ".." then Directory.GetParent(pwd.Current).FullName
                else path

            let text =
                if isDir then text + "/"
                else text

            TextBlock.create [
                TextBlock.text text
                TextBlock.padding (Thickness(0, 2))
                TextBlock.fontSize 16.0

                // Handle pointer events
                TextBlock.background (if focusedLine.Current = idx then "gray" else "white")
                TextBlock.onPointerEntered (fun _ -> focusedLine.Set(idx))
                TextBlock.onPointerExited (fun _ -> focusedLine.Set(-1))
                TextBlock.onDoubleTapped(fun _ -> 
                    if isDir then 
                        pwd.Set(path)
                    else 
                        pickFile path
                )
            ]
            (* We need to set the view key to force FuncUI to completly re-render the line each time 
               the directory changes otherwise it fails to unsubscribe the onDoubleTapped event 
               clicking always triggers then event which occupied that location in the original directory *)
            |> View.withKey path
            :> IView 

        let renderLines = 
            let dirs = Directory.GetDirectories(pwd.Current) |> Array.sort |> Array.map (fun path -> path, true)
            let files = Directory.GetFiles(pwd.Current) |> Array.sort |> Array.map (fun path -> path, false)

            Array.concat [[|"..", true|]; dirs; files]
            |> Array.mapi (fun idx (path, isDir) -> renderLine path idx isDir)
            |> Array.toList

        
        StackPanel.create [
            StackPanel.margin (Thickness(5, 0, 0, 0)) 
            StackPanel.children renderLines
        ]
    )

