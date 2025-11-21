using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerController player;
    public int damage = 2;
    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (player.isAttacking)
            { 
                enemy.TakeDamage(damage);
                enemy.StartKnockback();
            }
        }
    }

}
