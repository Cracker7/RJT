using UnityEngine;

public class treeDown : MonoBehaviour
{
   // public Animation animation;
    public Animator animator;
    public Animator animator2;
   

    private void Awake()
    {
       
    }


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool("isEnter", true);
        animator2.SetBool("isEnter", true);
    }



    //private void OnCollisionEnter(Collision collision)
    //{

    //        animator.SetBool("isEnter", true);
    //        animator2.SetBool("isEnter", true);

    //}
}
