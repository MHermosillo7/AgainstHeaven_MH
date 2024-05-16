using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.U2D;

namespace Heaven
{
    public class RotatePlayer : MonoBehaviour
    {
        PlayerMovement player;
        PlayerJump playerJump;
        EndCutscene endCutscene;
        SpriteRenderer sprite;
        Rigidbody2D rb;
        GameObject aim;

        public Vector2 facingDirection;
        public bool leftWall;
        public bool rightWall;
        // Start is called before the first frame update
        void Awake()
        {
            player = GetComponent<PlayerMovement>();
            playerJump = GetComponent<PlayerJump>();
            endCutscene = FindObjectOfType<EndCutscene>();
            rb = GetComponent<Rigidbody2D>();
            aim = GameObject.FindGameObjectWithTag("Aim");

            sprite = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!endCutscene.cutscene)
            {
                CheckRotation();
            }
        }

        void CheckRotation()
        {
            if (Input.GetAxisRaw("Horizontal") == 0 && player.isGrounded) return;
            else if (Input.GetAxisRaw("Horizontal") == 1)
            {
                sprite.flipX = false;
                facingDirection = Vector2.left;

                if (leftWall == true && Input.GetAxisRaw("Horizontal") == 1)
                {
                    playerJump.slideWall = false;
                    AwayFromWall(Vector2.right);
                }
            }
            else if (Input.GetAxisRaw("Horizontal") == -1)
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
            else if (leftWall)
            {
                sprite.flipX = true;
            }
            else if (rightWall)
            {
                sprite.flipX = false;
            }
        }
        private void AwayFromWall(Vector2 direction)
        {
            rb.velocity += direction * .25f;
        }
    }
}