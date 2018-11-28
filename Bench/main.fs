// Copyright (c) Stephan Tolksdorf 2008
// License: Simplified BSD License. See accompanying documentation.

// See parser.fs for more information.

open FParsec.CharParsers

open Ast
open Parser
open System
open System.Collections.Generic

[<EntryPoint>]
let main(args: string[]) =
    // The parser is run on the file path in args.[0].
    // If the file has no byte order marks, System.Text.Encoding.Default
    // is assumed to be the encoding.
    // The parser result will be the abstract syntax tree of the input file.
    let count = 1_000_000L;
    //let x = Spreads.Utils.Benchmark.Run("xxx", count * 1000L)
    let list = new List<_>(int count)
    let result =
      use b = Spreads.Utils.Benchmark.Run("xxx", count)
      // if args.Length <> 1 then
      let mutable x = Unchecked.defaultof<_>
      for i in 1L..count do
        x <- parseJsonString "{\"a\": 123, \"b\": true}" // , \"c\": null, \"d\": {\"nested\":\"asd\"} }"
        // list.Add(x)
      x
      //else 
      //  parseJsonString "{\"a\": 123, \"b\": true }"

    // for the moment we just print out the AST
    match result with
    | Success (struct (v, _, _)) ->
        printf "The AST of the input file is:\n%A\n" v
        
    | Failure (struct (msg, err, _)) ->
        printfn "%s" msg
        
    // x.Dispose()

    Console.WriteLine("Press enter: " + list.Count.ToString())
    Console.ReadLine() |> ignore
    0


