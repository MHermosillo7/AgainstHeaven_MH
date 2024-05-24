using UnityEngine;

namespace Heaven
{
    public class CollisionDetect : MonoBehaviour
    {
        PlayerMovement player;    //PlayerMovement script reference
        PlayerJump playerJump;    //PlayerJump script reference
        // Start is called before the first frame update
        void Awake()
        {
            //Find PlayerMovement and PlayerJump scripts
            player = FindObjectOfType<PlayerMovement>();
            playerJump = FindObjectOfType<PlayerJump>();
        }

        
        private void OnCollisionEnter2D(Collision2D other)
        {
            //On Collision with Wall game object
            if (other.gameObject.CompareTag("Wall"))
            {
                //Make true Player Jump's
                //touch wall and slide wall variables
                playerJump.touchWall = true;
                playerJump.slideWall = true;

                //If Player Jump's jump buffer time is greater than 0
                if (playerJump.jumpBufferTime > 0)
                {
                    //Jump buffer equals 0
                    playerJump.jumpBufferTime = 0;
                    //Player is not sliding against wall
                    playerJump.slideWall = false;
                    //Call PlayerJump's WallJump script
                    playerJump.WallJump();
                }
            }
            //On Collision with Ground Game Objects
            if (other.gameObject.CompareTag("Ground"))
            {
                //Player is grounded
                player.isGrounded = true;
                //Player has not jumped
                playerJump.jumped = false;
                //Reset amount of jumps left
                playerJump.jumpsLeft = playerJump.storeJumpsLeft;

                //If jump buffer time is greater than 0
                if (playerJump.jumpBufferTime > 0)
                {
                    //Jump buffer time equals 0
                    playerJump.jumpBufferTime = 0;
                    //Call PlayerJump's Jump method with
                    //integer 1 as input
                    playerJump.Jump(1f);
                }
            }
        }
        private void OnCollisionStay2D(Collision2D other)
        {
            //While staying in collision with wall
            if (other.gameObject.CompareTag("Wall"))
            {
                //Player is touching a wall
                playerJump.touchWall = true;
            }
            //While staying in collision with ground
            if (other.gameObject.CompareTag("Ground"))
            {
                //Player is grounded
                player.isGrounded = true;
            }

        }
        private void OnCollisionExit2D(Collision2D other)
        {
            //When exiting collision with Wall Game Object
            if (other.gameObject.CompareTag("Wall"))
            {
                //Player is not touching a wall
                playerJump.touchWall = false;
                //Player is not sliding down a wall
                playerJump.slideWall = false;
            }
            //When exiting collision with Ground Game Object
            if (other.gameObject.CompareTag("Ground"))
            {
                //Player is not grounded
                player.isGrounded = false;
            }
        }
    }
}
