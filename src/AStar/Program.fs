module Program

open prunner
open System.Collections.Generic
open System

type Location =
  {
    x : int
    y : int
  }

let directions =
  [
    { x = 1;  y = 0  }
    { x = 0;  y = -1 }
    { x = -1; y = 0  }
    { x = 0;  y = 1  }
  ]

type SquareGrid =
  {
    width : int
    height : int
    walls : HashSet<Location>
    forests : HashSet<Location>
    cameFrom : Dictionary<Location, Location>
    costSoFar : Dictionary<Location, int>
    path : Location list
  }

let inBounds grid location =
     0 <= location.x && location.x < grid.width
  && 0 <= location.y && location.y < grid.height

let passable grid location = not <| grid.walls.Contains(location)

let cost grid locationA locationB =
  if grid.forests.Contains(locationB)
  then 5
  else 1

let neighbors grid location =
  directions
  |> List.map (fun direction ->
    let next = { x = location.x + direction.x; y = location.y + direction.y }
    if (inBounds grid next) && (passable grid next)
    then Some next
    else None)
  |> List.choose id

let aStar graph start goal =
  //fake a priority queue using lists
  let mutable (frontier : (Location * int) list) = []
  let enqueue value = frontier <- value :: frontier
  let dequeue () =
    let sorted = frontier |> List.sortBy (fun (_, priority) -> priority)
    frontier <- sorted.Tail
    sorted.Head

  let heuristic locationA locationB = Math.Abs(locationA.x - locationB.x) + Math.Abs(locationA.y - locationB.y)

  enqueue (start, 0)
  graph.cameFrom.[start] <- start
  graph.costSoFar.[start] <- 0

  let mutable break' = false
  while frontier.Length > 0 && break' <> true do
    let current, _ = dequeue ()

    if current = goal then break' <- true
    if break' <> true then
      neighbors graph current
      |> List.iter (fun neighbor ->
        let newCost = graph.costSoFar.[current] + cost graph current neighbor
        if (not <| graph.costSoFar.ContainsKey(neighbor)) || newCost < graph.costSoFar.[neighbor] then
          graph.costSoFar.[neighbor] <- newCost
          let priority = newCost + heuristic neighbor goal
          enqueue (neighbor, priority)
          graph.cameFrom.[neighbor] <- current)

  graph

//test helpers

let drawGrid graph start goal =
  let xs = [ 0 .. graph.width - 1  ]
  let ys = [ 0 .. graph.height - 1 ]
  ys |> List.iter (fun y ->
    xs |> List.iter (fun x ->
      let location = { x = x; y = y; }
      let isPath = graph.path |> List.contains location
      let hasValue, outLocation = graph.cameFrom.TryGetValue(location)
      let pointer = if hasValue = false then location else outLocation
      if location = start                      then Console.Write("s ")
      else if location = goal                  then Console.Write("g ")
      else if isPath                           then Console.Write("@ ")
      else if (graph.walls.Contains(location)) then Console.Write("##")
      else if (pointer.x = x + 1)              then Console.Write("→ ")
      else if (pointer.x = x - 1)              then Console.Write("← ")
      else if (pointer.y = y + 1)              then Console.Write("↓ ")
      else if (pointer.y = y - 1)              then Console.Write("↑ ")
      else                                          Console.Write("* "))
    Console.WriteLine())

let reconstructPath (cameFrom : Dictionary<Location, Location>) start goal =
  let mutable current = goal
  let mutable path = [current]
  while current <> start do
    current <- cameFrom.[current]
    path <- current :: path

  path

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
