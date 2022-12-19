using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PathManager_Lab4 : PathManager{
    Vector3 desiredVelocity;

    float slowingDistance;

    Collider collider;
    public PathManager_Lab4(Agent p, Grid g = null, bool singleAgent = false) : base(p, g, singleAgent){
        slowingDistance = g.getCellSize();
    }

    void updateVelocity(GridCell goal){
        Vector3 targetOffset = goal.getPosition() - parent.transform.position;
        float distance = targetOffset.magnitude;
        float rampedSpeed = parent.maxSpeed * (distance/slowingDistance);
        float clippedSpeed = globals.min(rampedSpeed, parent.maxSpeed);
        desiredVelocity = targetOffset*clippedSpeed/distance;
    }



    
    public override void update(){
        if(path == null){
            newPath();
            return;
        }
        if(parent.currentCell != goalCell && path.Count != 0) {
            GridCell nextStep = path[path.Count -1];
            updateVelocity(nextStep);
            parent.transform.position += desiredVelocity * Time.deltaTime;

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




public class Simulation_Lab4 : MonoBehaviour
{
    List<Agent> Agentlist = new List<Agent>();
    List<Vector3> initialPositionList = new List<Vector3>();

    Grid grid;
    
    UnityEvent m_MyEvent;
    Globals global = new Globals();
    GridHeuristic heuristic;

    [SerializeField] bool multipleIndividuals = false;
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
        s.setPath(grid, singleAgent);
        s.setScale(5);
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
