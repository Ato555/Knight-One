using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    private AudioManager audioManager;
    private PlayerController playerController;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject pauseGame;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject gameWin;
    
    private bool isPaused = false;
    private bool isOver = false;
    private bool isWin = false;

    private void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        playerController = FindAnyObjectByType<PlayerController>();
    }
    void Start()
    {
        UpdateScore();
        UpdateHealth();
        pauseGame.SetActive(false);
        gameOver.SetActive(false);
        gameWin.SetActive(false);
    }

    void Update()
    {
        
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.text = score.ToString();
    }

    public void UpdateHealth()
    {
        healthText.text = "HP:" + playerController.health.ToString();
    }

    public void PauseGame()
    {
        pauseGame.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        audioManager.PauseBackgroundMusic();
    }

    public void ResumeGame()
    {
        pauseGame.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        audioManager.UnPauseBackgroundMusic();
    }

    public void GameOver()
    {
        isOver = true;
        score = 0;
        Time.timeScale = 0f;
        gameOver.SetActive(true);
        audioManager.PauseBackgroundMusic();
        audioManager.PlayLoseSound();
    }

    public void RestartGame()
    {
        isOver = false;
        score = 0;
        UpdateScore();
        Time.timeScale = 1f;

        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    public void GameWin()
    {
        isWin = true;
        Time.timeScale = 0f;
        gameWin.SetActive(true);
        audioManager.PauseBackgroundMusic();
        audioManager.PlayWinSound();
    }

    public void GotoMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public bool IsGameOver()
    {
        return isOver;
    }

    public bool IsGameWin()
    {
        return isWin;
    }
}
