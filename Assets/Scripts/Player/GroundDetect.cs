using UnityEngine;

namespace Heaven
{
    public class GroundDetect : MonoBehaviour
    {
        PlayerMovement player;
        PlayerJump playerJump;
        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<PlayerMovement>();
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
        }
        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                player.isGrounded = false;
            }
        }
    }
}