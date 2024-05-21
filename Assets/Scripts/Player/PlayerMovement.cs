using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References:")]
        public Rigidbody2D rb;
        Animator animator;
        EndCutscene endCutscene;
        GrapplingGun gun;
        GrapplingRope rope;
        PlayerJump playerJump;
        CameraMovement cameraMovement;
        MovementControl movementControl;

        [Header("Forces:")]
        public float moveSpeed = 0.5f;
        [SerializeField] float maxSpeed = 10f;

        [Header("State:")]
        public bool isGrounded;
        public bool stopped;
        public bool falling;
        public bool canMove;

        [Header("Vectors")]
        public Vector3 lastCheckpoint;
        Vector3 playerPos;
        Vector2 moveDirection;
        

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gun = GetComponentInChildren<GrapplingGun>();
            rope = GetComponentInChildren<GrapplingRope>();
            playerJump = GetComponentInChildren<PlayerJump>();
            movementControl = GetComponent<MovementControl>();
            cameraMovement = FindObjectOfType<CameraMovement>();
            endCutscene = FindObjectOfType<EndCutscene>();

            lastCheckpoint = transform.position;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Update is called once per frame
        void Update()
        {
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
            CheckFall();
        }
        private void Move(Vector2 direction)
        {
            if (playerJump.touchWall) return;

            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
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

            if (!endCutscene.cutscene)
            {
                return moveDirection =
                        Vector2.right * moveSpeed * Input.GetAxisRaw("Horizontal");
            }
            else return Vector2.zero;
        }
        public void Respawn()
        {
            gun.enabled = false;
            transform.position = lastCheckpoint;
            cameraMovement.enabled = true;
            cameraMovement.ResetCamera(lastCheckpoint);
            gun.enabled = true;
        }
        private void CheckFall()
        {
            if (rb.velocity.y == 0)
            {
                falling = false;
            }
            else falling = true;
        }
    }
}