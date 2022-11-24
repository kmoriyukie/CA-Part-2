using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Kalkatos.DottedArrow;

public class Globals{
    public int xMin = -9;
    public int xMax = 9;
    public int zMin = -9;
    public int zMax = 9;
    public Vector3 randomPosition(){
        float x, y, z;
        x = Random.Range(xMin*10, xMax*10) / 10f;
        y = 0f;
        z = Random.Range(zMin*10, zMax*10) / 10f;

        return new Vector3(x, y, z);
    }

    public Globals(){}

}
public class PathManager{
    Globals globals = new Globals();
    public Vector3 goals;
    slime parent;
    
    // Arrow arrow;
    public PathManager(slime p = null){
        goals = globals.randomPosition();
        parent = p;
        // loadArrow();
    }

    public void update(){
        if(parent.transform.position != goals) {
            Debug.Log("Parent transform: " + parent.transform.position + "\ngoals " + goals);
            parent.transform.Translate(goals.normalized * parent.maxSpeed * Time.deltaTime, Space.World);
            
            parent.transform.forward = Vector3.Lerp(parent.transform.forward, goals.normalized, 25*Time.deltaTime );
        }
        else{
            goals = globals.randomPosition();
        }
    }

    // void loadArrow(){
    //     Arrow o = AssetDatabase.LoadAssetAtPath<Arrow>("Assets/Kalkatos/DottedArrow/Prefabs/Arrow.prefab");
    //     arrow = Arrow.Instantiate((Arrow) o, Vector3.zero, Quaternion.identity);

    // }
}


public class slime{
    // Object obj;
    public GameObject prefab;
    public float maxSpeed = 1f;
    Vector3 maxVelocity = new Vector3(1,0,1);
    string[] filenames = {"Slime_01", "Slime_01_King", "Slime_01_MeltalHelmet", "Slime_01_Viking", 
                          "Slime_02", //"Slime_02_King", "Slime_02_MeltalHelmet", "Slime_02_Viking",
                          "Slime_03", "Slime_03 King", "Slime_03 Leaf", "Slime_03 Sprout"};
    // public Vector3 position;
    public string name;
    public Transform transform;
    
    public PathManager pathm;
    public slime(){
        int r = Random.Range(0,8);
        // UnityEngine.Object[] lst = loadprefabs();'
        try
        {
            List<Object> lst = loadprefabs();
            // position = Vector3.zero;
            name = filenames[r];
            pathm = new PathManager(this);
            prefab = GameObject.Instantiate((GameObject)lst[r], 
                                            Vector3.zero,
                                            Quaternion.identity);   
                                            
            transform = prefab.GetComponent<Transform>();
        }
        catch (System.Exception)
        {
            
            throw ;
        }
        // prefab.SetActive(true);
    } 
    public slime(Vector3 v, Quaternion q){
        int r = Random.Range(0,8);
        try
        {
            List<Object> lst = loadprefabs();
            // position = v;
            name = filenames[r];
            pathm = new PathManager(this);
            prefab = GameObject.Instantiate((GameObject)lst[r], v, q);
            
            transform = prefab.GetComponent<Transform>();
        }
        catch (System.Exception)
        {
            // Debug.Log(transform);
            throw;
        }
        // List<Object> lst = loadprefabs();
        
        // prefab.SetActive(true);
    } 

    public List<Object> loadprefabs(){
        List<Object> lst = new List<Object>();
        Object o;
        foreach(string s in filenames){
            o = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Kawaii Slimes/Prefabs/" + s + ".prefab");
            lst.Add(o);
        }
        // Debug.Log(lst);
        return lst;
    }  

    public void update(){
        pathm.update();
        Debug.Log(pathm.goals);
    }

}

public class Simulator : MonoBehaviour
{
    // Start is called before the first frame update
    List<slime> slimelist = new List<slime>();
    List<Vector3> initialPositionList = new List<Vector3>();

    Globals global = new Globals();
    void Start()
    {
        // for(int i = -5; i < 5; i++){
        //     addAgent();
        // }
        addAgent();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(slime s in slimelist) s.update();
    }

    // void OnGUI(){
    //     GUI.skin.textArea.fontSize = 40;
    //     GUI.skin.label.fontSize = 200;
    //     try
    //     {
    //         GUI.Label( new Rect(
    //             5,                   // x, left offset
    //             Screen.height - 200, // y, bottom offset
    //             700f,                // width
    //             200f                 // height
    //         ),
    //         "bla : ", GUI.skin.textArea);   
    //     }
    //     catch (System.Exception)
    //     {
    //         Debug.Log("msg");
    //         throw;
    //     }
    // }

    void addAgent(){
        Vector3 slimepos = global.randomPosition();
        while(checkIfPositionOccupied(slimepos))
            slimepos = global.randomPosition();
            
        slime s = new slime(slimepos, randomRotation());
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
