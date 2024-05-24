using UnityEngine;

//ATTACH TO HAZARD TILEMAP
namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class RespawnPlayer : MonoBehaviour
    {
        PlayerMovement player;    //PlayerMovement script reference
        Collider2D collider;      //Collider reference
        // Start is called before the first frame update
        void Start()
        {
            //FInd PlayerMovement script
            player = FindObjectOfType<PlayerMovement>();
            //Get Collider component
            collider = GetComponent<Collider2D>();
        }

        //On Collision with Player Game Object
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {    
                //Call PlayerMovement's Respawn method
                player.Respawn();
            }
        }
    }
}
