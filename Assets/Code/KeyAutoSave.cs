using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyAutoSave : MonoBehaviour
{
    private bool saved = false;
    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (saved) return;

        if (collision.CompareTag("Player"))
        {
            saved = true;

            Transform player = collision.transform;

            GameData data = new GameData
            {
                playerX = player.position.x,
                playerY = player.position.y,
                playerZ = player.position.z
            };

            string levelName = SceneManager.GetActiveScene().name;
            int score = gameManager.score;

            string fileName = $"save_{levelName}_{score}";

            SaveSystem.Save(data, fileName);

            Debug.Log("AutoSave: " + fileName);
        }
    }
}
