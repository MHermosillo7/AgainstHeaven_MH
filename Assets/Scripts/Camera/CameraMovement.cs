using UnityEngine;

namespace Heaven
{
    public class CameraMovement : MonoBehaviour
    {
        Collider2D collider;    //Collider reference
        PlayerMovement player;  //PlayerMovement reference

        public enum MoveCondition
        {
            MoveLeft,
            MoveRight
        }
        public MoveCondition moveCondition;
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
            if(moveCondition == MoveCondition.MoveRight)
            {
                MoveRight();
            }
            if(moveCondition == MoveCondition.MoveLeft)
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
                    (player.transform.position.x,transform.position.y, -10);

                //Move this object to target position by interpolation
                transform.position = Vector3.Lerp
                    (transform.position, targetPos, Time.deltaTime * 2.5f);
            }
            //If player's velocity is not greater than 0, do not move
            else cameraToPlayer = false;
        }
        void MoveLeft()
        {
            //If the player's velocity is greater than 0 and the camera can move
            if (player.rb.velocity.x < 0 && cameraToPlayer == true)
            {
                //Target position equals player's x coordinates
                Vector3 targetPos = new Vector3
                    (player.transform.position.x, transform.position.y, -10);

                //Move this object to target position by interpolation
                transform.position = Vector3.Lerp
                    (transform.position, targetPos, Time.deltaTime * 2.5f);
            }
            //If player's velocity is not greater than 0, do not move
            else cameraToPlayer = false;
        }
        public void ResetCamera(Vector3 cameraPos)
        {
            //Change the position to inputted Vector 3's coordinate x
            transform.position = new Vector3
                (cameraPos.x, transform.position.y, transform.position.z); ;
        }
        void CheckPlayerPosition()
        {
            //If the player is behind the camera
            if (player.transform.position.x < transform.position.x && moveCondition == MoveCondition.MoveRight)
            {
                //Camera cannot move
                cameraToPlayer = false;
            }
            if (player.transform.position.x > transform.position.x && moveCondition == MoveCondition.MoveRight)
            {
                cameraToPlayer = true;
            }
            if (player.transform.position.x > transform.position.x && moveCondition == MoveCondition.MoveLeft)
            {
                cameraToPlayer = false;
            }
            if (player.transform.position.x < transform.position.x && moveCondition == MoveCondition.MoveLeft)
            {
                cameraToPlayer = true;
            }
        }
    }
}