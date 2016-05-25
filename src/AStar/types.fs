module types

open System.Collections.Generic

type Node =
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
    walls : HashSet<Node>
    forests : HashSet<Node>
    cameFrom : Dictionary<Node, Node>
    costSoFar : Dictionary<Node, int>
    path : Node list
  }
