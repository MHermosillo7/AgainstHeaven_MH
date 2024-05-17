using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof (Collider2D))]
    public class DisableCamera : MonoBehaviour
    {
        //Reference collider that blocks the player at the end of scene
        [SerializeField] Collider2D blockPlayer;    
        Collider2D collider;        //Reference to this object's collider
        CameraMovement cameraMov;   //Reference CameraMovement script
        // Start is called before the first frame update
        void Awake()
        {
            //Get reference CameraMovement script in scene
            cameraMov = FindObjectOfType<CameraMovement>();

            //Get this collider and make it a trigger
            collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //On collision with the player
            if (other.gameObject.CompareTag("Player"))
            {
                //Disable CameraMovement script and
                //the collider that blocks the player
                cameraMov.enabled = false;
                blockPlayer.enabled = true;
            }
        }
    }
}