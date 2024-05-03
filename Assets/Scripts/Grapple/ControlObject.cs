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
        }
        public IEnumerator Deactivate()
        {
            for(int i = 0; i < 2; i++)
            {
                if ()
                {

                }
                Debug.Log("Hola");
                yield return new WaitForSeconds(appearRate);
            }
        }


    }
}