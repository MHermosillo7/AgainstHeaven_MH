using UnityEngine;

namespace Heaven
{
    public class GroundDetect : MonoBehaviour
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
                playerJump.jumpsLeft += 1;

                if(playerJump.jumpBufferTime > 0 && playerJump.jumpsLeft > 0)
                {
                    playerJump.Jump(1f);
                }
            }
        }
        private void OnCollisionExit2D(Collision2D other)
        {
            player.isGrounded = false;
            playerJump.substractBufferTime = true;
        }
    }
}