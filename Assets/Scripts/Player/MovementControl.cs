using UnityEngine;

namespace Heaven
{
    public class MovementControl : MonoBehaviour
    {
        Player player;
        PlayerJump playerJump;

        RaycastHit2D hitLeft;
        RaycastHit2D hitRight;
        // Start is called before the first frame update
        void Start()
        {
            player = GetComponent<Player>();
            playerJump = GetComponent<PlayerJump>();
        }

        // Update is called once per frame
        void Update()
        {
            CheckWall();
        }
        void CheckWall()
        {
            if (playerJump.touchWall)
            {
                hitLeft = Physics2D.Raycast(transform.position, Vector2.left.normalized, .1f);
                hitRight = Physics2D.Raycast(transform.position, Vector2.right.normalized, .1f);

                if (hitRight && hitRight.transform.gameObject.tag == "Wall")
                {
                    if (Input.GetAxisRaw("Horizontal") == -1)
                    {
                        playerJump.touchWall = false;
                    }
                    else player.canMove = false;
                }
                else if (hitLeft && hitLeft.transform.gameObject.tag == "Wall")
                {
                    if (Input.GetAxisRaw("Horizontal") == 1)
                    {
                        playerJump.touchWall = false;
                    }
                    else player.canMove = false;
                }
                else return;
            }
        }
    }
}