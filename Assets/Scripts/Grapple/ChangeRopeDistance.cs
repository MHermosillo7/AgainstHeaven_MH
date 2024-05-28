using UnityEngine;

namespace Heaven
{
    public class ChangeRopeDistance : MonoBehaviour
    {
        GrapplingGun gun;       //GrapplingGun script reference
        GrapplingRope rope;     //GrapplingRope script reference

        [SerializeField] float changeSpeed = 1f;    //Speed change joint2D length
        [SerializeField] float maxDistance = 5f;    //Maximum distance of joint2D
        private float input;                        //Store vertical input

        // Start is called before the first frame update
        void Awake()
        {
            gun = FindObjectOfType<GrapplingGun>();     //Find GrapplingGun script
            rope = FindObjectOfType<GrapplingRope>();   //Find GrapplingRope script
        }

        // Update is called once per frame
        void Update()
        {
            //If there is GrapplingGun and Rope in scene
            if(gun && rope)
            {
                //If the player is grappling and is not launched to hit point
                if (rope.isGrappling && gun.launchToPoint == false)
                {
                    //Call ChangeLength script
                    ChangeLength();  
                    
                    //Joint2D's distance equals GrapplingGun's target distance
                    gun.joint2D.distance = gun.targetDistance;
                }
            }
        }

        void ChangeLength()
        {
            //Get Vertical input as either 0, 1, or -1
            input = Input.GetAxisRaw("Vertical");

            //If targetDistance is greater or equal to 0
            //and is less than or equal to maxDistance
            if (gun.targetDistance >= 0 && gun.targetDistance <= maxDistance)
            {
                //Add the product of the frame independant
                //input times change speed times -1
                gun.targetDistance += -1 * input * changeSpeed * Time.deltaTime;
            }

            //Else if targetDistance is less than 0
            else if (gun.targetDistance < 0)
            {
                //Make target Distance 0
                gun.targetDistance = 0;
            }

            //Else make target distance into maxDistance
            else
            {
                gun.targetDistance = maxDistance;
            }
        }
    }
}