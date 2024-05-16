using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof (Collider2D))]
    public class DisableCamera : MonoBehaviour
    {
        [SerializeField] Collider2D blockPlayer;
        Collider2D collider;
        CameraMovement cameraMov;
        // Start is called before the first frame update
        void Awake()
        {
            cameraMov = FindObjectOfType<CameraMovement>();
            collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                cameraMov.enabled = false;
                blockPlayer.enabled = true;
            }
        }
    }
}