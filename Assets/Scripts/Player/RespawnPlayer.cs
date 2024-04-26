using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class RespawnPlayer : MonoBehaviour
    {
        //ATTACH TO CAMERA
        Player player;
        Collider2D collider;
        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<Player>();
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