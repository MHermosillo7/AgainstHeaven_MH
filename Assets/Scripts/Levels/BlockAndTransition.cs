using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class BlockCamera : MonoBehaviour
    {
        Collider2D collider;
        [SerializeField] Collider2D blockPlayer;
        Animator animator;
        CameraMovement cameraMov;
        // Start is called before the first frame update
        void Awake()
        {
            collider = GetComponent<Collider2D>();
            collider.isTrigger = true;

            animator = GameObject.Find("CircleWipe").GetComponent<Animator>();
            cameraMov = FindObjectOfType<CameraMovement>();

            blockPlayer.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("MainCamera"))
            {
                cameraMov.enabled = false;
                blockPlayer.enabled = true;
            }
            if (other.gameObject.CompareTag("Player"))
            {
                animator.SetTrigger("Start");
            }
        }

    }
}