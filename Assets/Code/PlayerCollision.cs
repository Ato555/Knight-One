using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController player;
    private AudioManager audioManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        player = GetComponent<PlayerController>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            gameManager.AddScore(1);
            audioManager.PlayCoinSound();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Trap"))
        {
            player.TakeDamage(player.health);
        }

        if (other.CompareTag("Key"))
        {
            gameManager.GameWin();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Heath"))
        {
            player.Heal(5);
            Destroy(other.gameObject);
        }
    }
}
