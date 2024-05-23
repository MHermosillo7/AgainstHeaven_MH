using UnityEngine;

namespace Heaven
{
    public class CameraMovement : MonoBehaviour
    {
        Collider2D collider;    //Collider reference
        PlayerMovement player;  //PlayerMovement reference

        //Controls whether camera moves left or right
        public enum MoveCondition
        {
            MoveLeft,
            MoveRight
        }
        public MoveCondition moveCondition; //Stores move condition value
        public bool cameraToPlayer; //Variable to control movement camera

        // Start is called before the first frame update
        void Awake()
        {
            //Get collider and PlayerMovement references
            player = FindObjectOfType<PlayerMovement>();
            collider = GetComponent<Collider2D>();
        }
        private void FixedUpdate()
        {
            CheckPlayerPosition();  //Check player's position relative to camera

            //If the camera should move right, call MoveRight method
            if (moveCondition == MoveCondition.MoveRight)
            {
                MoveRight();
            }
            //If the camera should move left, call MoveLeft method
            if (moveCondition == MoveCondition.MoveLeft)
            {
                MoveLeft();
            }
        }
        void MoveRight()
        {
            //If the player's velocity is greater than 0 and the camera can move
            if (player.rb.velocity.x > 0 && cameraToPlayer == true)
            {
                //Target position equals player's x coordinates
                Vector3 targetPos = new Vector3
                    (player.transform.position.x, transform.position.y, -10);

                //Move this object to target position by frame independant
                //interpolation
                transform.position = Vector3.Lerp
                    (transform.position, targetPos, Time.deltaTime * 2.5f);
            }
            //If player's velocity is not greater than 0, do not move
            else cameraToPlayer = false;
        }
        void MoveLeft()
        {
            //If the player's velocity is less than 0 and the camera can move
            if (player.rb.velocity.x < 0 && cameraToPlayer == true)
            {
                //Target position equals player's x coordinates
                Vector3 targetPos = new Vector3
                    (player.transform.position.x, transform.position.y, -10);

                //Move this object to target position by interpolation
                transform.position = Vector3.Lerp
                    (transform.position, targetPos, Time.deltaTime * 2.5f);
            }
            //If player's velocity is not less than 0, do not move
            else cameraToPlayer = false;
        }
        public void ResetCamera(Vector3 cameraPos)
        {
            //Change the position to inputted Vector 3's coordinate x
            transform.position = new Vector3
                (cameraPos.x, transform.position.y, transform.position.z);
        }
        void CheckPlayerPosition()
        {
            //If the player is to the left the camera 
            //and camera should move right
            if (player.transform.position.x < transform.position.x
                && moveCondition == MoveCondition.MoveRight)
            {
                //Camera cannot move
                cameraToPlayer = false;
            }
            //If player is to the right of the camera 
            //and camera should move right
            if (player.transform.position.x > transform.position.x
                && moveCondition == MoveCondition.MoveRight)
            {
                //Camera can move
                cameraToPlayer = true;
            }
            //If player is to the right of the camera
            //and camera should move left
            if (player.transform.position.x > transform.position.x
                && moveCondition == MoveCondition.MoveLeft)
            {
                //Camera cannot move
                cameraToPlayer = false;
            }
            //If player is to the left of the camera
            //and camera should move left
            if (player.transform.position.x < transform.position.x
                && moveCondition == MoveCondition.MoveLeft)
            {
                //Camera can move
                cameraToPlayer = true;
            }
        }
    }
}