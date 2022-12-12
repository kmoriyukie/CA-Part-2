using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PathFinding;

public class Grid_A_Star : A_Star<GridCell, CellConnection, GridConnections, Grid, GridHeuristic>
{
	// Class that implements the A* pathfinding algorithm	
	// over a Grid graph, componsed of GridCells and CellConnections
	// using GridHeuristic as the Heuristic function.

	// NOTHING TO DO HERE

	public Grid_A_Star(int maxNodes, float maxTime, int maxDepth) : base(maxNodes, maxTime, maxDepth)
	{

	}
};