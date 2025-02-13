namespace SnowShovel 

open System.IO

module Utils = 

    let isWDL = File.Exists("/proc/sys/fs/binfmt_misc/WSLInterop")

