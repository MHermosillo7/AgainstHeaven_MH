using UnityEngine;

namespace HeavenAndHell
{
    [RequireComponent(typeof(Collider2D))]
    public class Checkpoint : MonoBehaviour
    {
        Player player;

        public Transform checkpointPos;

        // Start is called before the first frame update
        void Awake()
        {
            player = FindObjectOfType<Player>();
            checkpointPos = GetComponentInChildren<Transform>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                player.lastCheckpoint = checkpointPos.position;
            }
        }
    }
}