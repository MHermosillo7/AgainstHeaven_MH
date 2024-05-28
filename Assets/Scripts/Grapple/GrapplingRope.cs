using UnityEngine;

namespace Heaven
{
    public class GrapplingRope : MonoBehaviour
    {
        [Header("General Refernces:")]
        public GrapplingGun grapplingGun;   //GrapplingGun script reference
        public LineRenderer lineRenderer;   //LineRenderer component reference

        [Header("General Settings:")]
        [SerializeField] private int precision = 40;    //Determines line precision

        //Speed line created changes (value between 0 and 20)
        [Range(0, 20)][SerializeField] private float straightenLineSpeed = 5;

        [Header("Rope Animation Settings:")]
        //Reference AnimationCurve of rope
        public AnimationCurve ropeAnimationCurve;    

        //Value between .01 and 4 representing starting wave size
        [Range(0.01f, 4)][SerializeField] private float StartWaveSize = 2;
        //Wave's size equals 0
        float waveSize = 0;

        [Header("Rope Progression:")]
        //Reference AnimationCurve of rope progression
        public AnimationCurve ropeProgressionCurve;    

        //Value between 1 and 50 for speed of rope progression
        [SerializeField][Range(1, 50)] private float ropeProgressionSpeed = 1;

        float moveTime = 0;                //Rate rope moves
        public float timeGrappling;        //Time grappled
        [SerializeField] bool startTimer;  //Whether start timer of time grappled
        public bool isGrappling = true;    //Whether player is grappling

        bool strightLine = true;           //Whether rope should be a straight line

        private void Awake()
        {
            //Get LineRenderer component
            //Find GrapplingGun script
            lineRenderer = GetComponent<LineRenderer>();
            grapplingGun = FindObjectOfType<GrapplingGun>();
        }
        private void OnEnable()
        {
            //Ropes rate of movement equals 0
            moveTime = 0;

            //LineRenderer's count position equals precision variable
            lineRenderer.positionCount = precision;

            //Wave size equals starting wave size
            waveSize = StartWaveSize;

            //Rope is not a straight line
            strightLine = false;

            //Cale LinePointsTOFirePOint method
            LinePointsToFirePoint();

            //Enable LineRenderer
            lineRenderer.enabled = true;
        }

        private void OnDisable()
        {
            //Disable LineRenderer
            lineRenderer.enabled = false;

            //Player is not grappling
            isGrappling = false;
        }

        public void LinePointsToFirePoint()
        {
            //Iterate through precision
            for (int i = 0; i < precision; i++)
            {
                //Set LineRenderer's position to fire point's position
                lineRenderer.SetPosition(i, grapplingGun.firePoint.position);
            }
        }

        private void Update()
        {
            //Add time passed to rope's rate of movement
            moveTime += Time.deltaTime;


            //Call for DrawRope method
            DrawRope();
        }

        public void DrawRope()
        {
            //If rope should not be straight
            if (!strightLine)
            {
                //If LineRenderer's one to last position
                //equals grappled point's x coordinate
                if (lineRenderer.GetPosition(precision - 1).x 
                    == grapplingGun.grapplePoint.x)
                {
                    //Rope should be straight
                    strightLine = true;
                }
                else
                {
                    //Call for DrawRopeWaves method
                    DrawRopeWaves();
                }
            }
            //Else rope should be straight
            else
            {
                //If player is not grappling
                if (!isGrappling)
                {
                    //Call GrapplingGun's Grapple method
                    grapplingGun.Grapple();

                    //Player is grappling
                    isGrappling = true;

                    //Start timer to record time grappled
                    startTimer = true;

                    //Call GrapplingTimer method
                    GrapplingTimer();
                }
                //If wave size is greater than 0
                if (waveSize > 0)
                {
                    //Wave size is decreased at a frame independant rate
                    //time speed to straighten line
                    waveSize -= Time.deltaTime * straightenLineSpeed;

                    //Call DrawRopeWaves method
                    DrawRopeWaves();
                }
                //Else wave size is less than or euqal to 0
                else
                {
                    //wave size equals 0
                    waveSize = 0;

                    //If LineRenderer's amount of positions is not equal to 2
                    //Its amount of positions equal 2
                    if (lineRenderer.positionCount != 2) 
                    { lineRenderer.positionCount = 2; }

                    //Call DrawRopeNoWaves method
                    DrawRopeNoWaves();
                }
            }
        }

        void DrawRopeWaves()
        {
            //Run this loop for [precision] number of times
            for (int i = 0; i < precision; i++)
            {
                //Create float to store number of loop divided by precision minus 1
                float delta = (float)i / ((float)precision - 1f);

                //Offset equals perpendicular vector to GrapplingGun's
                //distance vector direction times the evaluated RopeAnimationCurve
                //number relating to previous float times size of wave
                Vector2 offset = Vector2.Perpendicular(
                grapplingGun.grappleDistanceVector).normalized 
                * ropeAnimationCurve.Evaluate(delta) * waveSize;

                //Target position equals (interpolation between fire point position
                //and grappled point at the rate of the float atop the loop) 
                //plus the previous vector
                Vector2 targetPosition = Vector2.Lerp(
                grapplingGun.firePoint.position, grapplingGun.grapplePoint, 
                delta) + offset;

                //Current position equals interpolation between fire point's
                //position and target position at the rate of
                //(RopeProgressionCurve's value at moveTime)
                //times RopeProgressionSpeed
                Vector2 currentPosition = Vector2.Lerp(
                grapplingGun.firePoint.position, targetPosition, 
                ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);

                //Set LineRenderer's position equaling 
                //number of loop to current position
                lineRenderer.SetPosition(i, currentPosition);
            }
        }

        void DrawRopeNoWaves()
        {
            //LineRenderers first position equals firePoint's
            //And its second one equals grappled point's
            lineRenderer.SetPosition(0, grapplingGun.firePoint.position);
            lineRenderer.SetPosition(1, grapplingGun.grapplePoint);
        }
        void GrapplingTimer()
        {
            //If timer should be started and time grappled equals 0
            if (startTimer && timeGrappling == 0)
            {
                //Add time passed to time grappled
                timeGrappling += Time.deltaTime;
            }
            //Else time grappled is 0
            else timeGrappling = 0;
        }
    }
}
