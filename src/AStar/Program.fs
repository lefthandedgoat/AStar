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
    let min = sorted |> List.head
    let tail = sorted |> List.tail
    frontier <- tail
    min

  let heuristic locationA locationB = Math.Abs(locationA.x - locationB.x) + Math.Abs(locationA.y - locationB.y)

  enqueue (start, 0)
  graph.cameFrom.[start] <- start
  graph.costSoFar.[start] <- 0

  while frontier.Length > 0 do
    let current, priority = dequeue ()

    if current <> goal then
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
      let hasValue, outLocation = graph.cameFrom.TryGetValue(location)
      let pointer = if hasValue = false then location else outLocation
      if location = start                      then Console.Write("s ")
      else if location = goal                  then Console.Write("g ")
      else if (graph.walls.Contains(location)) then Console.Write("##")
      else if (pointer.x = x + 1)              then Console.Write("→ ")
      else if (pointer.x = x - 1)              then Console.Write("← ")
      else if (pointer.y = y + 1)              then Console.Write("↓ ")
      else if (pointer.y = y - 1)              then Console.Write("↑ ")
      else                                          Console.Write("* "))
    Console.WriteLine())

"Example test" &&& fun ctx ->
  let graph =
    {
      width = 10
      height = 10
      walls = HashSet<Location>()
      forests = HashSet<Location>()
      cameFrom = Dictionary<Location, Location>()
      costSoFar = Dictionary<Location, int>()
    }

  // Make "diagram 4" from main article
  [1 .. 3] |> List.iter (fun x ->
    [7 .. 8] |> List.iter (fun y ->
      graph.walls.Add({ x = x; y = y }) |> ignore))

  graph.forests.Add { x = 3; y = 4; } |> ignore
  graph.forests.Add { x = 3; y = 5; } |> ignore
  graph.forests.Add { x = 4; y = 1; } |> ignore
  graph.forests.Add { x = 4; y = 2; } |> ignore
  graph.forests.Add { x = 4; y = 3; } |> ignore
  graph.forests.Add { x = 4; y = 4; } |> ignore
  graph.forests.Add { x = 4; y = 5; } |> ignore
  graph.forests.Add { x = 4; y = 6; } |> ignore
  graph.forests.Add { x = 4; y = 7; } |> ignore
  graph.forests.Add { x = 4; y = 8; } |> ignore
  graph.forests.Add { x = 5; y = 1; } |> ignore
  graph.forests.Add { x = 5; y = 2; } |> ignore
  graph.forests.Add { x = 5; y = 3; } |> ignore
  graph.forests.Add { x = 5; y = 4; } |> ignore
  graph.forests.Add { x = 5; y = 5; } |> ignore
  graph.forests.Add { x = 5; y = 6; } |> ignore
  graph.forests.Add { x = 5; y = 7; } |> ignore
  graph.forests.Add { x = 5; y = 8; } |> ignore
  graph.forests.Add { x = 6; y = 2; } |> ignore
  graph.forests.Add { x = 6; y = 3; } |> ignore
  graph.forests.Add { x = 6; y = 4; } |> ignore
  graph.forests.Add { x = 6; y = 5; } |> ignore
  graph.forests.Add { x = 6; y = 6; } |> ignore
  graph.forests.Add { x = 6; y = 7; } |> ignore
  graph.forests.Add { x = 7; y = 3; } |> ignore
  graph.forests.Add { x = 7; y = 4; } |> ignore
  graph.forests.Add { x = 7; y = 5; } |> ignore

  let start = { x = 1; y = 4; }
  let goal =  { x = 8; y = 5; }

  drawGrid graph start goal
  System.Console.WriteLine("")
  let results = aStar graph start goal
  drawGrid results start goal

run 1 |> ignore

System.Console.ReadKey() |> ignore
