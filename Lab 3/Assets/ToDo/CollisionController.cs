using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public Agent parent;
    ContactPoint prev;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setParent(Agent p){
        parent = p;
    }
    void OnCollisionEnter(Collision collision)
    {
        int mult = 1;
        if(parent!=null){
            if(collision.GetContact(0).Equals(prev))
                mult = 2;
            parent.prefab.transform.position += mult *(parent.prefab.transform.position - collision.GetContact(0).point).normalized* parent.maxSpeed * Time.deltaTime;
            prev = collision.GetContact(0);
            
        }
    }
}
