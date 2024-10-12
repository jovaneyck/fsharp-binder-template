type Validation<'a> = Result<'a, string list>
type Age = Age of int
type Email = Email of string

let parseAge (text: string) : Validation<Age> =
    match System.Int32.TryParse text with
    | false, _ -> Error [ $"<%s{text}> is not a valid age" ]
    | true, parsed -> Ok(Age parsed)

let parseEmail (text: string) : Validation<Email> =
    if text.Contains("@") then
        Ok(Email text)
    else
        Error [ $"<%s{text}> is not a valid email" ]

let okAge = parseAge "12"
let nokAge = parseAge "jo@gmail.com"
let okEmail = parseEmail "jo@gmail.com"
let nokEmail = parseEmail "13"

let haveBirthDay (Age a) : Age = Age(a + 1)

//Functor: meet map
//map :: (a->b)->E(a)->E(b)
let map (f: 'a -> 'b) (a: Validation<'a>) : Validation<'b> =
    match a with
    | Ok v -> Ok(f v)
    | Error e -> Error e

parseAge "12" |> map haveBirthDay
parseAge "jo" |> map haveBirthDay

//Applicative: meet apply
//apply :: E(a->b)->E(a)->E(b)
let apply (f: Validation<'a -> 'b>) (a: Validation<'a>) : Validation<'b> =
    match f, a with
    | Ok f, Ok v -> Ok(f v)
    | Error e1, Error e2 -> Error(e1 @ e2)
    | Error e, _ -> Error e
    | _, Error e -> Error e

let (<*>) = apply

type Person = { Age: Age; Email: Email }
let mkPerson (a: Age) (e: Email) : Person = { Age = a; Email = e }

apply (apply (Ok mkPerson) (parseAge "13")) (parseEmail "jo@gmail.com")

(Ok mkPerson)
<*> (parseAge "13")
<*> (parseEmail "jo@gmail.com")

let (<!>) = map
//If you squint, you see the regular function application!
mkPerson <!> (parseAge "13")
<*> (parseEmail "jo@gmail.com")

mkPerson <!> (parseAge "dertien")
<*> (parseEmail "ongeldig")

//Monad: hello bind!
//World-crossing functions are EVERYWHERE: a -> E(b)
//Let's take a look at map again:
//map :: (a->b)->E(a)->E(b)
//Let's do something weird and plug in an elevated value for type b:
//       (a->E(b))->E(a)->E(E(b))


//The sucky part here is the E(E(b)), nobody likes to work with
//Results of results, lists of lists, asyncs of asyncs
//What can we do to make this workable?


//SMOOSING, flattening, collecting, selectmany'ing ofcourse!
//This is what the "bind" function is for, it does the same as map but flattens the result:
//bind :: (a->E(b))->E(a)->E(b)
let bind (f: 'a -> Validation<'b>) (a: Validation<'a>) : Validation<'b> =
    match a with
    | Ok v -> f v
    | Error e -> Error e

parseAge "13"
|> bind (fun age ->
    parseEmail "jo@gmail.com"
    |> bind (fun email -> Ok(mkPerson age email)))

//For F# "do syntax", we need some magic dust
type ValidatedResultBuilder() =
    member _.Return(x) = Ok x
    member _.Bind(m, f) = bind f m

let valid = ValidatedResultBuilder()

//This is identical to the bind result, but with nicer syntax!
valid {
    let! age = parseAge "13"
    let! email = parseEmail "jo@gmail.com"
    return mkPerson age email
}

valid   {
    let! age = parseAge "invalid"
    let! email = parseEmail "invalid"
    return mkPerson age email
}

//Applicative style vs monadic style: independent/parallel vs dependent/sequential style
//Other concrete example:
//Async: get html for different URL's
//applicative: parallellization possible (e.g. sum of page sizes)
//monadic: sequence of steps (e.g. web scraper that recursively scrapes)
