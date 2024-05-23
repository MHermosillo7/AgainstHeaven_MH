using UnityEngine;

namespace Heaven
{
    public class ExecuteNextLevel : MonoBehaviour
    {
        //Call Game Manager's NextLevel function
        void Execute()
        {
            GameManager.NextLevel();
        }
    }
}