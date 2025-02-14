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

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("123");
    //        animator.SetBool("isEnter", true);
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        
            animator.SetBool("isEnter", true);
            animator2.SetBool("isEnter", true);
        
    }
}
