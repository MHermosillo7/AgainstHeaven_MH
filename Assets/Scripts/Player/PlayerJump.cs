using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Heaven
{
    public class PlayerJump : MonoBehaviour
    {
        PlayerMovement player;
        RotatePlayer rotatePlayer;
        Rigidbody2D rb;
        EndCutscene endCutscene;

        [Header("Forces:")]
        [SerializeField] float jumpForce = 6f;
        [SerializeField] float horizontalForce = 3f;
        [SerializeField] float slideSpeed = .2f;

        [Header("State:")]
        public bool touchWall;
        public bool slideWall;
        public bool jumped;

        [Header("Jump Buffer")]
        [SerializeField] float holdBufferTimer = 0.05f;
        public float jumpBufferTime;
        public bool exitWall;
        public bool exitGround;

        Vector2 jumpDirection;
        public float jumpsLeft = 1;
        public float storeJumpsLeft;
        // Start is called before the first frame update
        void Awake()
        {
            player = GetComponent<PlayerMovement>();
            rotatePlayer = GetComponent<RotatePlayer>();
            rb = GetComponent<Rigidbody2D>();
            endCutscene = FindObjectOfType<EndCutscene>();
            storeJumpsLeft = jumpsLeft;
        }

        // Update is called once per frame
        void Update()
        {
            if (player.isGrounded)
            {
                jumpDirection = Vector2.up;
            }
            else if (rotatePlayer.enabled) 
                jumpDirection = Vector2.up + rotatePlayer.facingDirection;

            if (Input.GetKeyDown(KeyCode.Space) && !endCutscene.cutscene)
            {
                if (!touchWall && !player.isGrounded)
                {
                    ResetJumpBuffer();
                }
                if (jumpsLeft >= 1 && !jumped)
                {
                    Jump(1f);
                }
                if (touchWall == true && !player.isGrounded)
                {
                    slideWall = false;
                    WallJump();
                }
            }
            if (slideWall && !player.isGrounded)
            {
                WallSlide();
            }
            if (rotatePlayer.enabled && rotatePlayer.leftWall || 
                rotatePlayer.enabled && rotatePlayer.rightWall) touchWall = true;

            //Control buffer time
            if (jumpBufferTime > 0)
            {
                jumpBufferTime -= Time.deltaTime;
            }
            if (jumpBufferTime <= 0)
            {
                jumpBufferTime = 0;
            }
        }
        public void Jump(float multiplier)
        {
            jumped = true;
            rb.velocity = new Vector2(rb.velocity.x, 0);

            rb.velocity += jumpDirection * jumpForce * multiplier;
            jumpsLeft -= 1;
            jumpBufferTime = 0;
        }
        public void WallJump()
        {
            slideWall = false;
            rb.velocity += ((Vector2.up * 3f) + 
                (rotatePlayer.facingDirection * horizontalForce)) * jumpForce * 4;
            jumped = true;
            jumpBufferTime = 0;
        }
        void WallSlide()
        {
            rb.velocity = new Vector2(0, -slideSpeed);
        }
        public void ResetJumpBuffer()
        {
            jumpBufferTime = holdBufferTimer;
        }
    }

}