using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public, non-changeable GameManager instance
    public static GameManager instance;

    private void Awake()
    {
        //If there is a Game Manager instance already
        if (instance)
        {
            //Delete this game object
            Destroy(this);
        }
        //Else this is only Game Manager instance
        else
        {
            //This is the Game Manager instance
            instance = this;
            //Dont Destroy this game object when
            //loading another scene
            DontDestroyOnLoad(this);
        }
    }
    //Load scene through name according to input string
    public static void LoadScene(string newSceneName)
    {
        SceneManager.LoadScene(newSceneName);
    }
    //Load Scene corresponding to next in index nummber
    //But keep current scene playing while next one is loading
    public static void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
