using UnityEngine;

namespace Heaven
{
    public class MovementControl : MonoBehaviour
    {
        Player player;
        PlayerJump playerJump;

        RaycastHit2D hitLeft;
        RaycastHit2D hitRight;
        RaycastHit2D diagonalRight;
        RaycastHit2D diagonalLeft;
        // Start is called before the first frame update
        void Awake()
        {
            player = GetComponent<Player>();
            playerJump = GetComponent<PlayerJump>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            CheckWall();
        }
        void CheckWall()
        {
            if (playerJump.touchWall)
            {
                hitLeft = Physics2D.Raycast(transform.position, Vector2.left.normalized, .5f);
                hitRight = Physics2D.Raycast(transform.position, Vector2.right.normalized,.5f);

                if (hitRight && hitRight.transform.gameObject.tag == "Wall")
                {
                    if (Input.GetAxisRaw("Horizontal") == -1)
                    {
                        playerJump.touchWall = false;
                        playerJump.slideWall = false;
                    }
                    else player.canMove = false;
                }
                if (hitLeft && hitLeft.transform.gameObject.tag == "Wall")
                {
                    if (Input.GetAxisRaw("Horizontal") == 1)
                    {
                        Debug.LogWarning("Right");
                        playerJump.touchWall = false;
                        playerJump.slideWall = false;
                    }
                    else player.canMove = false;
                }
                else return;
            }
        }
    }
}