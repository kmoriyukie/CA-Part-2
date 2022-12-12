using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PathFinding;

public class GridHeuristic : Heuristic<GridCell>
{
	// Class that represents a Heuristic function to estimate the cost of going from 
	// one GridCell to another

	// constructor takes a goal node for estimating
	public GridHeuristic(GridCell goal):base(goal){
		// goalNode = goal;
	}
	
	public GridHeuristic(GridCell start, GridCell goal):base(start, goal){
	}
	
	public void setStart(GridCell start){
		startNode = start;
	}
	 // generates an estimated cost to reach the stored goal from the given node
	public override float estimateCost(GridCell fromNode){
		return (goalNode.getPosition() - fromNode.getPosition()).magnitude;
		// return 0;// TO IMPLEMENT
	}

	public override float costFromStart(GridCell toNode){
		return (startNode.getPosition() - toNode.getPosition()).magnitude;
	}

	// determines if the goal node has been reached by node
	public override bool goalReached(GridCell node){
		return false;// TO IMPLEMENT
	}

};
