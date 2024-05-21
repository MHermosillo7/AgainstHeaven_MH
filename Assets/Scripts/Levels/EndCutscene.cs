using System.Collections.Generic;
using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class EndCutscene : MonoBehaviour
    {
        Collider2D collider;
        [SerializeField] Transform targetPosition;
        [SerializeField] GameObject[] playerObjects;
        [SerializeField] List<MonoBehaviour> scripts;

        [Header("Player:")]
        GameObject player;
        Rigidbody2D playerRB;
        PlayerMovement playerScript;
        CameraMovement cameraMovement;
        Animator animator;

        public bool cutscene;
        private float moveSpeed;

        private void Awake()
        {
            collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
            player = GameObject.FindGameObjectWithTag("Player");
            playerRB = player.GetComponent<Rigidbody2D>();
            playerScript = player.GetComponent<PlayerMovement>();
            cameraMovement = FindObjectOfType<CameraMovement>();
            animator = player.GetComponent<Animator>();

            scripts.Add(player.GetComponent<RotatePlayer>());
            scripts.Add(FindObjectOfType<CameraMovement>());
            
            moveSpeed = playerScript.moveSpeed;
        }
        private void Update()
        {
            if (cutscene)
            {
                MovePlayer();
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                cutscene = true;
                playerRB.velocity = new Vector2 (0,0);

                player.GetComponent<Joint2D>().enabled = false;
                for (int i = 0; i < playerObjects.Length; i++)
                {
                    playerObjects[i].SetActive(false);
                }
                for (int i = 0; i < scripts.Count; i++)
                {
                    scripts[i].enabled = false;
                }
                GameObject.Find("RightWall").GetComponent<Collider2D>().enabled = false;
                GameObject.Find("BlockPlayer").GetComponent<Collider2D>().enabled = false;
                animator.SetBool("Cutscene", true);
            }
        }
        //Modified copy from player script
        private void MovePlayer()
        {
            if (player.transform.position.x != targetPosition.position.x)
            {
                if(cameraMovement.moveCondition == CameraMovement.MoveCondition.MoveRight)
                {
                    playerRB.velocity =
                    (new Vector2((Vector2.right * moveSpeed).x * moveSpeed, playerRB.velocity.y));
                }
                else if(cameraMovement.moveCondition == CameraMovement.MoveCondition.MoveLeft)
                {
                    playerRB.velocity =
                    (new Vector2((Vector2.left * moveSpeed).x * moveSpeed, playerRB.velocity.y));
                }
            }
        }
    }   
}

