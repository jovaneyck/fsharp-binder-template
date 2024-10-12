#r "nuget: Plotly.NET, 5.0.0"

open Plotly.NET
open Plotly.NET.TraceObjects

let maxIterations = 650
let half_width = 300

type Complex = decimal * decimal

module Complex =
    let add ((a, b): Complex) ((c, d): Complex) : Complex = (a + c, b + d)

    let square ((a, b): Complex) : Complex =
        //(a + bi)2
        //(a + bi)(a + bi)
        //a2 + abi + abi + b2i2
        //a2 + 2abi -b2
        //a2 -b2 + 2abi
        ((a * a - b * b), (2m * a * b))

    let modulus ((a, b): Complex) : double =
        //mod of complex number == pythagoras
        let x = a * a + b * b
        sqrt (double x)

let isBounded (c: Complex) : bool =
    //z0 = 0
    //zn+1 = zn*zn + c remains bounded (mod(z) < 2)

    let rec isBounded n zn =
        let zn1 = Complex.add (Complex.square zn) c

        if n > maxIterations then
            true
        else if Complex.modulus zn1 > 2 then
            false
        else
            isBounded (n + 1) zn1

    isBounded 1 (0m, 0m)

let partOfSet (row, column) = isBounded (row, column)

let points =
    [ for r in (-half_width) .. half_width do
          for c in (-half_width) .. half_width ->
              ((decimal r) / (decimal half_width), (decimal c) / (decimal half_width)) ]
    |> Seq.filter partOfSet

let marker = Marker.init (Size = 3)
let chart = Chart.Point(xy = points, Marker = marker)
chart |> Chart.show
