using System.Collections.Generic;
using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class EndCutscene : MonoBehaviour
    {
		//Collider reference
        Collider2D collider;	
		//Target position Transform
        [SerializeField] Transform targetPosition;
		//Game Objects related with player methods
        [SerializeField] GameObject[] playerObjects;
		//Scripts that should be turned off at end level
        [SerializeField] List<MonoBehaviour> scripts;

        [Header("Player:")]
		//Player Collider Reference
        GameObject player;	
		//Player Rigidbody reference
        Rigidbody2D playerRB;
		//PlayerMovement script reference
        PlayerMovement playerScript;
		//CameraMovement script reference
        CameraMovement cameraMovement;
		//Player Animator reference
        Animator animator;

		//Whether cutscene is playing
        public bool cutscene;
		//Player movement speed
        private float moveSpeed;

        private void Awake()
        {
			//Get Collider component and make it a trigger
            collider = GetComponent<Collider2D>();
            collider.isTrigger = true;

   			//Find Player Game Object, Get its Rigidbody,
	  		//PlayerMovement script component, and Animator
            player = GameObject.FindGameObjectWithTag("Player");
            playerRB = player.GetComponent<Rigidbody2D>();
            playerScript = player.GetComponent<PlayerMovement>();
            animator = player.GetComponent<Animator>();
			
   			//FInd the CameraMovement script
            cameraMovement = FindObjectOfType<CameraMovement>();

			//Add the RotatePlayer and CameraMovement script to
   			//the scripts list
            scripts.Add(player.GetComponent<RotatePlayer>());
            scripts.Add(FindObjectOfType<CameraMovement>());

			//MoveSpeed equals player's
            moveSpeed = playerScript.moveSpeed;
        }
        private void Update()
        {
			//If cutscene is playing
            if (cutscene)
            {
				//Call for script's MovePlayer method
                MovePlayer();
            }
        }
		//When Game Object is triggered by Player
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
				//Cutscene is playing
                cutscene = true;
				//Reset player's velocity vector
                playerRB.velocity = new Vector2 (0,0);

				//Get Player's SpringJoint2D and deactivate it
                player.GetComponent<Joint2D>().enabled = false;

				//Iterate through player game Objects array
				//And deactivate them
                for (int i = 0; i < playerObjects.Length; i++)
                {
                    playerObjects[i].SetActive(false);
                }
				//Iterate through the scripts list
				//And disable them
                for (int i = 0; i < scripts.Count; i++)
                {
                    scripts[i].enabled = false;
                }
				//Find Game Object's RightWall and BlockPlayer colliders and disable them
                GameObject.Find("RightWall").GetComponent<Collider2D>().enabled = false;
                GameObject.Find("BlockPlayer").GetComponent<Collider2D>().enabled = false;
				//Set Player's Animator bool 'Cutscene' to true
                animator.SetBool("Cutscene", true);
            }
        }
        //Modified copy from player script
		//Since the original idea was to deactivate Player script
  		//This method was copied and slightly modified
		//However, during development, PlayerMovement script was seen as essential
  		//ie. this method could be deleted by making PlayerMovement's public
        private void MovePlayer()
        {
			//If player's x coordinate is not same as target position's
            if (player.transform.position.x != targetPosition.position.x)
            {
				//If camera should move right
                if(cameraMovement.moveCondition == CameraMovement.MoveCondition.MoveRight)
                {
					//Player's velocity equals new vector2 
	 				//(Vector2 right times move speed).x times move speed
	  				//Keep y velocity the same
                    playerRB.velocity =
                    (new Vector2((Vector2.right * moveSpeed).x * moveSpeed, playerRB.velocity.y));
                }
				//Else if camera should move left
                else if(cameraMovement.moveCondition == CameraMovement.MoveCondition.MoveLeft)
                {
					//Player's velocity equals new vector2 
	 				//(Vector2 left times move speed).x times move speed
	  				//Keep y velocity the same
                    playerRB.velocity =
                    (new Vector2((Vector2.left * moveSpeed).x * moveSpeed, playerRB.velocity.y));
                }
            }
        }
    }   
}

