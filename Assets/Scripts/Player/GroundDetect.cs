using UnityEngine;

namespace Heaven
{
    public class GroundDetect : MonoBehaviour
    {
        PlayerMovement player;    //PlayerMovement script reference
        PlayerJump playerJump;    //PlayerJump script reference
        // Start is called before the first frame update
        void Start()
        {
            //Find PlayerMovement and PlayerJump scripts
            player = FindObjectOfType<PlayerMovement>();
            playerJump = FindObjectOfType<PlayerJump>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            //When colliding with Ground Game Object
            if (other.gameObject.CompareTag("Ground"))
            {
                //Player is grounded
                player.isGrounded = true;
                //Reset amount of jumps left
                playerJump.jumpsLeft = playerJump.storeJumpsLeft;

                //If jump buffer time is greater than 0 and
                //Player has more than 0 jumps left
                if (playerJump.jumpBufferTime > 0 && playerJump.jumpsLeft > 0)
                {
                    //Jump buffer time equals 0
                    playerJump.jumpBufferTime = 0;
                    //Call PlayerJump's Jump method and pass
                    //Integer 1 as its input
                    playerJump.Jump(1f);
                }
            }
        }
        private void OnCollisionExit2D(Collision2D other)
        {
            //When exiting collision with Ground Game Object
            if (other.gameObject.CompareTag("Ground"))
            {
                //Player is no longer grounded
                player.isGrounded = false;
            }
        }
    }
}
