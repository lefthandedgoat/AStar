module printing

open types
open AStar

let drawGrid graph start goal printer printern =
  let xs = [ 0 .. graph.width - 1  ]
  let ys = [ 0 .. graph.height - 1 ]
  ys |> List.iter (fun y ->
    xs |> List.iter (fun x ->
      let node = { x = x; y = y; weight = Blocked }
      let isPath = graph.path |> List.contains node
      let isForest = cost node = 5
      let hasValue, outNode = graph.cameFrom.TryGetValue(node)
      let pointer = if hasValue = false then node else outNode
      if node = start                          then printer "s "
      else if node = goal                      then printer "g "
      else if isPath                           then printer "@ "
      else if isForest                         then printer "🌲 "
      else if node.weight = Blocked            then printer "##"
      else if (pointer.x = x + 1)              then printer "→ "
      else if (pointer.x = x - 1)              then printer "← "
      else if (pointer.y = y + 1)              then printer "↓ "
      else if (pointer.y = y - 1)              then printer "↑ "
      else                                          printer "* ")
    printern "")
