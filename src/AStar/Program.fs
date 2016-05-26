module Program

open System.Collections.Generic
open prunner
open System
open types
open AStar
open printing

//test helpers

context "A* tests"

//this test is the example from the below blog
//http://www.redblobgames.com/pathfinding/a-star/introduction.html#dijkstra
"Example test" &&& fun ctx ->
  let width = 10
  let height = 10
  let graph = helpers.makeMovementCostExmapleGraph width height
  let start = { x = 1; y = 4; weight = Weight(1) }
  let goal =  { x = 8; y = 5; weight = Weight(1) }

  let drawGrid graph = drawRectangularGrid width height graph start goal ctx.print ctx.printn
  ctx.printn "empty graph"
  drawGrid graph
  ctx.printn ""

  ctx.printn "graph with path"
  let results = aStar graph start goal manhattanHeuristic
  let graph = { graph with path = reconstructPath graph.cameFrom start goal}
  drawGrid graph

run 1 |> ignore

System.Console.ReadKey() |> ignore
