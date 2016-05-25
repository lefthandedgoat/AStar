module AStar

open System
open System.Collections.Generic
open types

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

//manhattan distance
let manhattanHeuristic locationA locationB = Math.Abs(locationA.x - locationB.x) + Math.Abs(locationA.y - locationB.y)

let aStar graph start goal heuristic =
  //frontier are the items that you need to explore in the future
  let mutable (frontier : (Node * int) list) = []
  //fake a priority queue using lists
  let enqueue value = frontier <- value :: frontier
  let dequeue () =
    let sorted = frontier |> List.sortBy (fun (_, priority) -> priority)
    frontier <- sorted.Tail
    sorted.Head

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

let reconstructPath (cameFrom : Dictionary<Node, Node>) start goal =
  let mutable current = goal
  let mutable path = [current]
  while current <> start do
    current <- cameFrom.[current]
    path <- current :: path

  path
