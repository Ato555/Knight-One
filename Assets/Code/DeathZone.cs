using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.CompareTag("Player"))
        {
            GameManager gameManager = FindAnyObjectByType<GameManager>();
            gameManager.GameOver();
        }
    }
}
