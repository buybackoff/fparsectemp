// Copyright (c) Stephan Tolksdorf 2008
// License: Simplified BSD License. See accompanying documentation.


module Ast

[<StructuredFormatDisplay("{StructuredFormatDisplay}")>]
type Json = JString of string
          | JNumber of float
          | JBool   of bool
          | JNull
          | JList   of Json list
          | JObject of struct (string * Json) list
         with
            member private t.StructuredFormatDisplay =
                match t with
                | JString s -> box ("\"" + s + "\"")
                | JNumber f -> box f
                | JBool   b -> box b
                | JNull     -> box "null"
                | JList   l -> box l
                | JObject m -> Map.ofList (List.map (fun struct (k,v) -> (k,v)) m) :> obj
