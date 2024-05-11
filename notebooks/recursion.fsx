type Ordering =
    | Less
    | Equal
    | Greater

module BigInteger =

    open System.Numerics

    let fromInt (i: int) = BigInteger i
    let Zero = fromInt 0
    let One = fromInt 1

    let compare (other: bigint) (one: bigint) =
        match one.CompareTo(other) with
        | 0 -> Equal
        | n when n > 0 -> Greater
        | _ -> Less

    let rec fac (n: bigint) =
        match (n |> compare (BigInteger.Zero)) with
        | Less -> failwithf "negative input: %A" n
        | Equal -> BigInteger.One
        | Greater -> n * (fac (n - BigInteger.One))

let fibThreadingTail n =
    let rec f d n c =
        match d |> Map.tryFind n with
        | Some result -> (result, d) |> c
        | None ->
            match n with
            | 1 ->
                (BigInteger.One, d |> Map.add 1 BigInteger.One)
                |> c
            | 2 ->
                (BigInteger.One, d |> Map.add 2 BigInteger.One)
                |> c
            | _ ->
                f d (n - 1) (fun (n1, d1) ->
                    f d1 (n - 2) (fun (n2, d2) ->
                        let result = n1 + n2
                        let nd = d2 |> Map.add n result
                        (result, nd) |> c))

    let (result, _) = f Map.empty n id
    result

#time
fibThreadingTail 100_000
