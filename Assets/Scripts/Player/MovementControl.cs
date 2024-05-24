using UnityEngine;

//ABANDONED SCRIPT
//UNFINISHED
//NO LONGER IN USE
//Found a better, simpler solution later in development
namespace Heaven
{
    public class MovementControl : MonoBehaviour
    {
        PlayerMovement player;        //PlayerMovement script reference
        PlayerJump playerJump;        //PlayerJump script reference

        RaycastHit2D hitLeft;         //Store RaycastHit in left direction
        RaycastHit2D hitRight;        //Store RaycastHit in right direction
        RaycastHit2D diagonalRight;   //Store RaycastHit in diagonal right direction
        RaycastHit2D diagonalLeft;    //Store RaycastHit in diagonal left direction
        // Start is called before the first frame update
        void Awake()
        {
            //Get PlayerMovement and PlayerJump scripts
            player = GetComponent<PlayerMovement>();
            playerJump = GetComponent<PlayerJump>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            //Check whether there is wall in x direction
            //ie. Call Check Wall method
            CheckWall();
        }
        void CheckWall()
        {
            //If player is touching a wall
            if (playerJump.touchWall)
            {
                //Shoot two Raycasts, one in left direction and other in right, and store
                //the position where they hit an object
                hitLeft = Physics2D.Raycast(transform.position, Vector2.left.normalized, .5f);
                hitRight = Physics2D.Raycast(transform.position, Vector2.right.normalized,.5f);

                //If there was a game object hit to the right with tag Wall
                if (hitRight && hitRight.transform.gameObject.tag == "Wall")
                {
                    //If Input corresponds with left direction
                    if (Input.GetAxisRaw("Horizontal") == -1)
                    {
                        //Player is no longer touching a wall
                        playerJump.touchWall = false;
                        //Player is not sliding down a wall
                        playerJump.slideWall = false;
                    }
                    //Else player cannot move
                    else player.canMove = false;
                }
                //If there was a game object hit to left with tag wall
                if (hitLeft && hitLeft.transform.gameObject.tag == "Wall")
                {
                    //If input corresponds with right direction
                    if (Input.GetAxisRaw("Horizontal") == 1)
                    {
                        //Print Warning "Right"
                        Debug.LogWarning("Right");
                        //Player is not touching a wall
                        playerJump.touchWall = false;
                        //Player is not sliding down a wall
                        playerJump.slideWall = false;
                    }
                    //Else player cannot move
                    else player.canMove = false;
                }
                //If player is not touching a wall, return
                else return;
            }
        }
    }
}
