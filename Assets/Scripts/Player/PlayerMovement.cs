using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Heaven
{
    public class PlayerMovement : MonoBehaviour
    {
        

        [SerializeField] bool canMove;
        [SerializeField] bool wallJumped;

        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            Vector2 direction = new Vector2(x, y);

            Walk(direction);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            
        }
        private void Walk(Vector2 direction)
        {
            
        }
        private void Jump()
        {
            
        }
        private void WallJump()
        {
            

            
        }
        
    }
}