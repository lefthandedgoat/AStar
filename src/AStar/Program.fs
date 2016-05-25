module Program

open System.Collections.Generic
open prunner
open System
open types
open AStar
open printing

//test helpers

context "A* tests"

"Example test" &&& fun ctx ->
  let graph =
    {
      width = 10
      height = 10
      walls = HashSet<Location>()
      forests = HashSet<Location>()
      cameFrom = Dictionary<Location, Location>()
      costSoFar = Dictionary<Location, int>()
      path = []
    }

  // Make "diagram 4" from main article
  [1 .. 3] |> List.iter (fun x ->
    [7 .. 8] |> List.iter (fun y ->
      graph.walls.Add({ x = x; y = y }) |> ignore))

  let add location = graph.forests.Add(location) |> ignore
  add { x = 3; y = 4; }
  add { x = 3; y = 5; }
  add { x = 4; y = 1; }
  add { x = 4; y = 2; }
  add { x = 4; y = 3; }
  add { x = 4; y = 4; }
  add { x = 4; y = 5; }
  add { x = 4; y = 6; }
  add { x = 4; y = 7; }
  add { x = 4; y = 8; }
  add { x = 5; y = 1; }
  add { x = 5; y = 2; }
  add { x = 5; y = 3; }
  add { x = 5; y = 4; }
  add { x = 5; y = 5; }
  add { x = 5; y = 6; }
  add { x = 5; y = 7; }
  add { x = 5; y = 8; }
  add { x = 6; y = 2; }
  add { x = 6; y = 3; }
  add { x = 6; y = 4; }
  add { x = 6; y = 5; }
  add { x = 6; y = 6; }
  add { x = 6; y = 7; }
  add { x = 7; y = 3; }
  add { x = 7; y = 4; }
  add { x = 7; y = 5; }

  let start = { x = 1; y = 4; }
  let goal =  { x = 8; y = 5; }

  System.Console.WriteLine("empty graph")
  drawGrid graph start goal
  System.Console.WriteLine("")

  System.Console.WriteLine("graph with results")
  let results = aStar graph start goal
  drawGrid results start goal
  System.Console.WriteLine("")

  System.Console.WriteLine("graph with path")
  let graph = { graph with path = reconstructPath graph.cameFrom start goal}
  drawGrid graph start goal

run 1 |> ignore

System.Console.ReadKey() |> ignore
