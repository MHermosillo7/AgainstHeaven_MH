using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class BlockCamera : MonoBehaviour
    {
        Collider2D collider;
        Animator animator;
        CameraMovement cameraMov;
        // Start is called before the first frame update
        void Start()
        {
            collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
            animator = GameObject.Find("CircleWipe").GetComponent<Animator>();
            cameraMov = FindObjectOfType<CameraMovement>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("MainCamera"))
            {
                cameraMov.enabled = false;
            }
            if (other.gameObject.CompareTag("Player"))
            {
                animator.SetTrigger("Start");
            }
        }

    }
}