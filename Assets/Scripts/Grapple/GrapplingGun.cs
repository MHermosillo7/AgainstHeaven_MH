using UnityEngine;

namespace Heaven
{
    public class GrapplingGun : MonoBehaviour
    {
        [Header("Scripts Ref:")]
        public GrapplingRope grappleRope;   //GrappleRope script reference
        ControlObject controller;           //ControlObject script reference
        ChangeGrappleMode changeMode;       //ChangeGrappleMode script reference

        [Header("LayerSettings:")]
        //Control whether to ignore objects layer number
        [SerializeField] private bool grappleToAll = false;

        //Controls what specific layer's objects to grapple only
        [SerializeField] private int grappableLayerNumber = 9;

        [Header("Main Camera:")]
        public Camera camera;   //Camera reference

        [Header("Transform Ref:")]
        public Transform player;        //Player Transform reference
        public Transform gunPivot;      //GunPivot Transform reference
        public Transform firePoint;     //FirePoint Transform reference

        [Header("Physics Ref:")]
        public SpringJoint2D joint2D;   //Player's SpringJoint2D reference
        public Rigidbody2D rb;          //Player's Rigidbody reference

        [Header("Rotation:")]
        //Whether to rotate component or not
        [SerializeField] private bool rotateOverTime = true;
        //Speed of rotation of value between 0 and 60
        [Range(0, 60)][SerializeField] private float rotationSpeed = 4;

        [Header("Distance:")]
        //Whether there is max Distance to grapple to
        [SerializeField] private bool hasMaxDistance = false;
        //Maximum distance to grapple to objects
        [SerializeField] public float maxDistance = 20;

        [Header("GameObject:")]
        public GameObject aim;  //Aim Component reference

        //Controls whether grapple using Physics or Transform systems
        public enum LaunchType
        {
            Transform,
            Physics
        }

        [Header("Launching:")]
        //Whether move player to grappled object
        [SerializeField] public bool launchToPoint = true;

        //Variable to store type of system used for moving player
        [SerializeField] public LaunchType launchType = LaunchType.Physics;

        //Speed to move player when grappling
        [SerializeField] private float launchSpeed = 1;

        [Header("No Launch To Point")]
        //Whether control exact distance between object grappled and player
        [SerializeField] public bool autoConfigureDistance = false;

        //Distance between grappled object and player
        [SerializeField] public float targetDistance = 3;

        //Firing Frequency
        [SerializeField] private float targetFrequency = 1;

        //Point where object was grappled
        [HideInInspector] public Vector2 grapplePoint;

        //Store distance between object hit and player
        [HideInInspector] public Vector2 grappleDistanceVector;

        [Header("Variables")]
        //Control whether stop all grapple related methods running
        public bool resetGrapple;

        private void Awake()
        {
            //Get GrapplingRope script in children
            //Find Camera object in scene
            //Find ChangeGrappleMode script
            grappleRope = GetComponentInChildren<GrapplingRope>();
            camera = FindObjectOfType<Camera>();
            changeMode = FindObjectOfType<ChangeGrappleMode>();

            autoConfigureDistance = true;   //Enable automatic configured distance
            grappleRope.enabled = false;    //Disable GrapplingRope script
            joint2D.enabled = false;        //Disable SpringJoint2D component
        }
        //When script is enabled
        private void OnEnable()
        {
            grappleRope.enabled = false;    //Disable GrapplingRope script 
            joint2D.enabled = false;        //Disable SpringJoint2D component
        }
        private void Update()
        {
            //If mouse right-click or lefft-click ar pressed
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                changeMode.ChangeMode();    //Call for ChangeMode method
                SetGrapplePoint();          //Call SetGrapplePOint method
            }
            //Else if right-click or left-click are held down and
            //resetGrapple is false
            else if (!resetGrapple && Input.GetMouseButton(0) 
                || !resetGrapple && Input.GetMouseButton(1))
            {
                //If GrapplingRope is enabled
                if (grappleRope.enabled)
                {
                    RotateGun(grapplePoint, false);
                    aim.SetActive(false);
                }
                else
                {
                    Vector2 mousePos = camera.ScreenToWorldPoint(
                        Input.mousePosition);
                    RotateGun(mousePos, true);
                }

                if (launchToPoint && grappleRope.isGrappling)
                {
                    if (launchType == LaunchType.Transform)
                    {
                        Vector2 firePointDistnace =
                            firePoint.position - player.localPosition;

                        Vector2 targetPos = grapplePoint - firePointDistnace;
                        player.position =
                            Vector2.Lerp(player.position, targetPos,
                            Time.deltaTime * launchSpeed);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) 
                || resetGrapple)
            {
                resetGrapple = false;
                aim.SetActive(true);
                grappleRope.enabled = false;
                joint2D.enabled = false;
                rb.gravityScale = 1;

                if (controller)
                {
                    StartCoroutine(controller.Deactivate());
                    controller = null;
                }
                else controller = null;

            }
            else
            {
                Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }
        }

        void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
        {
            Vector3 distanceVector = lookPoint - gunPivot.position;

            float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;

            if (rotateOverTime && allowRotationOverTime)
            {
                gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation,
                    Quaternion.AngleAxis(angle, Vector3.forward),
                    Time.deltaTime * rotationSpeed);
            }
            else
            {
                gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        void SetGrapplePoint()
        {
            Vector2 distanceVector = 
                camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;

            if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
            {
                RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
                if (_hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
                {
                    if (Vector2.Distance(_hit.point, firePoint.position) <= maxDistance || !hasMaxDistance)
                    {
                        controller = _hit.transform.gameObject.
                        GetComponent<ControlObject>();

                        grapplePoint = _hit.point;
                        grappleDistanceVector =
                            grapplePoint - (Vector2)gunPivot.position;
                        grappleRope.enabled = true;
                    }
                }
            }
        }

        public void Grapple()
        {
            joint2D.autoConfigureDistance = false;

            if (!launchToPoint && !autoConfigureDistance)
            {
                joint2D.distance = targetDistance;
                joint2D.frequency = targetFrequency;
            }

            if (!launchToPoint)
            {
                if (autoConfigureDistance)
                {
                    joint2D.autoConfigureDistance = true;
                    joint2D.frequency = 0;
                }

                joint2D.connectedAnchor = grapplePoint;
                joint2D.enabled = true;
            }
            else
            {
                switch (launchType)
                {
                    case LaunchType.Physics:
                        joint2D.connectedAnchor = grapplePoint;

                        Vector2 distanceVector = firePoint.position - player.position;

                        joint2D.distance = distanceVector.magnitude;
                        joint2D.frequency = launchSpeed;
                        joint2D.enabled = true;
                        break;

                    case LaunchType.Transform:
                        rb.gravityScale = 0;
                        rb.velocity = Vector2.zero;
                        break;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (firePoint != null && hasMaxDistance)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(firePoint.position, maxDistance);
            }
        }

    }
}