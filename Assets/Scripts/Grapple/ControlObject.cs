using System.Collections;
using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class ControlObject : MonoBehaviour
    {
        Collider2D collider;                //Collider reference
        SpriteRenderer spriteRenderer;      //Sprite Renderer reference
        [SerializeField] float appearRate;  //Time to re-appear

        // Start is called before the first frame update
        void Awake()
        {
            //Get Collider and Sprite Renderer components
            collider = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public IEnumerator Deactivate()
        {
            //Run the loop two times
            for (int i = 0; i < 2; i++)
            {
                //If the collider is enabled
                if (collider.enabled)
                {
                    //Deactivate collider and sprite renderer
                    collider.enabled = false;
                    spriteRenderer.enabled = false;
                }
                //Else collider is disabled
                else
                {
                    //Enable collider and sprite renderer
                    collider.enabled = true;
                    spriteRenderer.enabled = true;
                }
                //Wait for a number of seconds equivalent to
                //appearRate before running the loop again
                yield return new WaitForSeconds(appearRate);
            }
        }
    }
}