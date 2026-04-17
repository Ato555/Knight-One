using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLv : MonoBehaviour
{
    public void StartGame(string levelName)
    {
        PlayerPrefs.SetString("Level01", levelName);

        SceneManager.LoadScene("SelectNv");
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Start");
    }
}
