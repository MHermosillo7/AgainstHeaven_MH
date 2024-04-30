using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class WallDetect : MonoBehaviour
    {
        PlayerJump playerJump;

        // Start is called before the first frame update
        void Start()
        {
            playerJump = FindObjectOfType<PlayerJump>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                playerJump.touchWall = true;

                if (playerJump.jumpBufferTime > 0)
                {
                    playerJump.WallJump();
                }
            }
        }
        private void OnCollisionExit2D(Collision2D other)
        {
            playerJump.touchWall = false;
        }
    }
}