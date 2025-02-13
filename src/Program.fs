namespace SnowShovel

open System
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI

open Snowflake.Data.Client

open DataStore

module Main =

    let view () = Component(fun ctx ->
        let conn: IWritable<SnowflakeDbConnection option> = ctx.useState Option.None

        let setConnection newConn = conn.Set(Some(newConn))

        match conn.Current with 
        //| None -> Login.view setConnection
        | None | Some(_) -> LoaderView.view ()
    )

type MainWindow() =
    inherit HostWindow()
    do
        base.Title <- "Snowshovel"
        base.Content <- Main.view ()

type App() =
    inherit Application()

    override this.Initialize() = 
        this.Styles.Add (FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Light
        ()

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        initDataStore () |> ignore
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)

