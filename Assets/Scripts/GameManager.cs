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
    public static void NextLevel()
    {
        //Get index number of next scene
        int buildIndex = SceneManager.GetActiveScene().buildIndex + 1;

        //If there is another scene after this one
        if(buildIndex < SceneManager.sceneCountInBuildSettings)
        {
            //Load Scene corresponding to next in index nummber
            //But keep current scene playing while next one is loading
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }

        //Else, load title screen
        else
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}
