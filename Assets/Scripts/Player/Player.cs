using System.Collections;
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
        PlayerJump playerJump;
        CameraMovement cameraMovement;
        MovementControl movementControl;
        SpriteRenderer sprite;
        GameObject aim;

        [Header("Forces:")]
        [SerializeField] float moveSpeed = 0.5f;
        [SerializeField] float maxSpeed = 10f;

        [Header("State:")]
        public bool isGrounded;
        public bool leftWall;
        public bool rightWall;
        public bool stopped;
        public bool falling;
        public bool canMove;

        [Header("Vectors")]
        public Vector3 lastCheckpoint;
        public Vector2 facingDirection;
        Vector3 playerPos;
        Vector2 moveDirection;
        

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            rope = GetComponentInChildren<GrapplingRope>();
            playerJump = GetComponentInChildren<PlayerJump>();
            cameraMovement = FindObjectOfType<CameraMovement>();
            movementControl = GetComponent<MovementControl>(); ;
            sprite = GetComponent<SpriteRenderer>();
            aim = GameObject.FindGameObjectWithTag("Aim");

            Cursor.visible = false;
            lastCheckpoint = transform.position;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Update is called once per frame
        void Update()
        {
            if(Mathf.RoundToInt(transform.position.x) == Mathf.RoundToInt(Camera.main.transform.position.x))
            {
                cameraMovement.cameraToPlayer = true;
            }
            RotatePlayer();
            CheckFall();
            //Set velocity limit
            if (rb.velocity.magnitude >= maxSpeed)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            }

            //Control forces applied to player when
            //moving horizontally depending on state isGrounded
            if (rope.isGrappling)
            {
                MoveGrapple(GetMoveInput());
            }
            else Move(GetMoveInput());
            //Set animator values to player equivalents
            if (animator)
            {
                animator.SetFloat("XVelocity", rb.velocity.x);
                animator.SetFloat("YVelocity", rb.velocity.y);
                animator.SetBool("Stopped", stopped);
                animator.SetBool("Falling", falling);
                animator.SetBool("IsGrounded", isGrounded);
                animator.SetBool("TouchWall", playerJump.touchWall);
                animator.SetBool("IsGrappling", rope.isGrappling);
            }
        }
        void RotatePlayer()
        {
            if (Input.GetAxisRaw("Horizontal") == 0 && isGrounded) return;
            else if (Input.GetAxisRaw("Horizontal") == 1 || rb.velocity.x > 0)
            {
                sprite.flipX = false;
                facingDirection = Vector2.left;

                if (leftWall == true && Input.GetAxisRaw("Horizontal") == 1)
                {
                    playerJump.slideWall = false;
                    AwayFromWall(Vector2.right);
                }
            }
            else if (Input.GetAxisRaw("Horizontal") == -1 || rb.velocity.x < 0)
            {
                sprite.flipX = true;
                facingDirection = Vector2.right;

                if (rightWall == true && Input.GetAxisRaw("Horizontal") == -1)
                {
                    playerJump.slideWall = false;
                    AwayFromWall(Vector2.left);
                }
            }
            else if (aim && aim.transform.position.x > transform.position.x
                && !playerJump.touchWall)
            {
                sprite.flipX = false;
            }
            else if (aim && aim.transform.position.x < transform.position.x
                && !playerJump.touchWall)
            {
                sprite.flipX = true;
            }
            else if(playerJump.touchWall && leftWall)
            {
                sprite.flipX = true;
            }
            else if(playerJump.touchWall && rightWall)
            {
                sprite.flipX = false;
            }
        }
        private void Move(Vector2 direction)
        {
            if (playerJump.touchWall) return;

            rb.velocity = (new Vector2(direction.x * moveSpeed, rb.velocity.y));
        }
        private void MoveGrapple(Vector2 direction)
        {
            rb.velocity += direction * moveSpeed * Time.deltaTime * 3;
        }
        private Vector2 GetMoveInput()
        {
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                if (rb.velocity.x ==0) stopped = true;
                return Vector2.zero;
            }
            else stopped = false;

            return moveDirection = 
                    Vector2.right * moveSpeed * Input.GetAxisRaw("Horizontal");
        }
        public void Respawn()
        {
            transform.position = lastCheckpoint;
            cameraMovement.ResetCamera(lastCheckpoint);
        }
        private void CheckFall()
        {
            if (rb.velocity.y == 0)
            {
                falling = false;
            }
            else falling = true;
        }
        private void AwayFromWall(Vector2 direction)
        {
            rb.velocity += direction * .25f;
        }
    }
}