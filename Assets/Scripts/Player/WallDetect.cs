using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class WallDetect : MonoBehaviour
    {
        Player player;

        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<Player>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                player.touchWall = true;
                player.rb.gravityScale = .8f;
            }
        }
        private void OnCollisionExit2D(Collision2D other)
        {
            player.touchWall = false;
            player.rb.gravityScale = 1f;
        }
    }
}