using UnityEngine;

namespace Heaven
{
    public class GroundDetect : MonoBehaviour
    {
        Player player;

        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<Player>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                player.isGrounded = true;
                player.jumpsLeft += 1;
                if(player.jumpBufferTime > 0)
                {
                    player.Jump();
                }
            }
        }
        private void OnCollisionExit2D(Collision2D other)
        {
            player.isGrounded = false;
            player.substractBufferTime = true;
        }
    }
}