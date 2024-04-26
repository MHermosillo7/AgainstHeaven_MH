using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class NextLevel : MonoBehaviour
    {
        [SerializeField] string nextLevel;
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameManager.LoadScene(nextLevel);
            }
        }
    }
}

