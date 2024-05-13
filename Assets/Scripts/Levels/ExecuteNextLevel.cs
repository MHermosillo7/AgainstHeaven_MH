using UnityEngine;

namespace Heaven
{
    public class ExecuteNextLevel : MonoBehaviour
    {
        void Execute()
        {
            GameManager.NextLevel();
        }
    }
}