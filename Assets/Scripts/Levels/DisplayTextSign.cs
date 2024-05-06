using UnityEngine.UI;
using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class DisplayTextSign : MonoBehaviour
    {
        Collider2D collider;
        [SerializeField] Text text;
        [SerializeField] string displayText;
        // Start is called before the first frame update
        void Start()
        {
            collider = GetComponent<Collider2D>();
            text = GameObject.Find("Instructions").GetComponent<Text>();
            collider.isTrigger = true;
            if (text)
            {
                text.text = "";
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                text.text = displayText;
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                text.text = "";
            }
        }
    }
}