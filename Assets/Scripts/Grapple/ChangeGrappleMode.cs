using UnityEngine;

namespace Heaven
{
    public class ChangeGrappleMode : MonoBehaviour
    {
        [SerializeField] GrapplingGun gun;
        [SerializeField] Animator animator;
        // Start is called before the first frame update
        void Awake()
        {
            gun = FindObjectOfType<GrapplingGun>();
        }

        public void ChangeMode()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (gun.launchType == GrapplingGun.LaunchType.Physics)
                {
                    gun.launchType = GrapplingGun.LaunchType.Transform;
                    gun.launchToPoint = false;
                    gun.autoConfigureDistance = false;
                    gun.maxDistance = 7;
                    animator.SetTrigger("ChangeMode");
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                if(gun.launchType == GrapplingGun.LaunchType.Transform)
                {
                    gun.launchType = GrapplingGun.LaunchType.Physics;
                    gun.launchToPoint = true;
                    gun.maxDistance = 10;
                    animator.SetTrigger("ChangeMode");
                }
            }
            
          
        }
    }
}