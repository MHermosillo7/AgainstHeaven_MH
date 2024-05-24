using UnityEngine.UI;
using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class DisplayTextSign : MonoBehaviour
    {
        Collider2D collider;                    //Collider reference
        [SerializeField] Text text;             //Text reference
        [SerializeField] string displayText;    //String to set displayed text
        [SerializeField] GameObject panel;      //Panel game object
        // Start is called before the first frame update
        void Awake()
        {
            //Get Collider component and make it a trigger
            collider = GetComponent<Collider2D>();
            collider.isTrigger = true;

            //If there is a text reference
            if (text)
            {
                //Leave text displayed blank
                text.text = "";
                //Disable panel game object
                panel.SetActive(false);
            }
        }

        //When object is triggered by player
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                //Enable panel
                panel.SetActive(true);
                //Set text to text to display
                text.text = displayText;
            }
        }
        //When player exits the collider
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                //Leave text blank
                text.text = "";
                //Disable panel
                panel.SetActive(false);
            }
        }
    }
}
