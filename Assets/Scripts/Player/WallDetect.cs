using UnityEngine;

namespace Heaven
{
    public class WallDetect : MonoBehaviour
    {
        RotatePlayer rotatePlayer;
        PlayerJump playerJump;
        // Start is called before the first frame update
        void Awake()
        {
            playerJump = FindObjectOfType<PlayerJump>();
            rotatePlayer = FindObjectOfType<RotatePlayer>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                if (name == "Left")
                {
                    rotatePlayer.leftWall = true;
                }
                else if (name == "Right")
                {
                    rotatePlayer.rightWall = true;
                }
            }
        }
        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                playerJump.touchWall = false;
                playerJump.slideWall = false;
                rotatePlayer.leftWall = false;
                rotatePlayer.rightWall = false;

                playerJump.ResetJumpBuffer();
                playerJump.exitWall = true;
            }
        }
    }
}