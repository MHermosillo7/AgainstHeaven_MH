using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class Checkpoint : MonoBehaviour
    {
        Player player;
        Animator animator;

        public Transform checkpointPos;

        // Start is called before the first frame update
        void Awake()
        {
            player = FindObjectOfType<Player>();
            animator = GetComponent<Animator>();
            checkpointPos = GetComponentInChildren<Transform>();
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