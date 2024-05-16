using UnityEngine;

namespace Heaven
{
    public class CameraMovement : MonoBehaviour
    {
        Collider2D collider;
        PlayerMovement player;

        public bool cameraToPlayer;
        // Start is called before the first frame update
        void Awake()
        {
            player = FindObjectOfType<PlayerMovement>();
            collider = GetComponent<Collider2D>();
        }
        private void FixedUpdate()
        {
            CheckPlayerPosition();
            GroundedLevel();
        }
        void GroundedLevel()
        {
            if (player.rb.velocity.x > 0 && cameraToPlayer == true)
            {
                Vector3 targetPos = new Vector3(player.transform.position.x,transform.position.y, -10);
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 2);
            }
            else cameraToPlayer = false;
        }
        public void ResetCamera(Vector3 cameraPos)
        {
            transform.position = new Vector3
                (cameraPos.x, transform.position.y, transform.position.z); ;
        }
        void CheckPlayerPosition()
        {
            if (player.transform.position.x < transform.position.x)
            {
                cameraToPlayer = false;
            }
            else cameraToPlayer = true;
        }
    }
}