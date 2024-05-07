using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class Checkpoint : MonoBehaviour
    {
        Player player;
        Animator animator;
        Collider2D collider;

        public Transform checkpointPos;

        // Start is called before the first frame update
        void Awake()
        {
            player = FindObjectOfType<Player>();
            animator = GetComponent<Animator>();
            collider = GetComponent<Collider2D>();
            checkpointPos = GetComponentInChildren<Transform>();
            collider.isTrigger = true;
        }

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