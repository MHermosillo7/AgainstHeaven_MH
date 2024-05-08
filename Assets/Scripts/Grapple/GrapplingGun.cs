using UnityEngine;

namespace Heaven
{
    public class GrapplingGun : MonoBehaviour
    {
        [Header("Scripts Ref:")]
        public GrapplingRope grappleRope;
        ControlObject controller;

        [Header("LayerSettings:")]
        [SerializeField] private bool grappleToAll = false;
        [SerializeField] private int grappableLayerNumber = 9;

        [Header("Main Camera:")]
        public Camera camera;

        [Header("Transform Ref:")]
        public Transform player;
        public Transform gunPivot;
        public Transform firePoint;

        [Header("Physics Ref:")]
        public SpringJoint2D joint2D;
        public Rigidbody2D rb;

        [Header("Rotation:")]
        [SerializeField] private bool rotateOverTime = true;
        [Range(0, 60)][SerializeField] private float rotationSpeed = 4;

        [Header("Distance:")]
        [SerializeField] private bool hasMaxDistance = false;
        [SerializeField] private float maxDistance = 20;

        [Header("GameObject:")]
        public GameObject aim;
        public enum LaunchType
        {
            Transform,
            Physics
        }

        [Header("Launching:")]
        [SerializeField] public bool launchToPoint = true;
        [SerializeField] public LaunchType launchType = LaunchType.Physics;
        [SerializeField] private float launchSpeed = 1;

        [Header("No Launch To Point")]
        [SerializeField] public bool autoConfigureDistance = false;
        [SerializeField] public float targetDistance = 3;
        [SerializeField] private float targetFrequency = 1;

        [HideInInspector] public Vector2 grapplePoint;
        [HideInInspector] public Vector2 grappleDistanceVector;

        private void Awake()
        {
            grappleRope = GetComponentInChildren<GrapplingRope>();
            camera = FindObjectOfType<Camera>();

            autoConfigureDistance = true;
            grappleRope.enabled = false;
            joint2D.enabled = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                SetGrapplePoint();
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                if (grappleRope.enabled)
                {
                    RotateGun(grapplePoint, false);
                    aim.SetActive(false);
                }
                else
                {
                    Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
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
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                aim.SetActive(true);
                grappleRope.enabled = false;
                joint2D.enabled = false;
                rb.gravityScale = 1;

                if (controller && grappleRope.timeGrappling >= 1f)
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
            Vector2 distanceVector = camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;

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