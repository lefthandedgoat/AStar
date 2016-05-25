module types

open System.Collections.Generic

type Location =
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
    walls : HashSet<Location>
    forests : HashSet<Location>
    cameFrom : Dictionary<Location, Location>
    costSoFar : Dictionary<Location, int>
    path : Location list
  }
