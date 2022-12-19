using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Globals{
    public int xMin = 0;
    public int xMax = 10;
    public int zMin = 0;
    public int zMax = 10;
    public Vector3 randomPosition(){
        float x, y, z;
        x = Random.Range(xMin, xMax);
        y = 1.0f;
        z = Random.Range(zMin, zMax);

        return new Vector3(x, y, z);
    }

    public GridCell randomCell(Grid g){
        int x, z;
        x = Random.Range(xMin, xMax);
        // y = 1.0f;
        z = Random.Range(zMin, zMax);

        return g.getNode(x * g.getColumns() + z);
    }

    public float min(float a, float b){
        if(a < b) return a;
        return b;
    }
    public Globals(){}

}


public class PathManager{
    public Globals globals = new Globals();
    public GridCell goalCell;
    public Agent parent;
    
    public Grid grid;
    public Grid_JumpAhead gas = new Grid_JumpAhead(100, 100, 100);

    public List<GridCell> path;

    public bool renderPath = false;

    public GridHeuristic heuristic;
    public PathManager(Agent p, Grid g = null, bool singleAgent = false){
        grid = g;
        parent = p;
        renderPath = singleAgent;
    }
    
    public void newPath(){
        int i = 0;
        
        do goalCell = globals.randomCell(grid); while (goalCell.isOccupied());
        heuristic = new GridHeuristic(goalCell, parent.agentList);

        path = gas.findpath(grid, parent.currentCell, goalCell, heuristic, ref i);
        
        if(renderPath == true){
            path[path.Count -1].obj.GetComponent<Renderer>().material.color = new Color(1,0,0, 1);
            foreach(GridCell gr in path){
                if(gr.obj.GetComponent<Renderer>() != null)
                    gr.obj.GetComponent<Renderer>().material.color = new Color(0,1,0,1);
            };
            path[0].obj.GetComponent<Renderer>().material.color = new Color(0,0,1, 1);
        }
    }

    public virtual void update(){
        if(path == null){
            newPath();
            return;
        }
        if(parent.currentCell != goalCell && path.Count != 0) {
            GridCell nextStep = path[path.Count -1];
            parent.transform.position += (nextStep.getPosition() - parent.transform.position).normalized * Time.deltaTime * parent.maxSpeed;;
            if((parent.transform.position - nextStep.getPosition()).magnitude < grid.getCellSize()/4){
                if(renderPath == true)
                    parent.currentCell.obj.GetComponent<Renderer>().material.color = new Color(1,1,1,1);
                parent.currentCell = nextStep;
                path.Remove(path[path.Count -1]); 
            }
            Vector3 aux = nextStep.getPosition();
            aux.y = 1.5f;
            parent.transform.LookAt(aux, new Vector3(0,1,0));
        }
        else{
            newPath();
        }
    }
}




public class Simulator : MonoBehaviour
{
    // Start is called before the first frame update
    List<Agent> Agentlist = new List<Agent>();
    List<Vector3> initialPositionList = new List<Vector3>();

    Grid grid;
    
    UnityEvent m_MyEvent;
    Globals global = new Globals();
    GridHeuristic heuristic;

    [SerializeField] public bool multipleIndividuals = false;
    void Start()
    {
        NewScene();
        
        if (m_MyEvent == null)
            m_MyEvent = new UnityEvent();

        m_MyEvent.AddListener(NewScene);


    }

    // Update is called once per frame
    void Update()
    {
        foreach(Agent s in Agentlist) s.update();

        if (Input.GetKey(KeyCode.Space))
        {
            m_MyEvent.Invoke();
        }
    }
    void destroy(){
        if(grid != null){
            foreach(GridCell node in grid.nodes){
                if(node.obj != null)
                    Destroy(node.obj);
            }
            foreach(Agent s in Agentlist){
                Destroy(s.prefab.gameObject);
            }
            Agentlist.Clear();
        }
    }
    void NewScene(){
        destroy();
        grid = new Grid(0, 20, 0, 10, 10, 1);
        if(!multipleIndividuals) addAgent(true);
        else{
            for(int i = 0; i < 10; i++){
                addAgent(false);
            }
        }
    }

    

    void addAgent(bool singleAgent = false){
        Vector3 Agentpos = global.randomPosition();
        while(grid.getNode(((int)Agentpos.x) * grid.getColumns() + ((int)Agentpos.z)).isOccupied())
            Agentpos = global.randomPosition();
        Agent s;
        if(!singleAgent) s = new Agent(Agentpos, randomRotation(), grid, singleAgent, Agentlist); // only takes into account other agents if they exist!
        else s = new Agent(Agentpos, randomRotation(), grid, singleAgent);
        Agentlist.Add(s);
    }
    void removeAgent(string name){
        foreach(Agent s in Agentlist){
            if(s.name == name) Agentlist.Remove(s);
        }
    }
    bool checkIfPositionOccupied(Vector3 v){
        foreach(Agent s in Agentlist){
            if((s.transform.position - v).magnitude < 0.8) return true;
        }
        return false;
    }
    Quaternion randomRotation(){
        return Quaternion.Euler(0, Random.Range(-180,180), 0);
    }

}
