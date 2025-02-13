namespace SnowShovel 

open System.IO
open System.Text

module Utils = 

    let isWDL = File.Exists("/proc/sys/fs/binfmt_misc/WSLInterop")

    let trim (str: string) = str.Trim()

    let add (field: string) (value: string) (builder: StringBuilder) = 
        if value <> "" then 
            builder.Append($"{field}={value};") |> ignore
        builder

module Validators = 

    let validate<'t> (input: 't) (validator: 't -> bool) (message: string) (errors: string list) = 
        if not (validator input) then message :: errors else errors

    let public validateStr = validate<string>

    let public notEmpty str = str <> ""
    let public isEmail str = System.Net.Mail.MailAddress(str).Address = str
