namespace SnowShovel 

open System.Collections.Generic

open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Avalonia.Media

open UploadSet

module DataView = 

    type DataState = {
        error: string 
        display: Chunk
        remaining: Chunk option seq
    }

    type Context = IWritable<DataState>

    let getFirstData (source: IDataSource) = 
        match source.load() with 
        | Error(message) -> Error(message)
        | Ok (chunk) -> 
            let data = Seq.tryHead chunk
            let rest = Seq.skip 1 chunk
            match data with 
            | Some(Some(data)) -> Ok (data, rest)
            | _ -> Error ("Failed to read data from the source")


    let initState (source: IDataSource) (ctx: IComponentContext): Context = 
        let emptyState = {
            error = ""
            display = Dictionary()
            remaining = []
        }
        let state = ctx.useState emptyState

        match getFirstData source with 
        | Ok (display, rest) -> 
            state.Set({ state.Current with display = display; remaining = rest })
        | Error msg -> 
            state.Set({ state.Current with error = msg })

        state



    let view (source: IDataSource) = Component.create("DataView", fun ctx -> 
        let state = initState source ctx

        let errorView (state: Context) =
            TextBlock.create [TextBlock.text state.Current.error ]

        let dataView _ = 
            TextBlock.create [ TextBlock.text "Loaded Data" ]

        let placeholderView _ = 
            TextBlock.create [ TextBlock.text "Waiting to Load Data" ]

        if state.Current.error <> "" then 
            errorView state
        else if state.Current.display.Count <> 0 then 
            dataView state
        else 
            placeholderView state

    )

