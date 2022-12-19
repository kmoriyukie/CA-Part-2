using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Agent{
    public GameObject prefab;
    public GameObject colliderObject;
    public float maxSpeed = 8;
    Vector3 maxVelocity = new Vector3(1,0,1);
    string[] filenames = {"Slime_01", "Slime_01_King", "Slime_01_MeltalHelmet", "Slime_01_Viking", 
                          "Slime_02", 
                          "Slime_03", "Slime_03 King", "Slime_03 Leaf", "Slime_03 Sprout"};

    public string name;
    public Transform transform;
    
    public PathManager pathm;
    public PathManager_Lab4 path;
    public GridCell currentCell;

    public GridHeuristic heuristic;

    public List<Agent> agentList;
    public void setPath(Grid grid, bool highlight){
        path = new PathManager_Lab4(this, grid, highlight);
    }

    public Agent(Vector3 v, Quaternion q, Grid grid, bool highlight = false, List<Agent> lsta = null){
        int r = Random.Range(0,8);
        colliderObject = new GameObject();
        agentList = lsta;
        
        List<Object> lst = loadprefabs();
        name = filenames[r];

        if(!highlight) maxSpeed = 5; //slower if cluster
        currentCell = grid.getNode(((int)v.x) * grid.getColumns() + ((int)v.z));

        Vector3 pos = currentCell.getPosition();
        pos.y = 1f;
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

        colliderObject.AddComponent<Rigidbody>();
        colliderObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
        colliderObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        colliderObject.GetComponent<Rigidbody>().isKinematic = true;


        colliderObject.AddComponent<CollisionController>();
        
        colliderObject.GetComponent<CollisionController>().parent = this;
        colliderObject.transform.localScale = prefab.transform.localScale;
        colliderObject.transform.forward = prefab.transform.forward;
        colliderObject.transform.position = prefab.transform.position;        
        
        colliderObject.AddComponent<CapsuleCollider>();
        colliderObject.GetComponent<CapsuleCollider>().direction = 2;
        colliderObject.GetComponent<CapsuleCollider>().height = 2;
        colliderObject.GetComponent<CapsuleCollider>().radius = 0.35f;
        
        colliderObject.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.5f, 0.5f);   
    } 


    public void setScale(int x){
        transform.localScale = new Vector3(x,x,x);
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
        if(path != null) path.update();
        else pathm.update();

        
        colliderObject.transform.localScale = prefab.transform.localScale;
        colliderObject.transform.forward = prefab.transform.forward;
        colliderObject.transform.position = prefab.transform.position;        
        colliderObject.transform.rotation = prefab.transform.rotation;
    }

}