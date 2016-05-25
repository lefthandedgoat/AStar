module helpers

open System.Collections.Generic
open types

//http://www.redblobgames.com/pathfinding/a-star/introduction.html#dijkstra
let makeMovementCostExmapleGraph () =
  let graph =
    {
      width = 10
      height = 10
      cameFrom = Dictionary<Node, Node>()
      costSoFar = Dictionary<Node, int>()
      path = []
    }

  //// Make "diagram 4" from main article
  //[1 .. 3] |> List.iter (fun x ->
  //  [7 .. 8] |> List.iter (fun y ->
  //    graph.walls.Add({ x = x; y = y }) |> ignore))
//
//  //let add node = graph.forests.Add(node) |> ignore
//  //add { x = 3; y = 4; }
//  //add { x = 3; y = 5; }
//  //add { x = 4; y = 1; }
//  //add { x = 4; y = 2; }
//  //add { x = 4; y = 3; }
//  //add { x = 4; y = 4; }
//  //add { x = 4; y = 5; }
//  //add { x = 4; y = 6; }
//  //add { x = 4; y = 7; }
//  //add { x = 4; y = 8; }
//  //add { x = 5; y = 1; }
//  //add { x = 5; y = 2; }
//  //add { x = 5; y = 3; }
//  //add { x = 5; y = 4; }
//  //add { x = 5; y = 5; }
//  //add { x = 5; y = 6; }
//  //add { x = 5; y = 7; }
//  //add { x = 5; y = 8; }
//  //add { x = 6; y = 2; }
//  //add { x = 6; y = 3; }
//  //add { x = 6; y = 4; }
//  //add { x = 6; y = 5; }
//  //add { x = 6; y = 6; }
//  //add { x = 6; y = 7; }
//  //add { x = 7; y = 3; }
//  //add { x = 7; y = 4; }
  //add { x = 7; y = 5; }

  graph
