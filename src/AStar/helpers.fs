module helpers

open System.Collections.Generic
open types

let blocked = Blocked
let river  = Weight(4)
let forest  = Weight(5)

let generateRectangularGrid width height =
  let nodes = Dictionary<(int * int), Node>()
  [ 0 .. height - 1 ] |> List.iter (fun y ->
    [ 0 .. width - 1  ] |> List.iter (fun x ->
      nodes.Add((x, y), { x = x; y = y; weight = Weight(1) }) |> ignore))

  {
    nodes = nodes
    cameFrom = Dictionary<Node, Node>()
    costSoFar = Dictionary<Node, int>()
    path = []
  }

let verticalLine graph x startY endY weight =
  [ startY .. endY ]
  |> List.iter ( fun y -> graph.nodes.[(x, y)] <- { x = x; y = y; weight = weight } )

let horizontalLine graph startX endX y weight =
  [ startX .. endX ]
  |> List.iter ( fun x -> graph.nodes.[(x, y)] <- { x = x; y = y; weight = weight } )

//http://www.redblobgames.com/pathfinding/a-star/introduction.html#dijkstra
let makeMovementCostExmapleGraph width height =
  let graph = generateRectangularGrid width height

  horizontalLine graph 1 3 7 blocked
  horizontalLine graph 1 3 8 blocked
  verticalLine   graph 3 4 5 forest
  verticalLine   graph 4 1 8 forest
  verticalLine   graph 5 1 8 river
  verticalLine   graph 6 2 7 forest
  verticalLine   graph 7 3 5 forest

  graph
