using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;
    public Vector3 velocity = new Vector3(1,1,1);
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getSpeed(){
        return animator.speed;
    }
    public Vector3 getVelocity(){
        return animator.speed * transform.forward;
    }

    public Vector3 getOrientation(){
        return transform.forward;
    }

}
