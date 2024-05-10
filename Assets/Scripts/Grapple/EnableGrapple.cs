using UnityEngine;

namespace Heaven
{
    public class EnableGrapple : MonoBehaviour
    {
        [SerializeField] GameObject[] grappleObjects;
        int i = 0;
        // Start is called before the first frame update
        void Start()
        {
            for (int a = 0; a < grappleObjects.Length; a++)
            {
                grappleObjects[a].SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (i == 0)
                {
                    for(int a = 0; a < grappleObjects.Length; a++)
                    {
                        grappleObjects[a].SetActive(true);
                    }
                    i++;
                }
            }
        }
    }
}