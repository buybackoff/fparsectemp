// Copyright (c) Stephan Tolksdorf 2008
// License: Simplified BSD License. See accompanying documentation.

// See parser.fs for more information.

open FParsec.CharParsers

open Ast
open Parser
open System

[<EntryPoint>]
let main(args: string[]) =
    // The parser is run on the file path in args.[0].
    // If the file has no byte order marks, System.Text.Encoding.Default
    // is assumed to be the encoding.
    // The parser result will be the abstract syntax tree of the input file.
    let count = 1000_000L;
    let x = Spreads.Utils.Benchmark.Run("xxx", count * 1000L)

    let result =
      if args.Length <> 1 then
          let mutable x = Unchecked.defaultof<_>
          for i in 1L..count do
            x <- parseJsonFile "test_json.txt" System.Text.Encoding.UTF8
          x
      else 
        parseJsonFile args.[0] System.Text.Encoding.UTF8

    // for the moment we just print out the AST
    match result with
    | Success (v, _, _) ->
        printf "The AST of the input file is:\n%A\n" v
        0
    | Failure (msg, err, _) ->
        printfn "%s" msg
        1
    x.Dispose()

    Console.WriteLine("Press enter")
    Console.ReadLine() |> ignore
    0


