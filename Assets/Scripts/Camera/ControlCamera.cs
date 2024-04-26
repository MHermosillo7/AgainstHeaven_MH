using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class ControlCamera : MonoBehaviour
    {
        MoveCamera moveCamera;
        Transform childPosition;
        // Start is called before the first frame update
        void Start()
        {
            moveCamera = FindObjectOfType<MoveCamera>();
        }
        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                //If there is no transform stored,
                //focus camera on player
                if (!childPosition)
                {
                    moveCamera.focusCamera = true;
                }
                //if there is a transform stored,
                //Do not focus camera on player
                //and move it to stored position
                else
                {
                    moveCamera.focusCamera = false;
                    moveCamera.targetPosition = childPosition;
                }
            }
        }
        //When exiting collider, re-enable focus on camera
        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                moveCamera.focusCamera = true;
            }
        }
    }
}