using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References:")]
        //Rigidbody reference
        public Rigidbody2D rb; 
        //Animator reference
        Animator animator;
        //EndCutscene script reference
        EndCutscene endCutscene;
        //GrapplingGun script reference
        GrapplingGun gun;
        //GrapplingRope script reference
        GrapplingRope rope;
        //PlayerJump script reference
        PlayerJump playerJump;
        //CameraMovement script reference
        CameraMovement cameraMovement;
        //MovementControl script reference
        MovementControl movementControl;

        [Header("Forces:")]
        //Speed of movement
        public float moveSpeed = 0.5f;
        //Maximum possible speed of movement
        [SerializeField] float maxSpeed = 10f;

        [Header("State:")]
        //Whether Player is grounded
        public bool isGrounded;        
        //Whether Player has stopped
        public bool stopped;
        //Whether Player is falling
        public bool falling;
        //Whether Player can move
        //Redundant I believe, this is
        public bool canMove;

        [Header("Vectors")]
        //Records Last Checkpoint to respawn to
        public Vector3 lastCheckpoint;
        //Tracks Player position
        Vector3 playerPos;
        //Records direction to move to
        Vector2 moveDirection;
        

        private void Awake()
        {
            //Get Rigidbody component
            rb = GetComponent<Rigidbody2D>();
            //Get Animator component
            animator = GetComponent<Animator>();
            //Get GrapplingGun script in children
            gun = GetComponentInChildren<GrapplingGun>();
            //Get GrapplingRope script in children
            rope = GetComponentInChildren<GrapplingRope>();
            //Get PlayerJump script in children
            //Weird, I believe it is not in children but
            //in same Game Object [maybe change it]
            playerJump = GetComponentInChildren<PlayerJump>();
            //Get MovementControl script
            movementControl = GetComponent<MovementControl>();
            //Find CameraMovement script
            cameraMovement = FindObjectOfType<CameraMovement>();
            //Find EndCutscene script
            endCutscene = FindObjectOfType<EndCutscene>();

            //Last Checkpoint equals player's position at that moment
            lastCheckpoint = transform.position;
            //Constraint Player's Rotation
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
            //Set velocity limit to maximum speed
            if (rb.velocity.magnitude >= maxSpeed)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            }
            //If Player is Grappling
            if (rope.isGrappling)
            {
                //Call MoveGrapple method with GetMoveInput as input
                MoveGrapple(GetMoveInput());
            }
            //Else call Move method with GetMoveInput as input
            else Move(GetMoveInput());
            //Call CheckFall method
            CheckFall();
        }
        private void Move(Vector2 direction)
        {
            //If Player is not touching a wall, do nothing
            if (playerJump.touchWall) return;

            //Apply a horizontal force equal to input direction times move speed
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        }
        private void MoveGrapple(Vector2 direction)
        {
            //Add a horizontal, frame independant force 
            //equal to input direction times move speed times 3
            rb.velocity += direction * moveSpeed * Time.deltaTime * 3;
        }
        private Vector2 GetMoveInput()
        {
            //If no input
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                //If horizontal velocity equals 0, Player has stopped
                if (rb.velocity.x ==0) stopped = true;
                //Return Vector zero
                return Vector2.zero;
            }
            //Else Player has not stopped
            else stopped = false;

            //If there is no cutscene playing
            if (!endCutscene.cutscene)
            {
                //Return movement direction equaling
                //Vector 2 right times move speed times horizontal input axis
                return moveDirection =
                        Vector2.right * moveSpeed * Input.GetAxisRaw("Horizontal");
            }
            //Else ther is cutscene playing, return Vector zero
            else return Vector2.zero;
        }
        public void Respawn()
        {
            //Make GrapplingGun's resetGrapple variable true
            gun.resetGrapple = true;
            //CHange Player's position to last checkpoint's
            transform.position = lastCheckpoint;
            //Enable CameraMovement script
            cameraMovement.enabled = true;
            //ResetCamera position to last checkpoint's
            //by calling Reset Camera
            cameraMovement.ResetCamera(lastCheckpoint);
        }
        //Checks whether player is falling or rising and
        //modifies falling state accordingly
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
