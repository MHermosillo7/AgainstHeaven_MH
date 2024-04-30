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
        [SerializeField] float moveSpeed = 0.5f;
        [SerializeField] float maxSpeed = 10f;

        [Header("State:")]
        public bool isGrounded;
        public bool stopped;
        public bool canMove;

        [Header("Vectors")]
        public Vector3 lastCheckpoint;
        public Vector2 facingDirection;
        Vector2 moveDirection;
        

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
            if (rb.velocity.x == 0 && Input.GetAxisRaw("Horizontal") == 0)
            {
                stopped = true;

                return Vector2.zero;
            }
            else stopped = false;
            
            return moveDirection = 
                    Vector2.right * moveSpeed * Input.GetAxisRaw("Horizontal");
        }
        public void Respawn()
        {
            transform.position = lastCheckpoint;
        }
    }
}