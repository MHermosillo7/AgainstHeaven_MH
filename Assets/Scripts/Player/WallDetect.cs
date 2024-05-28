using UnityEngine;

namespace Heaven
{
    public class WallDetect : MonoBehaviour
    {
        RotatePlayer rotatePlayer;    //RotatePlayer script reference
        PlayerJump playerJump;        //PlayerJump script reference
        // Start is called before the first frame update
        void Awake()
        {
            //Find PlayerJump and RotatePlayer scripts
            playerJump = FindObjectOfType<PlayerJump>();
            rotatePlayer = FindObjectOfType<RotatePlayer>();
        }

        //On Collision with object tagged Wall
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                //If this object is called Left
                if (name == "Left")
                {    
                    //Player is touching wall to left
                    rotatePlayer.leftWall = true;
                }
                //Else if object is called Right
                else if (name == "Right")
                {
                    //Player is touching a wall to right
                    rotatePlayer.rightWall = true;
                }
            }
        }
        //On exit of Collision with object tagged Wall
        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                //Player is not touching a wall
                playerJump.touchWall = false;
                //Player is not sliding down a wall
                playerJump.slideWall = false;
                //Player is not touching wall to left
                rotatePlayer.leftWall = false;
                //Player is not touching wall to right
                rotatePlayer.rightWall = false;

                //Call ResetJumpBUffer method
                //This function could be deleted, 
                //it is redundant
                playerJump.ResetJumpBuffer();

                //Player has exited a wall
                //[Redundant too]
                playerJump.exitWall = true;
            }
        }
    }
}
