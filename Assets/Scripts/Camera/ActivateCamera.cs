using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class ActivateCamera : MonoBehaviour
    {
        Collider2D collider;        //Collider reference
        CameraMovement cameraMov;   //CameraMovement script reference

        // Start is called before the first frame update
        void Awake()
        {
            //Get the component's collider and make it a trigger
            collider = GetComponent<Collider2D>();
            collider.isTrigger = true;

            //Get a reference to CameraMovement in scene
            cameraMov = FindObjectOfType<CameraMovement>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //If the player collides with the object
            if (other.gameObject.CompareTag("Player"))
            {
                //Enable the condition that makes the camera follow the player
                //And disable this script
                cameraMov.cameraToPlayer = true;
                this.enabled = false;
            }
        }
    }
}