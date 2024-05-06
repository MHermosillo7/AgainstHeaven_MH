
using UnityEngine;

namespace Heaven
{
    public class CameraMovement : MonoBehaviour
    {
        Collider2D collider;
        Player player;

        public bool cameraToPlayer;
        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<Player>();
            collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
        }
        private void Update()
        {
            if (player.rb.velocity.x > 0 && cameraToPlayer == true)
            {
               transform.position = new Vector3
                    (player.transform.position.x, transform.position.y, transform.position.z);
            }
        }
        public void ResetCamera(Vector3 cameraPos)
        {
            transform.position = cameraPos;
        }
    }
}