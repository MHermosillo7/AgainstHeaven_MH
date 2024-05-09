using UnityEngine;

namespace Heaven
{
    public class EnableGrapple : MonoBehaviour
    {
        GameObject grappleGun;
        GameObject controlGrapple;
        GameObject hookMode;
        GameObject[] grappleObjects = new GameObject[3];
        int i = 0;
        // Start is called before the first frame update
        void Start()
        {
            grappleGun = GameObject.Find("GrapplingGun");
            controlGrapple = GameObject.Find("ControlGrapple");
            hookMode = GameObject.Find("HookMode");

            grappleObjects[0] = grappleGun;
            grappleObjects[1] = controlGrapple;
            grappleObjects[2] = hookMode;
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