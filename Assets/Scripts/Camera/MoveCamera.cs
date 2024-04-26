using UnityEngine;

namespace Heaven
{
    public class MoveCamera : MonoBehaviour
    {
        Camera camera;
        Player player;

        public bool focusCamera;
        public bool moveToTarget;
        public Transform targetPosition;
        // Start is called before the first frame update
        void Start()
        {
            camera = GetComponent<Camera>();
            player = FindObjectOfType<Player>();
        }

        // Update is called once per frame
        void Update()
        {
            if (focusCamera)
            {
                camera.transform.position = player.transform.position;
            }
            else if(targetPosition != null)
            {
                camera.transform.position += targetPosition.position;
            }
        }
    }
}