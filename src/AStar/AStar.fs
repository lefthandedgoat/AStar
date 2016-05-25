module AStar

open System
open System.Collections.Generic
open types

let inBounds grid node =
     0 <= node.x && node.x < grid.width
  && 0 <= node.y && node.y < grid.height

let passable node = node.weight <> Blocked

let cost nodeB =
  match nodeB.weight with
  | Weight(weight) -> weight
  | Blocked -> failwith "cost function should never be called with a blocked node"

let neighbors grid node =
  directions
  |> List.map (fun direction ->
    let next = { x = node.x + direction.x; y = node.y + direction.y; weight = Blocked }
    if (inBounds grid next) && (passable next)
    then Some next
    else None)
  |> List.choose id

//manhattan distance
let manhattanHeuristic nodeA nodeB = Math.Abs(nodeA.x - nodeB.x) + Math.Abs(nodeA.y - nodeB.y)

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
        let newCost = graph.costSoFar.[current] + cost neighbor
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
