module printing

open types

let drawGrid graph start goal printer printern =
  let xs = [ 0 .. graph.width - 1  ]
  let ys = [ 0 .. graph.height - 1 ]
  ys |> List.iter (fun y ->
    xs |> List.iter (fun x ->
      let location = { x = x; y = y; }
      let isPath = graph.path |> List.contains location
      let hasValue, outLocation = graph.cameFrom.TryGetValue(location)
      let pointer = if hasValue = false then location else outLocation
      if location = start                      then printer "s "
      else if location = goal                  then printer "g "
      else if isPath                           then printer "@ "
      else if (graph.walls.Contains(location)) then printer "##"
      else if (pointer.x = x + 1)              then printer "→ "
      else if (pointer.x = x - 1)              then printer "← "
      else if (pointer.y = y + 1)              then printer "↓ "
      else if (pointer.y = y - 1)              then printer "↑ "
      else                                          printer "* ")
    printern "")
