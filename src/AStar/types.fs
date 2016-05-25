module types

open System.Collections.Generic

type Weight =
  | Blocked
  | Weight of weight:int

type Node =
  {
    x : int
    y : int
    weight : Weight
  }

let directions =
  [
    { x =  1; y =  0; weight = Blocked }
    { x =  0; y = -1; weight = Blocked }
    { x = -1; y =  0; weight = Blocked }
    { x =  0; y =  1; weight = Blocked }
  ]

type Graph =
  {
    nodes : Dictionary<(int * int), Node>
    cameFrom : Dictionary<Node, Node>
    costSoFar : Dictionary<Node, int>
    path : Node list
  }
