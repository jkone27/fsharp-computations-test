// Learn more about F# at http://fsharp.org

open System
open Types

type Test(s, validationFunction) = 
    member val Property : string = s with get, set
    new() =
        Test("", fun _ -> true)
    new(str) = Test(str, fun _ -> true)
    interface IBuild<string,Test> with 
        member __.Build x validationFunction = Test(x, validationFunction)
        member __.IsValid = validationFunction(__.Property)

let testBuilder = GenericBuilder<string,Test>(fun x -> x.Contains("true"))

let log = SimpleDoMonad<int>(fun x -> Console.WriteLine(x))

[<EntryPoint>]
let main argv =

    let r = maybe {
        let x = Some 1
        let! y = x
        return 1
    }

    let x = testBuilder {
        let x = new Test("hello")
        let! x1 = x
        let r = x1.Property
        return r
    }

    let r = log {
        let! x = 1 //logs
        let y = 2 //doesnt
        return (x + y) //logs
    }

    printfn "Hello World from F#!"
    0 // return an integer exit code
