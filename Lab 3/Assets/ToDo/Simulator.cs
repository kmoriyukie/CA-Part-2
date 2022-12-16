using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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

    public Globals(){}

}


public class PathManager{
    Globals globals = new Globals();
    // public Vector3 goal;
    public GridCell goalCell;
    slime parent;
    
    // Vector3 goal;
    Grid grid;
    Grid_JumpAhead gas = new Grid_JumpAhead(100, 100, 100);

    public List<GridCell> path;
    // Arrow arrow;

    bool renderPath = false;

    GridHeuristic heuristic;
    public PathManager(slime p = null, Grid g = null, bool highlight = false){
        grid = g;
        parent = p;
        renderPath = highlight;
        newPath();
    }
    
    void newPath(){
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

    public void update(){
        if(parent.currentCell != goalCell && path.Count != 0) {
            GridCell nextStep = path[path.Count -1];
            parent.transform.position += (nextStep.getPosition() - parent.transform.position).normalized * Time.deltaTime * parent.maxSpeed;;
            if((parent.transform.position - nextStep.getPosition()).magnitude < grid.getCellSize()/4){
                if(renderPath == true)
                    parent.currentCell.obj.GetComponent<Renderer>().material.color = new Color(1,1,1,1);
                parent.currentCell = nextStep;
                path.Remove(path[path.Count -1]); 
            }
            
            // parent.transform.forward =  (nextStep.getPosition() - parent.transform.position).normalized;
            parent.transform.forward = Vector3.Lerp(nextStep.getPosition().normalized, parent.transform.position, 2*Time.deltaTime).normalized;
                
        }
        else{
            newPath();
            // parent.transform.forward = (goalCell.getPosition() - parent.transform.position).normalized;
            // parent.transform.forward = Vector3.Lerp(parent.transform.forward,goalCell.getPosition().normalized, Time.deltaTime*50);
        }
    }
}


public class slime{
    // Object obj;
    public GameObject prefab;
    public float maxSpeed = Random.Range(5, 15)/5f;
    Vector3 maxVelocity = new Vector3(1,0,1);
    string[] filenames = {"Slime_01", "Slime_01_King", "Slime_01_MeltalHelmet", "Slime_01_Viking", 
                          "Slime_02", 
                          "Slime_03", "Slime_03 King", "Slime_03 Leaf", "Slime_03 Sprout"};

    public string name;
    public Transform transform;
    
    public PathManager pathm;

    public GridCell currentCell;

    public GridHeuristic heuristic;

    public List<slime> agentList;
    public slime(Grid grid, List<slime> lsta = null){
        int r = Random.Range(0,8);

        agentList = lsta;

        List<Object> lst = loadprefabs();
        name = filenames[r];
        pathm = new PathManager(this, grid);
        currentCell = grid.getNode(0);

        prefab = GameObject.Instantiate((GameObject)lst[r], 
                                        currentCell.getPosition(),
                                        Quaternion.identity);   
                                        
        transform = prefab.GetComponent<Transform>();
        prefab.AddComponent<Rigidbody>();

    } 
    public slime(Vector3 v, Quaternion q, Grid grid, bool highlight = false, List<slime> lsta = null){
        int r = Random.Range(0,8);

        agentList = lsta;
        
        List<Object> lst = loadprefabs();
        name = filenames[r];

        currentCell = grid.getNode(((int)v.x) * grid.getColumns() + ((int)v.z));
        Debug.Log(currentCell.getPosition());
        Vector3 pos = currentCell.getPosition();
        pos.y = 1.0f;
        pathm = new PathManager(this, grid, highlight);

        prefab = GameObject.Instantiate((GameObject)lst[r], pos, q);
        
        transform = prefab.GetComponent<Transform>();
        transform.localScale = new Vector3(10, 10, 10);
        
        prefab.AddComponent<Rigidbody>();

        prefab.AddComponent<SphereCollider>();
        prefab.GetComponent<SphereCollider>().radius = 0.35f;
        prefab.GetComponent<SphereCollider>().center = new Vector3(0, 0.35f, 0);
        
        prefab.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Extrapolate;
        prefab.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
    } 

    public List<Object> loadprefabs(){
        List<Object> lst = new List<Object>();
        Object o;
        foreach(string s in filenames){
            o = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Kawaii Slimes/Prefabs/" + s + ".prefab");
            lst.Add(o);
        }
        return lst;
    }  

    public void update(){
        pathm.update();
    }

}

public class Simulator : MonoBehaviour
{
    // Start is called before the first frame update
    List<slime> slimelist = new List<slime>();
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
        foreach(slime s in slimelist) s.update();

        if (Input.GetKey(KeyCode.Space))
        {
            m_MyEvent.Invoke();
            // if(get)'
        }
    }
    void destroy(){
        if(grid != null){
            foreach(GridCell node in grid.nodes){
                if(node.obj != null)
                    Destroy(node.obj);
            }
            foreach(slime s in slimelist){
                Destroy(s.prefab.gameObject);
            }
            slimelist.Clear();
        }
    }
    void NewScene(){
        destroy();
        grid = new Grid(0, 10, 0, 10, 10, 1);
        if(!multipleIndividuals) addAgent(true);
        else{
            for(int i = 0; i < 10; i++){
                addAgent(false);
            }
        }
    }
    void addAgent(bool highlight = false){
        Vector3 slimepos = global.randomPosition();
        while(grid.getNode(((int)slimepos.x) * grid.getColumns() + ((int)slimepos.z)).isOccupied())
            slimepos = global.randomPosition();
        slime s;
        if(!highlight) s = new slime(slimepos, randomRotation(), grid, highlight, slimelist); // only takes into account other agents if they exist!
        else s = new slime(slimepos, randomRotation(), grid, highlight);
        slimelist.Add(s);
    }
    void removeAgent(string name){
        foreach(slime s in slimelist){
            if(s.name == name) slimelist.Remove(s);
        }
    }
    bool checkIfPositionOccupied(Vector3 v){
        foreach(slime s in slimelist){
            if((s.transform.position - v).magnitude < 0.8) return true;
        }
        return false;
    }
    Quaternion randomRotation(){
        return Quaternion.Euler(0, Random.Range(-180,180), 0);
    }

}
