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

    public override List<GridCell> findpath(Grid graph, GridCell start, GridCell end, GridHeuristic heuristic, ref int found)
    {
		List<GridCell> path = new List<GridCell>();
			
			// TO IMPLEMENT
		Queue open = new Queue();
		List<GridCell> closed = new List<GridCell>();

		open.Add(new NodeRecord(start));
		NodeRecord current;
		
		float cost = 0;

		while (open.getLowestCostNode() != null && open.getLowestCostNode().node != end)
		{
			current = open.getLowestCostNode();
			open.Remove(current);
			closed.Add(current.node);

			visitedNodes.Add(current.node);
			foreach (var con in graph.getConnections(current.node).connections)
			{
				cost = con.cost + heuristic.estimateCost(con.toNode);
				if(open.Contains(con.toNode) && cost < con.cost){
					open.Remove(con.toNode);
				}
				if(!open.Contains(con.toNode) && !closed.Contains(con.toNode)){
					con.setCost(cost);
					open.Add(new NodeRecord(con.toNode), cost, current);
					currentBest = open.getLowestCostNode(); // get lowest cost element
					// Debug.Log(currentBest.node.id);
				}
			}
		}


		while(currentBest != null){
			path.Add(currentBest.node);
			currentBest = currentBest.connection;
		}

		return path;
        // return base.findpath(graph, start, end, heuristic, ref found);
    }
}