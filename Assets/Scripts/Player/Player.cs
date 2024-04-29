using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Heaven
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
        public float wallJumpForce = 10f;
        public float moveSpeed = 0.5f;
        public float slideSpeed = .2f;
        public float airResistance = 1f;

        [Header("State:")]
        public bool isGrounded;
        public bool touchWall;
        public bool stopped;
        public bool canMove;

        [Header("Vectors")]
        public Vector3 lastCheckpoint;
        Vector2 facingDirection;
        Vector2 moveDirection;

        [Header("Jump Buffer")]
        [SerializeField] float holdInputTimer = 0.1f;
        public float jumpBufferTime;
        public bool substractBufferTime;

        [Header("Others:")]
        [SerializeField] float maxSpeed = 10f;
        public float jumpsLeft = 1;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            rope = GetComponentInChildren<GrapplingRope>();
            sprite = GetComponent<SpriteRenderer>();
            aim = GameObject.FindGameObjectWithTag("Aim");

            canMove = true;
            lastCheckpoint = transform.position;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Update is called once per frame
        void Update()
        {
            //Rotate player according to velocity
            //or aim direction
            RotatePlayer();

            //Set velocity limit
            if (rb.velocity.magnitude >= maxSpeed)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            }

            //Control forces applied to player when
            //moving horizontally depending on state isGrounded
            MoveGround(GetMoveInput());

            //Control whether to jump or wall jump depending on
            //conditions or hold input for short time
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(jumpsLeft >= 1 && isGrounded == true || jumpsLeft >= 1 && jumpBufferTime > 0)
                {
                    Jump();
                }
                else if (touchWall == true)
                {
                    WallJump();
                }
                else
                {
                    jumpBufferTime = holdInputTimer;
                }
            }
            //Control buffer time
            if (substractBufferTime)
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
            //Set animator values to player equivalents
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
        private void MoveGround(Vector2 direction)
        {
            if (!canMove) return;

            rb.velocity = (new Vector2(direction.x * moveSpeed, rb.velocity.y));
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
        public void Jump()
        {
            isGrounded = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += Vector2.up * jumpForce;
            jumpsLeft = 0;
        }

        public void WallJump()
        {
            if (!canMove) return;

            StopCoroutine(DisableMovement(0));
            StartCoroutine(DisableMovement(.1f));

            rb.velocity += ((facingDirection * wallJumpForce + Vector2.up) * jumpForce);
            touchWall = false;
        }
        void WallSlide()
        {
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
        }
        IEnumerator DisableMovement(float time)
        {
            canMove = false;
            yield return new WaitForSeconds(time);
            canMove = true;
        }
        public void Respawn()
        {
            transform.position = lastCheckpoint;
        }
    }
}