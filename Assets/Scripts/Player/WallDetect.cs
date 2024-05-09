using UnityEngine;

namespace Heaven
{
    public class WallDetect : MonoBehaviour
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
            if (other.gameObject.CompareTag("Wall"))
            {
                if (name == "Left")
                {
                    player.leftWall = true;
                }
                else if (name == "Right")
                {
                    player.rightWall = true;
                }
            }
        }
        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                playerJump.touchWall = false;
                playerJump.slideWall = false;
                player.leftWall = false;
                player.rightWall = false;
            }
        }
    }
}