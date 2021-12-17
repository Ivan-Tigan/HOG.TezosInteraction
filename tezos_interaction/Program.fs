// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System

// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom
type ('a, 'b) errmonad = ('a -> 'b) -> (string -> 'b) -> 'b 
let ok (x:'a) (s:'a -> 'b) (f:string -> 'b) : 'b = s x
let err (msg:string) (s:'a -> 'b) (f:string -> 'b) : 'b = f msg
let bind (x:errmonad<'x, errmonad<'y, 'e2>>) (fm: 'x -> errmonad<'y, 'e2>) : errmonad<'y, 'e2> =
    x (fun okx -> fm okx) (fun msg s f -> f msg)

let x<'x> : errmonad<int, 'x> = ok 3
let y<'x> : errmonad<int, 'x> = ok 4
let z<'x> : errmonad<int, 'x> =
    bind x (fun x ->
    bind y (fun y ->
    ok (x * y)))
let run (x:errmonad<'x, 'x>) = x (fun (x:'a) -> x) (fun msg -> failwithf "%A" msg)

[<EntryPoint>]
let main argv =
    
    printfn "Hello world %A" (run z)
    0 // return an integer exit code