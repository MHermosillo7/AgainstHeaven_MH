using UnityEngine;

namespace Heaven
{
    public class ChangeRopeDistance : MonoBehaviour
    {
        GrapplingGun gun;
        GrapplingRope rope;
        [SerializeField] float changeSpeed = 1f;
        [SerializeField] float maxDistance = 5f;
        private float input;

        // Start is called before the first frame update
        void Start()
        {
            gun = GetComponentInChildren<GrapplingGun>();
            rope = GetComponentInChildren<GrapplingRope>();
        }

        // Update is called once per frame
        void Update()
        {
            if (rope.isGrappling && gun.launchType == GrapplingGun.LaunchType.Transform)
            {
                ChangeLength();
                gun.joint2D.distance = gun.targetDistance;
            }
        }

        void ChangeLength()
        {
            input = Input.GetAxisRaw("Vertical");
            if (gun.targetDistance >= 0 && gun.targetDistance <= maxDistance)
            {
                gun.targetDistance += input * changeSpeed * Time.deltaTime;
            }
            else if (gun.targetDistance < 0)
            {
                gun.targetDistance = 0;
            }
            else
            {
                gun.targetDistance = maxDistance;
            }
        }
    }
}