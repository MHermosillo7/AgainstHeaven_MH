using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heaven
{
    public class EnableGrapple : MonoBehaviour
    {
        GameObject grappleGun;
        int i = 0;
        // Start is called before the first frame update
        void Start()
        {
            grappleGun = GameObject.Find("GrapplingGun");
            grappleGun.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (i == 0)
                {
                    grappleGun.SetActive(true); 
                    i++;
                }
            }
        }
    }
}