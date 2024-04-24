using UnityEngine;

namespace HeavenAndHell
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [Header("References:")]
        public Rigidbody2D rb;
        Animator animator;
        GrapplingRope rope;
        SpriteRenderer sprite;
        GameObject aim;

        [Header("Forces:")]
        public float jumpForce = 6f;
        public float moveSpeed = 0.5f;
        public float dashSpeed = 1f;
        public float fallSpeed = 0.4f;
        public float airResistance = 1f;

        [Header("State:")]
        public bool isGrounded;
        public bool touchWall;
        public bool stopped;

        [Header("Vectors")]
        public Vector3 lastCheckpoint;
        Vector2 facingDirection;
        Vector2 moveDirection;

        [Header("Others:")]
        public float jumpsLeft = 1;
        [SerializeField] float maxSpeed = 10f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            rope = GetComponentInChildren<GrapplingRope>();
            sprite = GetComponent<SpriteRenderer>();
            aim = GameObject.FindGameObjectWithTag("Aim");

            lastCheckpoint = transform.position;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Update is called once per frame
        void Update()
        {
            RotatePlayer();
            if (rb.velocity.magnitude >= maxSpeed)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            }

            if (isGrounded)
            {
                MoveGround(GetMoveInput());
            }
            else MoveAir(GetMoveInput());

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(jumpsLeft >= 1 && isGrounded == true)
                {
                    Jump();
                }
                else if (touchWall == true)
                {
                    WallJump();
                }
            }
            if (animator)
            {
                animator.SetFloat("XVelocity", rb.velocity.x);
                animator.SetFloat("YVelocity", rb.velocity.y);
                animator.SetBool("Stopped", stopped);
                animator.SetBool("IsGrounded", isGrounded);
                animator.SetBool("IsGrappling", rope.isGrappling);
            }
        }
        void RotatePlayer()
        {
            if(Input.GetAxisRaw("Horizontal") == 1 || rb.velocity.x > 0)
            {
                sprite.flipX = false;
                facingDirection = Vector2.left;
            }
            else if(Input.GetAxisRaw("Horizontal") == -1 || rb.velocity.x < 0)
            {
                sprite.flipX = true;
                facingDirection = Vector2.right;
            }
            else if(aim.transform.position.x > transform.position.x)
            {
                sprite.flipX = false;
            }
            else if(aim.transform.position.x < transform.position.x)
            {
                sprite.flipX = true;
            }
            
        }
        private void MoveGround(Vector2 moveDirection)
        {
            rb.velocity = moveDirection;
        }
        void MoveAir(Vector2 moveDirection)
        {
            rb.AddForce(moveDirection / airResistance, ForceMode2D.Force);
        }
        private Vector2 GetMoveInput()
        {
            if (rb.velocity.magnitude == 0 && Input.GetAxisRaw("Horizontal") == 0)
            {
                stopped = true;

                return Vector2.zero;
            }
            else stopped = false;
            
            return moveDirection = 
                    Vector2.right * moveSpeed * Input.GetAxisRaw("Horizontal");


        }
        private void Jump()
        {
            isGrounded = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpsLeft = 0;
        }

        void WallJump()
        {
            rb.AddForce((facingDirection + Vector2.up) * jumpForce / 1.2f, 
                ForceMode2D.Impulse);
            touchWall = false;
        }
        public void Respawn()
        {
            transform.position = lastCheckpoint;
        }
    }
}