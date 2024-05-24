using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class Transition : MonoBehaviour
    {
        Collider2D collider;    //Collider reference
        Animator animator;      //Animator reference
        // Start is called before the first frame update
        void Awake()
        {
            //Get Collider component and make it a trigger
            collider = GetComponent<Collider2D>();
            collider.isTrigger = true;

            //Find animator of object called CircleWipe
            animator = GameObject.Find("CircleWipe").GetComponent<Animator>();
        }

        //When object is triggered by Player
        //Set animator's 'Start' trigger
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                animator.SetTrigger("Start");
            }
        }

    }
}
