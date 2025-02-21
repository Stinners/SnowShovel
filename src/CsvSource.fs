namespace SnowShovel 

open FSharp.Data

open UploadSet

module CsvSource = 

    // A data source can just be a type which can yeild a Chunk seq 
    // Then we can walk through it to display or validate the data 
    // And walk through it again to generate the file(s) to upload 
    type CsvSource(file: CsvFile) = 
        // let file = file 

        interface IDataSource
            member LoadChunk (): Chunk option = 
                failwith "Unimplimented"

            member updateChunk (): 



