module Types

type IBuild<'a,'T> =
    abstract member Build : 'a -> ('a -> bool) -> 'T
    abstract member IsValid : bool with get
    
//non-sense crazyness
type GenericBuilder<'a, 'T  when 'T :> IBuild<'a,'T> and 'T : (new : unit -> 'T)>(validatingFunc) =
    member __.Return(x: 'a) = (new 'T() :> IBuild<'a,'T>).Build x validatingFunc
    member __.Delay(f) = f()
    member __.Bind(x,f) = f(x)

type MaybeBuilder() =
    member __.Bind(x, f) =
        match x with
        | Some(x) -> f(x)
        | _ -> None
    member __.Delay(f) = f()
    member __.Return(x) = Some x

type SimpleDoMonad<'T>(someSideEffect: 'T -> unit) =
    member __.Bind(x,f) = 
        let _ = __.Return(x)
        f(x)
    member __.Return(x) =
        someSideEffect(x) |> ignore
        x
    

let maybe = MaybeBuilder()


