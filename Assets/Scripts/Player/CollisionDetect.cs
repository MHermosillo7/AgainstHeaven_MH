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
            Debug.Log(other.transform.name);
            if (other.gameObject.CompareTag("Wall"))
            {
                playerJump.touchWall = true;
                playerJump.slideWall = true;

                if (playerJump.jumpBufferTime > 0)
                {
                    playerJump.jumpBufferTime = 0;
                    playerJump.WallJump();
                }
            }
            if (other.gameObject.CompareTag("Ground"))
            {
                player.isGrounded = true;
                playerJump.jumped = false;
                playerJump.jumpsLeft = playerJump.storeJumpsLeft;

                if (playerJump.jumpBufferTime > 0 && playerJump.jumpsLeft > 0)
                {
                    playerJump.jumpBufferTime = 0;
                    playerJump.Jump(1f);
                }
            }
        }
        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                playerJump.touchWall = false;
                playerJump.slideWall = false;
            }
            if (other.gameObject.CompareTag("Ground"))
            {
                if(playerJump.jumped == false)
                {
                    player.isGrounded = true;
                }
                else player.isGrounded = false;

                playerJump.substractBufferTime = true;
            }
        }
    }
}