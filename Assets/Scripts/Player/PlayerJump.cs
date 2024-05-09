using System.Collections;
using UnityEngine;

namespace Heaven
{
    public class PlayerJump : MonoBehaviour
    {
        Player player;
        Rigidbody2D rb;
        [Header("Forces:")]
        [SerializeField] float jumpForce = 6f;
        [SerializeField] float slideSpeed = .2f;

        [Header("State:")]
        public bool touchWall;
        public bool slideWall;
        public bool jumped;

        [Header("Jump Buffer")]
        [SerializeField] float holdBufferTimer = 0.1f;
        public float jumpBufferTime;
        public bool substractBufferTime;

        Vector2 jumpDirection;
        public float jumpsLeft = 1;
        public float storeJumpsLeft;
        // Start is called before the first frame update
        void Start()
        {
            player = GetComponent<Player>();
            rb = GetComponent<Rigidbody2D>();
            storeJumpsLeft = jumpsLeft;
        }

        // Update is called once per frame
        void Update()
        {
            if (player.isGrounded)
            {
                jumpDirection = Vector2.up;
            }
            else jumpDirection = Vector2.up + player.facingDirection;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (jumpsLeft >= 1 && !jumped)//&& player.isGrounded == true 
                {
                    Jump(1f);
                }
                if (touchWall == true && !player.isGrounded)
                {
                    slideWall = false;
                    WallJump();
                }
                else
                {
                    jumpBufferTime = holdBufferTimer;
                }
            }
            if (slideWall && !player.isGrounded)
            {
                WallSlide();
            }
            if (player.leftWall || player.rightWall) touchWall = true;

            //Control buffer time
            if (substractBufferTime || jumpBufferTime > 0)
            {
                jumpBufferTime -= Time.deltaTime;
            }
            if (jumpBufferTime <= 0)
            {
                substractBufferTime = false;
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
            rb.velocity += ((Vector2.up *2f) + player.facingDirection) * jumpForce * 2f;
            jumped = true;
            jumpBufferTime = 0;
        }
        void WallSlide()
        {
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
        }
    }

}