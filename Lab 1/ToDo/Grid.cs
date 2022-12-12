using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PathFinding;

public class Grid : FiniteGraph<GridCell, CellConnection, GridConnections>
{
	// Class that represent the finite graph corresponding to a grid of cells
	// There is a known set of nodes (GridCells), 
	// and a known set of connections (CellConnections) between those nodes (GridConnections)
	
	// Example Data 
	/* 
	protected float xMin;
	protected float xMax;
	protected float zMin;
	protected float zMax;
	
	protected float gridHeight;
	
	protected float sizeOfCell;
	
	protected int numCells;
	protected int numRows;
	protected int numColumns;
	*/
	
	// Example Constructor function declaration
	// public Grid(float minX, float maxX, float minZ, float maxZ, float cellSize, float height = 0):base()
	
	// You have basically to fill the base fields "nodes" and "connections", 
	// i.e. create your list of GridCells (with random obstacles) 
	// and then create the corresponding GridConnections for each one of them
	// based on where the obstacles are and the valid movements allowed between GridCells. 
	
	
	// TO IMPLEMENT
		
	
	
};
