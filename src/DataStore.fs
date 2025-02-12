namespace CounterApp 

// TODO This should probably be abstracted at some point in the
// future but for now we'll write a couple of concrete 
// implimentations and see with commonalities emerge

open System.IO
open System.Text
open System.Collections.Generic
open Newtonsoft.Json

open LoginTypes

module DataStore = 

    let dataStoreDir = ".data"

    let public initDataStore () = 
        Directory.CreateDirectory dataStoreDir |> ignore
    
    let lookup<'t> (name: string) (dataSet: Dictionary<string, 't>) = 
        try Some (dataSet[name]) 
        with | :? KeyNotFoundException -> None

    type public IDataStore<'t> = 
        abstract member load: string -> 't option
        abstract member write: string -> 't -> unit
        abstract member list: unit -> string list

    // Generic Helper Functions //

    let initFile (filePath: string) (initalText: string) = 
        if not (File.Exists(filePath)) then 
            let initalBytes = Encoding.UTF8.GetBytes(initalText)
            let file = File.Create(filePath)
            file.Write(initalBytes)
            file.Dispose()

    let readFile<'t> (filePath: string) = 
        let text = File.ReadAllText(filePath)
        JsonConvert.DeserializeObject<'t>(text)

    let writeFile<'t> (filePath: string) (values: 't) = 
        let text = JsonConvert.SerializeObject(values)
        File.WriteAllText(filePath, text)

    // Interface Implimentations //

    type LoginSet = Dictionary<string, LoginDetails>
    type public LoginStore(?dir: string, ?filename: string) = 
        let dir = Option.defaultValue dataStoreDir dir
        let filename = Option.defaultValue "loginDetails.json" filename
        let filePath = Path.Join(dir, filename)
        do initFile filePath "{}"

        interface IDataStore<LoginDetails> with 
            member this.load name = readFile<LoginSet> filePath |> lookup name

            member this.write name value = 
                let data = readFile<LoginSet> filePath
                data[name] <- value
                writeFile<LoginSet> filePath data

            member this.list () = 
                readFile<LoginSet> filePath
                |> _.Keys  
                |> List.ofSeq





