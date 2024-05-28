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
            grappleRope = GetComponentInChildren<GrapplingRope>();

            //Find Camera object in scene
            camera = FindObjectOfType<Camera>();

            //Find ChangeGrappleMode script
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
                    //Fix gun pivot's rotation angle toward grappled point
                    RotateGun(grapplePoint, false);

                    //Deactivate aim game object
                    aim.SetActive(false);
                }

                //Else GrapplingRope is diabled
                else
                {
                    //Rotate gun pivot toward mouse' position
                    Vector2 mousePos = camera.ScreenToWorldPoint(
                        Input.mousePosition);
                    RotateGun(mousePos, true);
                }

                //If player should be launched to grappled point
                //And player is grappling
                if (launchToPoint && grappleRope.isGrappling)
                {
                    //If type of launch is of Transform system
                    if (launchType == LaunchType.Transform)
                    {
                        //Create new distance vector 2 that equals
                        //fire point position minus player's local position
                        Vector2 firePointDistnace =
                            firePoint.position - player.localPosition;

                        //Create new vector 2 to store target position
                        //equals grappled point minus previous distance vector
                        Vector2 targetPos = grapplePoint - firePointDistnace;

                        //Move Player's position through interpolations by
                        //moving from current position to target position
                        //at a frame independant rate times launch speed
                        player.position =
                            Vector2.Lerp(player.position, targetPos,
                            Time.deltaTime * launchSpeed);
                    }
                }
            }
            //Else if left or right click are no longer pressed
            //or all grapple related method should be stopped
            else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) 
                || resetGrapple)
            {
                resetGrapple = false;        //Not stop grapple methods anymore
                aim.SetActive(true);         //Activate aim
                grappleRope.enabled = false; //Disable GrapplingRope script
                joint2D.enabled = false;     //Disable SpringJoint2D component
                rb.gravityScale = 1;         //Reset player's gravity to 1

                //if there is a ControlObject script reference
                if (controller)
                {
                    //Call for reference's Deactivate coroutine
                    StartCoroutine(controller.Deactivate());
                    //Make controller reference null
                    controller = null;
                }
                //If there is no ControlObject script reference
                //It is null
                else controller = null;

            }
            //If no button has been clicked
            else
            {
                //Rotate gun pivot toward mouse' position
                Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }
        }

        void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
        {
            //Create Vector of distance between point looked and gun pivot position
            Vector3 distanceVector = lookPoint - gunPivot.position;

            //Create float to store angle of distance vector 
            //after converted to radians and back to degrees
            float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * 
                Mathf.Rad2Deg;

            //If gun pivot should rotate over time
            if (rotateOverTime && allowRotationOverTime)
            {
                //Rotate gun pivot by Quaternion interpolation 
                //between its rotation and that of angle degrees in the forward axis
                //at a frame independant rate times speed of rotation
                gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation,
                    Quaternion.AngleAxis(angle, Vector3.forward),
                    Time.deltaTime * rotationSpeed);
            }
            //If gun pivot should not be rotated
            else
            {
                //Rotate gun pivot by angle degrees in the forward axis
                gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        void SetGrapplePoint()
        {
            //Distance between mouse and gun pivot
            Vector2 distanceVector = 
                camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;

            //FIre a raycast from fire point to the direction of distance vector
            if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
            {
                //Instantiate a RaycastHit with previous values in if statement
                RaycastHit2D _hit = Physics2D.
                    Raycast(firePoint.position, distanceVector.normalized);

                //If object hit's layer number equals the grappable layer number
                //or player can grapple to all layer objects
                if (_hit.transform.gameObject.layer == grappableLayerNumber 
                    || grappleToAll)
                {
                    //If distance between object hit and fire point is
                    //less than or equal to max distance or no maximum distance
                    if (Vector2.Distance(_hit.point, firePoint.position) 
                        <= maxDistance || !hasMaxDistance)
                    {
                        //Get reference of hit object's ControlObject script
                        controller = _hit.transform.gameObject.
                        GetComponent<ControlObject>();

                        //grappled point equals hit point
                        grapplePoint = _hit.point;

                        //Grappled Distance Vector equals grappled point minus
                        //gun pivot's position
                        grappleDistanceVector =
                            grapplePoint - (Vector2)gunPivot.position;

                        //Enable GrapplingRope script
                        grappleRope.enabled = true;
                    }
                }
            }
        }

        public void Grapple()
        {
            //Disable SpringJoint2D's from auto configuring the distance
            joint2D.autoConfigureDistance = false;

            //If player should not be moved to grappled point
            //or distance between the two should not be auto configured
            if (!launchToPoint && !autoConfigureDistance)
            {
                //SpringJoing2D's distance equals target distance
                //And its frequency equals target frequency
                joint2D.distance = targetDistance;
                joint2D.frequency = targetFrequency;
            }

            //If player should not be moved to grappled point
            if (!launchToPoint)
            {
                //If distance between the two should be controlled
                if (autoConfigureDistance)
                {
                    //Make SpringJoint2D auto configure the distance
                    //And change its frequency to 0
                    joint2D.autoConfigureDistance = true;
                    joint2D.frequency = 0;
                }

                //SpringJoint2D's connected anchor equals grappled point
                //And enable SpringJoint2D component
                joint2D.connectedAnchor = grapplePoint;
                joint2D.enabled = true;
            }
            //Else player should not be laucnhed to grappled point
            else
            {    
                //Switch launch system in place
                switch (launchType)
                {
                    //In case system is type Physics
                    case LaunchType.Physics:

                        //SpringJoint2D's anchor equals grappled point
                        joint2D.connectedAnchor = grapplePoint;

                        //Distance Vector equals
                        //fire point position minus player's
                        Vector2 distanceVector = 
                            firePoint.position - player.position;

                        //SpringJoint2D's distance equals
                        //magnitude of previous vector
                        //It frequency equals speed of launch
                        //And enable SpringJoint2D component
                        joint2D.distance = distanceVector.magnitude;
                        joint2D.frequency = launchSpeed;
                        joint2D.enabled = true;
                        break;
                        
                    //In case system is type Transform
                    case LaunchType.Transform:
                        rb.gravityScale = 0;        //Player's gravity equals 0
                        rb.velocity = Vector2.zero; //Player's velocity equals 0
                        break;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            //If there is a fire point and max distance
            if (firePoint != null && hasMaxDistance)
            {
                //Draw a green wire sphere around fire point of 
                //with a radius equaling max distance
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(firePoint.position, maxDistance);
            }
        }

    }
}
