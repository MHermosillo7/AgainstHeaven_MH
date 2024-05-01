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
                if (jumpsLeft >= 1)//&& player.isGrounded == true 
                {
                    Jump();
                }
                if (touchWall == true)
                {
                    touchWall = false;
                    Jump();
                }
                else
                {
                    jumpBufferTime = holdBufferTimer;
                }
            }
            if (player.isGrounded)
            {
                touchWall = false;
            }
            if (touchWall)
            {
                WallSlide();
            }
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
        public void Jump()
        {
            player.isGrounded = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);

            rb.velocity += jumpDirection * jumpForce;
            jumpsLeft -= 1;
        }
        public void WallJump()
        {
            //WorkInProgress
            //rb.velocity += (Vector2.up + player.facingDirection) * jumpForce;
        }
        void WallSlide()
        {
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
        }
    }

}