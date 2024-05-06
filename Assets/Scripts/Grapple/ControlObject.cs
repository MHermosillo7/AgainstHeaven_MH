using System.Collections;
using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class ControlObject : MonoBehaviour
    {
        Collider2D collider;
        SpriteRenderer spriteRenderer;
        [SerializeField] float appearRate;
        // Start is called before the first frame update
        void Start()
        {
            collider = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public IEnumerator Deactivate()
        {
            for(int i = 0; i < 2; i++)
            {
                if (collider.enabled)
                {
                    collider.enabled = false;
                    spriteRenderer.enabled = false;
                }
                else
                {
                    collider.enabled = true;
                    spriteRenderer.enabled = true;
                }
                yield return new WaitForSeconds(appearRate);
            }
        }


    }
}