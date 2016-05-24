module Program

open prunner

let add a b = a + b

context "Hello world"

"adding two numbers works" &&& fun ctx ->
  add 1 2 == 3
  add 3 2 != 3

run 1 |> ignore

System.Console.ReadKey() |> ignore
