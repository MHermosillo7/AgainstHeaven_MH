using UnityEngine;

namespace Heaven
{
    [RequireComponent(typeof(Collider2D))]
    public class EndCutscene : MonoBehaviour
    {
        Collider2D collider;
        [SerializeField] Transform targetPosition;
        [SerializeField] GameObject[] playerObjects;

        [Header("Player:")]
        GameObject player;
        Rigidbody2D playerRB;
        Player playerScript;

        public bool cutscene;
        private float moveSpeed;

        private void Awake()
        {
            collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
            player = GameObject.FindGameObjectWithTag("Player");
            playerRB = player.GetComponent<Rigidbody2D>();
            playerScript = player.GetComponent<Player>();

            moveSpeed = playerScript.moveSpeed;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                cutscene = true;
                playerRB.velocity = new Vector2 (0,0);

                player.GetComponent<Joint2D>().enabled = false;
                for (int i = 0; i < playerObjects.Length; i++)
                {
                    playerObjects[i].SetActive(false);
                }
                MovePlayer();
            }
        }
        //Modified copy from player script
        private void MovePlayer()
        {
            while (player.transform.position.x != targetPosition.position.x)
            {
                playerRB.velocity =
                (new Vector2((Vector2.right * moveSpeed).x * moveSpeed, playerRB.velocity.y));
            }

        }
    }   
}

