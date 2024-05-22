using UnityEngine;

namespace Heaven
{
    public class BetterJump : MonoBehaviour
    {
        [SerializeField] float fallMultiplier = 2.5f;   //Applied downward force
        [SerializeField] float lowJumpMultiplier = 2f;  //Force lightly pressing jump

        Rigidbody2D rb;                                 //Rigidbody reference
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();           //Get Rigidbody component
        }

        // Update is called once per frame
        void Update()
        {
            //If object is falling
            if (rb.velocity.y < 0)
            {
                //Apply a frame independant upward force 
                //product of gravity and (fall multiplier - 1)
                rb.velocity += Vector2.up * Physics2D.gravity.y * 
                    (fallMultiplier - 1) * Time.deltaTime;
            }
            //If the object is jumping but button does not stay pressed
            else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                //Apply a frame independant upward force
                //product of gravity and (low jump multiplier - 1)
                rb.velocity += Vector2.up * Physics2D.gravity.y * 
                    (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }
}