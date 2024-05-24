using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Heaven
{
    public class PlayerJump : MonoBehaviour
    {
        PlayerMovement player;        //PlayerMovement script reference
        RotatePlayer rotatePlayer;    //RotatePlayer script reference
        Rigidbody2D rb;               //Rigidobdy reference
        EndCutscene endCutscene;      //EndCutscene script reference

        [Header("Forces:")]
        //Force applied vertically when jumping from ground
        [SerializeField] float jumpForce = 6f;
        //Force applied horizontally when jumping from wall
        [SerializeField] float horizontalForce = 3f;
        //Speed of sliding from wall
        [SerializeField] float slideSpeed = .2f;

        [Header("State:")]
        //Whether Player is touching a wall
        public bool touchWall;   
        //Whether Player is sliding down a wall
        public bool slideWall;    
        //Whether Player has jumped
        public bool jumped;       

        [Header("Jump Buffer")]
        //Hold the jump buffer time value for reset purposes
        [SerializeField] float holdBufferTimer = 0.05f;
        //Jump buffer time value to view and apply changes
        public float jumpBufferTime;
        //Whether player has exited a wall
        public bool exitWall;
        //WHether player has exited the ground
        public bool exitGround;

        Vector2 jumpDirection;
        public float jumpsLeft = 1;
        public float storeJumpsLeft;
        // Start is called before the first frame update
        void Awake()
        {
            //Get PlayerMovement and RotatePlayer scripts
            player = GetComponent<PlayerMovement>();
            rotatePlayer = GetComponent<RotatePlayer>();
            //Get Rigidbody component
            rb = GetComponent<Rigidbody2D>();
            //Find EndCutscene script
            endCutscene = FindObjectOfType<EndCutscene>();
            //Hold value of jumps left in a separate variable
            storeJumpsLeft = jumpsLeft;
        }

        // Update is called once per frame
        void Update()
        {
            //If Player is grounded
            if (player.isGrounded)
            {
                //Jump in the upward direction
                jumpDirection = Vector2.up;
            }
            //Else if RotatePlayer script is enabled
            else if (rotatePlayer.enabled) 
                //Jump in upward direction plus the facing direction
                jumpDirection = Vector2.up + rotatePlayer.facingDirection;

            //If Space key is pressed and no cutscene is playing
            if (Input.GetKeyDown(KeyCode.Space) && !endCutscene.cutscene)
            {
                //If Player is not touching a wall or grounded
                if (!touchWall && !player.isGrounded)
                {
                    //Call ResetJumpBuffer method
                    ResetJumpBuffer();
                }
                //If jumps left is greater than or equal to 1
                //And Player has not jumped
                if (jumpsLeft >= 1 && !jumped)
                {
                    //Call Jump method and pass integer 1 as input
                    Jump(1f);
                }
                //If player is touching a wall and is not grounded
                if (touchWall == true && !player.isGrounded)
                {
                    //Player is not sliding down a wall
                    slideWall = false;
                    //Call WallJump method
                    WallJump();
                }
            }
            //If player is sliding down a wall
            //And is not grounded
            if (slideWall && !player.isGrounded)
            {
                //Call WallSlide method
                WallSlide();
            }
            //If RotatePlayer is enabled and Player is touching either
            //left or right wall, Player is touching a wall
            if (rotatePlayer.enabled && rotatePlayer.leftWall || 
                rotatePlayer.enabled && rotatePlayer.rightWall) touchWall = true;

            //If jump buffer time is freater than 0
            if (jumpBufferTime > 0)
            {
                //Decrease its value every frame
                jumpBufferTime -= Time.deltaTime;
            }
            //If Jump buffer time equals 0 or less than 0
            if (jumpBufferTime <= 0)
            {
                //jump buffer time equals 0
                jumpBufferTime = 0;
            }
        }
        public void Jump(float multiplier)
        {
            //Player has jumped
            jumped = true;
            //Reset Player's y velocity to 0
            rb.velocity = new Vector2(rb.velocity.x, 0);

            //Add to Player's velocity the result of
            //JumpDirection vector times jump force times multiplier
            rb.velocity += jumpDirection * jumpForce * multiplier;
            //Decrease jumps left by 1
            jumpsLeft -= 1;
            //Change jump Buffer time to 0
            jumpBufferTime = 0;
        }
        public void WallJump()
        {
            //Player is not sliding down a wall
            slideWall = false;
            // (Apply a vertical force times 3
            //Plus RotatePlayer's facing direction vector times horizontal force)
            //Times jump force times 4
            rb.velocity += ((Vector2.up * 3f) + 
                (rotatePlayer.facingDirection * horizontalForce)) * jumpForce * 4;
            //Player has jumped
            jumped = true;
            //Jump buffer time equals 0
            jumpBufferTime = 0;
        }
        //Change Player's x velocity to 0 and
        //that of y to negative sliding speed
        void WallSlide()
        {
            rb.velocity = new Vector2(0, -slideSpeed);
        }
        //Change jump buffers time value
        //to the one that holds the time
        public void ResetJumpBuffer()
        {
            jumpBufferTime = holdBufferTimer;
        }
    }

}
