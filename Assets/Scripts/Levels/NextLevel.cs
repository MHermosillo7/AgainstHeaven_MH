using UnityEngine;
using UnityEngine.SceneManagement;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class NextLevel : MonoBehaviour
    {
        public Animator animator;
        Collider2D collider;
        private void Start()
        {
            collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
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

