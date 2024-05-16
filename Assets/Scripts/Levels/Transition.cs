using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class Transition : MonoBehaviour
    {
        Collider2D collider;
        Animator animator;
        // Start is called before the first frame update
        void Awake()
        {
            collider = GetComponent<Collider2D>();
            collider.isTrigger = true;

            animator = GameObject.Find("CircleWipe").GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                animator.SetTrigger("Start");
            }
        }

    }
}