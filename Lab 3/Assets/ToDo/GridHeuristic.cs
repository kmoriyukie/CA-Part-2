using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PathFinding;

public class GridHeuristic : Heuristic<GridCell>
{
	// Class that represents a Heuristic function to estimate the cost of going from 
	// one GridCell to another

	// constructor takes a goal node for estimating

	List<slime> agentList;
	public GridHeuristic(GridCell goal):base(goal){
		// goalNode = goal;
	}
	
	public GridHeuristic(GridCell start, GridCell goal):base(start, goal){
	}
	

	public GridHeuristic(GridCell goal, List<slime> lst):base(goal){
		agentList = lst;
	}
	public void setStart(GridCell start){
		startNode = start;
	}
	 // generates an estimated cost to reach the stored goal from the given node
	public override float estimateCost(GridCell fromNode){
		if(agentList != null){
			int aux = 1;
			foreach (var item in agentList)
			{
				if(item.pathm.path.Contains(fromNode)) 
					aux*=20;
			}
			return aux * (goalNode.getPosition() - fromNode.getPosition()).magnitude;
			// increased cost if other agents in cell
		}
		else return(goalNode.getPosition() - fromNode.getPosition()).magnitude;
	}

	public override float costFromStart(GridCell toNode){
		return (startNode.getPosition() - toNode.getPosition()).magnitude;
	}

	// determines if the goal node has been reached by node
	public override bool goalReached(GridCell node){
		return false;// TO IMPLEMENT
	}

};
