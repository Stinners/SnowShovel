namespace SnowShovel 

open System 
open FSharp.Collections
open System.Text.RegularExpressions
open System.Collections


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
    // This is just a very primitive dataframe 
    // All data is stored as strings for now - before validation
    type Chunk = Collections.Generic.Dictionary<string, Generic.List<string>>

    type DataSource = Chunk seq 

    // Represents what we know about a single column 
    type ColumnSchema = 
        { candiates: Set<SnowflakeType> 
          nullable: bool }

    // Represents What we know about the whole dataset 
    type TableKnowledge = Collections.Generic.Dictionary<string, ColumnSchema>
          

    type IDataSource =
        abstract member load: unit -> Result<Chunk option seq, string>
        abstract member name: unit -> string
        abstract member sourceType: unit -> string

