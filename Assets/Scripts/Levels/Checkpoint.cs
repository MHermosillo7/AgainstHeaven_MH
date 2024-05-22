using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class Checkpoint : MonoBehaviour
    {
        PlayerMovement player;  //PlayerMovement script reference
        Animator animator;      //Animator reference
        Collider2D collider;    //Collider2D reference

        public Transform checkpointPos; //Checkpoint transform reference

        // Start is called before the first frame update
        void Awake()
        {
            //Find PlayerMovement script
            player = FindObjectOfType<PlayerMovement>();

            //Get Animator component
            animator = GetComponent<Animator>(); 
            
            //Get Collider2D                   
            collider = GetComponent<Collider2D>();

            //Get the transform component in children
            checkpointPos = GetComponentInChildren<Transform>();

            //Make this collider a trigger
            collider.isTrigger = true;
        }

        //On trigger by the object tagged Player, 
        //change the PlayerMovement script's variable "lastCheckpoint"
        //into this script's Transform reference
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                player.lastCheckpoint = checkpointPos.position;
                animator.SetTrigger("Open");
            }
        }
    }
}