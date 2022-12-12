using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Grid;
using UnityEngine.Events;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    Grid grid;
    Grid_A_Star gas = new Grid_A_Star(100, 100, 100);

    UnityEvent m_MyEvent;
    void Start()
    {

        if (m_MyEvent == null)
            m_MyEvent = new UnityEvent();

        m_MyEvent.AddListener(NewScene);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            m_MyEvent.Invoke();
        }
    }

    void destroy(){
        if(grid != null)
        foreach(GridCell node in grid.nodes){
            if(node.obj != null)
                Destroy(node.obj);
        }
    }
    void NewScene(){
        destroy();
        grid = new Grid(0, 10, 0, 10, 10, 1);
        int i = -1;


        int idx = (int)Random.Range(0, 99);
        while(grid.getNode(idx).isOccupied())
            idx = (int)Random.Range(0, 99);
        GridCell goal = grid.getNode(idx);

        idx = (int)Random.Range(0, 99);
        while(grid.getNode(idx).isOccupied())
            idx = (int)Random.Range(0, 99);
        GridCell start = grid.getNode(idx);


        // for(int k = 0; k < grid.getConnections(11).connections.Count; k++){
        //     Debug.Log(grid.getConnections(11).connections[k].toNode.getId());
        // }
        foreach(GridCell g in gas.findpath(grid, start, goal, new GridHeuristic(start, goal), ref i)){
            if(g.obj.GetComponent<Renderer>() != null)
                g.obj.GetComponent<Renderer>().material.color = new Color(0,1,0,1);
        };
        
        if(start.obj.GetComponent<Renderer>() != null)
            start.obj.GetComponent<Renderer>().material.color = new Color(0,0,1,1);
        if(goal.obj.GetComponent<Renderer>() != null)
            goal.obj.GetComponent<Renderer>().material.color = new Color(1,0,0,1);

    }
}
