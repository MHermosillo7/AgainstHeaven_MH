using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class ActivateCamera : MonoBehaviour
    {
        Collider2D collider;
        CameraMovement cameraMov;
        // Start is called before the first frame update
        void Awake()
        {
            collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
            cameraMov = FindObjectOfType<CameraMovement>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                cameraMov.cameraToPlayer = true;
                this.enabled = false;
            }
        }
    }
}