using UnityEngine;

namespace Heaven
{
    public class ChangeGrappleMode : MonoBehaviour
    {
        [SerializeField] GrapplingGun gun;      //GrapplingGun script reference
        [SerializeField] Animator animator;     //Animator component reference

        // Start is called before the first frame update
        void Awake()
        {
            gun = FindObjectOfType<GrapplingGun>(); //Find GrapplingGun script
        }

        public void ChangeMode()
        {
            //If the left mouse button is pressed
            if (Input.GetMouseButtonDown(0))
            {
                //If GraapplingGun's launch type is of state Physics
                if (gun.launchType == GrapplingGun.LaunchType.Physics)
                {
                    //Change launchType to state Transform
                    gun.launchType = GrapplingGun.LaunchType.Transform;
                    
                    //Change launchToPoint variable to false
                    gun.launchToPoint = false;
                    
                    //Change autoConfigureDistance to false
                    gun.autoConfigureDistance = false;
                    
                    //Change maxDistance to 7
                    gun.maxDistance = 7;
                    
                    //Set animator's trigger "ChangeMode"
                    animator.SetTrigger("ChangeMode");
                }
            }
            //If the right mouse button is pressed
            if (Input.GetMouseButtonDown(1))
            {
                //If GrapplingGun's launch type is of state Transform
                if(gun.launchType == GrapplingGun.LaunchType.Transform)
                {
                    //Change launchType to state Physics
                    gun.launchType = GrapplingGun.LaunchType.Physics;

                    //Change launchToPoint to true
                    gun.launchToPoint = true;

                    //Change autoConfigureDistance to true
                    gun.autoConfigureDistance = true;

                    //Change maxDistance to 10
                    gun.maxDistance = 10;

                    //Set animator's trigger "ChangeMode"
                    animator.SetTrigger("ChangeMode");
                }
            }
            
          
        }
    }
}