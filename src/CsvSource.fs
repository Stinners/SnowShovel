namespace SnowShovel

open System.Collections.Generic
open System.IO

open FSharp.Data

open UploadSet

module CsvSource =

    type RowSet = CsvRow array
    type Headers = string array

    type CsvSource(file: string) =
        let filename = file
        let filepath = Path.Join(Directory.GetCurrentDirectory(), file)
        let batchSize = 500

        let transposeRows (headers: Headers) (rows: RowSet): Chunk option =
            let chunk = Dictionary<string, List<string>>()
            for header in headers do
                chunk[header] <- List<string>(500)

            // A CSV file should always have the the same set of headers
            // But we still need to deal will malformed files
            for row in rows do
                for header in headers do
                    let value = row[header]
                    chunk[header].Add(value)

            Some chunk


        interface IDataSource with

            member self.load(): Result<Chunk option seq, string>  =
                try
                    let file = CsvFile.Load(filepath)
                    let rows = Seq.windowed batchSize file.Rows
                    let headers = file.Headers

                    match headers with
                    | None -> Error (sprintf "Csv File '%s' does not have headers" filepath)
                    | Some(headers) ->
                        let data = seq {
                            for window in rows -> transposeRows headers window
                        }
                        Ok data
                with
                | :? System.IO.FileNotFoundException ->
                    Error (sprintf "Csv File '%s' does not exist" filepath)
                | ex when ex.Message.StartsWith("Invalid CSV file") ->
                    Error (sprintf "Csv File %s: %s" filename ex.Message)

            member self.name(): string = filename
            member self.sourceType(): string = "CSV File"




