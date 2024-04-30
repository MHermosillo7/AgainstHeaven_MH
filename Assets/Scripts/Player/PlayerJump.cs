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
        [SerializeField] float wallJumpForce = 10f;
        [SerializeField] float slideSpeed = .2f;

        [Header("State:")]
        public bool touchWall;
        public bool canMove;

        [Header("Jump Buffer")]
        [SerializeField] float holdInputTimer = 0.1f;
        public float jumpBufferTime;
        public bool substractBufferTime;

        public float jumpsLeft = 1;
        // Start is called before the first frame update
        void Start()
        {
            player = GetComponent<Player>();
            rb = GetComponent<Rigidbody2D>();
            canMove = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (jumpsLeft >= 1)//&& player.isGrounded == true 
                {
                    Jump();
                }
                if (touchWall == true)
                {
                    touchWall = false;
                    WallJump();
                }
                else
                {
                    jumpBufferTime = holdInputTimer;
                }
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
            if (touchWall)
            {
                WallSlide();
            }
            if (player.isGrounded)
            {
                touchWall = false;
            }
            
        }
        public void Jump()
        {
            player.isGrounded = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += Vector2.up * jumpForce;
            jumpsLeft = 1;
        }
        public void WallJump()
        {
            Debug.Log("Holero");
            rb.velocity += (Vector2.left * wallJumpForce);
        }
        void WallSlide()
        {
            rb.velocity = new Vector2(.01f, -slideSpeed);
        }
    }

}