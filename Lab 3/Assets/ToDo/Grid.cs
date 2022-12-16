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
 
	protected float xMin;
	protected float xMax;
	protected float zMin;
	protected float zMax;
	
	protected float gridHeight;
	
	protected float sizeOfCell;
	
	protected int numCells;
	protected int numRows = 10;
	protected int numColumns = 10;
	
	
	// Example Constructor function declaration
	
	// You have basically to fill the base fields "nodes" and "connections", 
	// i.e. create your list of GridCells (with random obstacles) 
	// and then create the corresponding GridConnections for each one of them
	// based on where the obstacles are and the valid movements allowed between GridCells. 
	

	// TO IMPLEMENT

	public Grid(float minX, float maxX, float minZ, float maxZ, float cellSize, float height = 0, int numR = 10, int numC = 10):base(){
		xMax = maxX;
		xMin = minX;
		zMax = maxZ;
		zMin = minZ;
		sizeOfCell = cellSize;
		gridHeight = height;

		numRows = numR;
		numColumns = numC;
		numCells = numRows * numColumns;

		for(int i = 0; i < numR; i++){
			for (int j = 0; j < numC; j++)
			{
				nodes.Add(new GridCell(i, j, numRows, numColumns, cellSize, height, i < 8 && j == 2, true));
				connections.Add(new GridConnections());

				for(int k = 0; k < 8; k++){ // guarantee always 8 neighbors, even if empty / obstacle
					connections[i * numColumns + j].Add(null);
				}
			}
		}
		int idx;
/*
Neighbor order!
1 8 7
2   6
3 4 5
*/
		for(int i = 0; i < numR; i++){
			for (int j = 0; j < numC; j++)
			{
				idx = i * numColumns + j;
				if(i > 0){
					if(j < numC - 1) CheckAddConnection(idx, (i - 1)*numColumns + (j + 1), 0);
					if(j > 0) CheckAddConnection(idx, (i - 1)*numColumns + (j - 1), 1);

					CheckAddConnection(idx, (i - 1)*numColumns + j, 2);
				}

				if(j > 0) CheckAddConnection(idx, i*numColumns + j - 1, 3);
				// else connections[idx].Add(null);
				
				if(i < numR - 1){
					if(j > 0) CheckAddConnection(idx, (i + 1)*numColumns + (j - 1), 4);
					// else connections[idx].Add(null);

					CheckAddConnection(idx, (i + 1)*numColumns + j, 5);

					if(j < numC - 1) CheckAddConnection(idx, (i + 1)*numColumns + j + 1, 6);
					// else connections[idx].Add(null);
				}
				// else connections[idx].Add(null);

				if(j < numC - 1) CheckAddConnection(idx, i*numColumns + j + 1, 7);
				// else connections[idx].Add(null);
			}
		}

	}
	
	void CheckAddConnection(int idx, int idx2, int direction){
		if(!nodes[idx2].isOccupied()){
			connections[idx].SetElementAt(direction, new CellConnection(nodes[idx], nodes[idx2]));
		}
		// else connections[idx].Add(null);
	}

	// public GridCell getCellFromPosition(Vector3 pos){
	// 	double dist = 10000;
	// 	GridCell aux;
	// 	foreach (var item in nodes)
	// 	{
	// 		if((item.getPosition() - pos).magnitude < dist) {
	// 			dist = (item.getPosition() - pos).magnitude;
	// 			aux = 
	// 		}
	// 	}
	// }

	public int getColumns(){
		return numColumns;
	}
	public int getRows(){
		return numRows;
	}
	public float getCellSize(){
		return sizeOfCell;
	}
	
};
