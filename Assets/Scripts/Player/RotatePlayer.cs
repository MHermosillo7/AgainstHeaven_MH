using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.U2D;

namespace Heaven
{
    public class RotatePlayer : MonoBehaviour
    {
        PlayerMovement player;    //PlayerMovement script reference
        PlayerJump playerJump;    //PlayerJump script reference
        EndCutscene endCutscene;  //EndCutscene script reference
        SpriteRenderer sprite;    //Sprite Renderer reference
        Rigidbody2D rb;           //Rigidbody reference
        GameObject aim;           //Game Object "Aim" reference

        public Vector2 facingDirection;    //Facing Direction vector
        public bool leftWall;              //Whether touching wall to left
        public bool rightWall;             //Whether touching wall to right
        // Start is called before the first frame update
        void Awake()
        {
            //Get Player Movement script
            player = GetComponent<PlayerMovement>();
            //Get PlayerJump script
            playerJump = GetComponent<PlayerJump>();
            //Find EndCutscene script
            endCutscene = FindObjectOfType<EndCutscene>();
            //Get Rigidbody component
            rb = GetComponent<Rigidbody2D>();
            //FInd Object with tag Aim
            aim = GameObject.FindGameObjectWithTag("Aim");
            //Get Sprite Renderer component
            sprite = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            //If no cutscene is playing
            if (!endCutscene.cutscene)
            {
                //Call CheckRotation method
                CheckRotation();
            }
        }

        void CheckRotation()
        {
            //If there is no input and player is grounded, do nothing
            if (Input.GetAxisRaw("Horizontal") == 0 && player.isGrounded) return;

            //If input corresponds right direction
            else if (Input.GetAxisRaw("Horizontal") == 1)
            {
                //Sprite is flipped
                sprite.flipX = false;
                //Facing direction equals left vector
                facingDirection = Vector2.left;

                //If Player is touching a wall to left and Input
                //corresponds right direction
                if (leftWall == true && Input.GetAxisRaw("Horizontal") == 1)
                {
                    //Player is not sliding down a wall
                    playerJump.slideWall = false;
                    //Call AwayFromWall method with right vector as input
                    AwayFromWall(Vector2.right);
                }
            }
            //Else if input corresponds left direction
            else if (Input.GetAxisRaw("Horizontal") == -1)
            {
                //Flip sprite along x plane
                sprite.flipX = true;

                //Facing direction equals right vector
                facingDirection = Vector2.right;

                //If Player is touching a wall to right and Input
                //corresponds left direction
                if (rightWall == true && Input.GetAxisRaw("Horizontal") == -1)
                {
                    //Player is not sliding down a wall
                    playerJump.slideWall = false;

                    //Call AwayFromWall method with left vector as input
                    AwayFromWall(Vector2.left);
                }
            }
            //Else if aim is active and its position is to right of Player
            //And Player is not touching a wall
            else if (aim && aim.transform.position.x > transform.position.x
                && !playerJump.touchWall)
            {
                //Sprite is not flipped
                sprite.flipX = false;
            }
            //Else if aim is enabled and its position is left of Player
            //And Player is not touching a wall
            else if (aim && aim.transform.position.x < transform.position.x
                && !playerJump.touchWall)
            {
                //Flip sprite along x plane
                sprite.flipX = true;
            }
            //Else if player is touching a wall to left
            else if (leftWall)
            {
                //Flip sprite along x plane
                sprite.flipX = true;
            }
            //Else if player is touching a wall to right
            else if (rightWall)
            {
                //Sprite is not flipped
                sprite.flipX = false;
            }
        }
        //Push Player to input direction
        //Serves to manually control the 
        //force conflicts and overrides
        private void AwayFromWall(Vector2 direction)
        {
            rb.velocity += direction * .25f;
        }
    }
}
