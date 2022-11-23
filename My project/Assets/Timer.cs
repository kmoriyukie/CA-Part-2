 using UnityEngine;
 using System.Collections;
 
 public class Timer: MonoBehaviour {
 
    public float targetTime = 5.0f;
    Animator animator;

    public float targetTime2 = 1.0f;

    void Start(){
        animator = GetComponent<Animator>();
    }

    void Update(){
        targetTime -= Time.deltaTime;
        if (targetTime <= 0.0f)
        {
            timerEnded();
            targetTime2 -= Time.deltaTime;
            // Destroy(animator);
        }
        if(targetTime2 <= 0.0f){
            targetTime = 5.0f;
            animator.SetBool("AnimationTimer", false);
            targetTime2 = 1.0f;
        }
    }
    
    void timerEnded()
    {
        animator.SetBool("AnimationTimer", true);
    }
 
 
 }