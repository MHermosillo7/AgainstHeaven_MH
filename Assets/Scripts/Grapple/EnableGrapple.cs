using UnityEngine;

namespace Heaven
{
    public class EnableGrapple : MonoBehaviour
    {
        //GameObject's related to grapple methods
        [SerializeField] GameObject[] grappleObjects;

        //Int to ensure only one cycle occurs
        int i = 0;
        // Start is called before the first frame update
        void Awake()
        {
            //Iterate through the GameObject array and deactivate them
            for (int a = 0; a < grappleObjects.Length; a++)
            {
                grappleObjects[a].SetActive(false);
            }
        }

        //On trigger by the object tagged Player
        //Make sure integer i is 0, and if yes
        //Iterate through GameObject array and activate them
        //Then add one to i so cycle happens only once
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