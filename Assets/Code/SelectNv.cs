using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectNv : MonoBehaviour
{
    public void SelectHero()
    {
        PlayerPrefs.SetString("Player", "Hero");
        string level = PlayerPrefs.GetString("Level01");
        SceneManager.LoadScene(level);
    }

    public void SelectKnight()
    {
        PlayerPrefs.SetString("Player", "Knight");
        string level = PlayerPrefs.GetString("Level01");
        SceneManager.LoadScene(level);
    }
}
