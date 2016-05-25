module printing

open types
open AStar

let drawRectangularGrid width height graph start goal printer printern =
  let xs = [ 0 .. width - 1  ]
  let ys = [ 0 .. height - 1 ]
  ys |> List.iter (fun y ->
    xs |> List.iter (fun x ->
      let node = graph.nodes.[(x, y)]
      let isPath = graph.path |> List.contains node
      let hasValue, outNode = graph.cameFrom.TryGetValue(node)
      let pointer = if hasValue = false then node else outNode
      if node = start                          then printer "s "
      else if node = goal                      then printer "g "
      else if isPath                           then printer "@ "
      else if node.weight = helpers.tree       then printer "🌲 "
      else if node.weight = helpers.blocked    then printer "##"
      else if (pointer.x = x + 1)              then printer "→ "
      else if (pointer.x = x - 1)              then printer "← "
      else if (pointer.y = y + 1)              then printer "↓ "
      else if (pointer.y = y - 1)              then printer "↑ "
      else                                          printer "* ")
    printern "")
