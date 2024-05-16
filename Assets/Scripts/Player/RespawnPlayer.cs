using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class RespawnPlayer : MonoBehaviour
    {
        //ATTACH TO CAMERA
        PlayerMovement player;
        Collider2D collider;
        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<PlayerMovement>();
            collider = GetComponent<Collider2D>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                player.Respawn();
            }
        }
    }
}