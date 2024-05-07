using UnityEngine;

namespace Heaven
{
    public class CollisionDetect : MonoBehaviour
    {
        Player player;
        PlayerJump playerJump;
        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<Player>();
            playerJump = FindObjectOfType<PlayerJump>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                player.isGrounded = true;
                playerJump.jumpsLeft = playerJump.storeJumpsLeft;

                if (playerJump.jumpBufferTime > 0 && playerJump.jumpsLeft > 0)
                {
                    playerJump.jumpBufferTime = 0;
                    playerJump.Jump(1f);
                }
            }
            if (other.gameObject.CompareTag("Wall"))
            {
                playerJump.touchWall = true;
                playerJump.slideWall = true;

                if (playerJump.jumpBufferTime > 0)
                {
                    playerJump.jumpBufferTime = 0;
                    playerJump.Jump(2f);
                }
            }
        }
        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                player.isGrounded = false;
                playerJump.substractBufferTime = true;
            }
            if (other.gameObject.CompareTag("Wall"))
            {
                playerJump.touchWall = false;
                playerJump.slideWall = false;
            }
        }
    }
}