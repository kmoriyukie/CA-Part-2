using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using PathFinding;

public class GridCell : Node 
{
	protected float xMin;
	protected float xMax;
	protected float zMin;
	protected float zMax;

	protected int Rows = 10; // default elements per row
	protected int Cols = 10; // default elements per column

	protected float Width = 10;
	protected float Height = 10;
	
	
	protected bool occupied;
	protected Vector3 center;

	public GameObject obj;
	public GameObject obj2;
	

	public GridCell(int i):base(i) {
		xMin = i - Width/2;
		xMax = i + Width/2;
		zMin = i - Height/2;
		zMax = i + Height/2;
		occupied = false;
	}
	public GridCell(GridCell n):base(n) {
		// TO IMPLEMENT
		xMin = n.xMin;
		xMax = n.xMax;
		zMin = n.zMin;
		zMax = n.zMax;
		occupied = false;
	}

	public GridCell(int r, int c, int numRow = 10, int numCol = 10, float cellSize = 0f, float height = 10, bool oc = false, bool random = false):base(r * numCol + c ) {
		Rows = numRow;
		Cols = numCol;
		Width = cellSize;
		if(!random)
			occupied = oc;
		else{
			occupied = Random.Range(0.0f,1.0f) > 0.8;			
		}
		Height = height;

		int i = r * numCol + c;
		
		xMin = r - Width/2;
		xMax = r + Width/2;
		zMin = c - Height/2;
		zMax = c + Height/2;

		center = new Vector3(r * Width, height, c*Width);

		if(occupied){
			GameObject o = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SineVFX/TranslucentCrystals/Prefabs/Crystalsv06.prefab");
			obj = GameObject.Instantiate((GameObject)o, center, Quaternion.identity);
			
			obj.transform.localScale = new Vector3(3.5f,2f,3.5f);
			obj.transform.position = center;
			obj.transform.position += new Vector3(0, 0.5f, 0);  
			obj2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			obj2.transform.localScale = new Vector3(cellSize, height, cellSize);
			obj2.transform.position = center;  
			obj2.GetComponent<Renderer>().material.color = new Color(0,0,0, 1);
			
		}
		else{
			obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			obj.transform.localScale = new Vector3(cellSize, height, cellSize);
			obj.transform.position = center;  
		}
	}

	public bool isOccupied(){
		return occupied;
	}
	
	public Vector3 getPosition(){
		return center;
	}




};
