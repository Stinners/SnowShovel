namespace SnowShovel 

open System 
open System.Linq;
open FSharp.Collections
open System.Text.RegularExpressions


module UploadSet = 

    let IntegerRegex = Regex("-?\d+")
    let FloatRegex = Regex("-?\d+(\.\d*)?")  // Doesn't match exponential notation
    let BooleanRegex = Regex("(?i)true|false")

    type SnowflakeType = Integer 
                       | Float
                       | String 
                       | Boolean
                       | Date
                       | DateTime

    let allSnowflakeTypes = 
        Set([Integer; String; Boolean; Date; DateTime])


    let validators snowflakeType (value: String) = 
        match snowflakeType with 
        | Integer -> IntegerRegex.IsMatch(value)
        | Float -> FloatRegex.IsMatch(value)
        | Boolean -> BooleanRegex.IsMatch(value)

        // Currently not supported
        | Date -> false 
        | DateTime -> false

        // Allways true
        | String -> true


    // A Chunk represents a subset of rows from a data source 
    // as a bare bones-data frame object.
    // For the moment all data is stored as strings, before undergoing 
    // validation
    // Column contains both the data and the metadata about a single column 
    type Column = 
        { data:      string array
          candidates: Set<SnowflakeType>
          nullable:  bool
        }

    type Chunk = Collections.Generic.Dictionary<string, Column>


    type IDataSource =
        abstract member LoadChunk: unit -> Chunk option 
        abstract member Name: unit -> string

    // TODO figure out how to handle errors in this part of the code
    let initChunk (source:IDataSource) = 
        match source.LoadChunk() with 
        | Some chunk -> chunk
        | None -> failwith $"Failed to load data from source: {source.Name()}"

    let updateChunk (oldChunk:Chunk) (newChunk:Chunk) = 
        let oldKeys = oldChunk.Keys
        let newKeys = oldChunk.Keys

        if Set(oldKeys) <> Set(newKeys) then 
            failwith "New Chunk has different column set to old chunk"

        for key in newKeys do 
            oldChunk[key] <- { oldChunk[key] with data = newChunk[key].data }

