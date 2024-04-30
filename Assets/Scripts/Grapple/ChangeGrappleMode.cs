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

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ChangeMode();
                animator.SetTrigger("ChangeMode");
            }
        }

        void ChangeMode()
        {
            if (gun.launchType == GrapplingGun.LaunchType.Physics)
            {
                gun.launchType = GrapplingGun.LaunchType.Transform;
                gun.launchToPoint = false;
                gun.autoConfigureDistance = false;
            }
            else
            {
                gun.launchType = GrapplingGun.LaunchType.Physics;
                gun.launchToPoint = true;
            }
          
        }
    }
}