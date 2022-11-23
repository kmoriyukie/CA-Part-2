using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class locomotion : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1.0f;
    public float rotationSpeed = 1000.0f;
    Animator animator;

    Vector3 motion;
    Quaternion rotation;
    Vector3 dest;
    void Start()
    {
         animator = GetComponent<Animator>();
         animator.applyRootMotion = false;

         dest = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        motion = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        motion.Normalize();
        
        transform.Translate(motion * speed * Time.deltaTime, Space.World);
        
        if(motion != Vector3.zero){
            // if (Input.GetKey(KeyCode.A))
            setState("Walk");
            
            transform.forward = Vector3.Lerp(transform.forward, motion, 25*Time.deltaTime );
        }
    }
    // void OnGUI(){
    //     GUI.skin.textArea.fontSize = 40;
    //     GUI.skin.label.fontSize = 200;
    //     GUI.Label( new Rect(
    //            5,                   // x, left offset
    //            Screen.height - 200, // y, bottom offset
    //            700f,                // width
    //            200f                 // height
    //        ),
    //        "Right Vector : " + transform.rotation + "\n test : " + new Vector3(translationAD, 0, translationWS).normalized, GUI.skin.textArea);
        
    // }
    void setState(string s){
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName(s)){
            animator.Play(s);
        }
    }
}
