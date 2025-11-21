using UnityEngine;
using static EnemyController;

public class EnemyAttackZone : MonoBehaviour
{
    private EnemyController enemy;
    public int damage = 2;
    private void Awake()
    {
        enemy = GetComponentInParent<EnemyController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                bool playerFacingRight = player.transform.localScale.x > 0;
                bool enemyIsOnRightSide = transform.position.x > player.transform.position.x;
                bool isBlocking = (playerFacingRight && enemyIsOnRightSide) || (!playerFacingRight && !enemyIsOnRightSide);
                if (player.isDefending && isBlocking)
                {
                    enemy.StartKnockback();
                    return;
                }
                player.knockbackCouter = player.knockbackTotalTime;
                player.isKnockback = (other.transform.position.x < transform.position.x);

                player.TakeDamage(damage, transform.parent);
            }
        }
    }
}
