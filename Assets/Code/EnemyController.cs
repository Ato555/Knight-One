using UnityEngine;
using UnityEngine.InputSystem.XInput;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private LayerMask checkEnemy;
    [SerializeField] private Transform groundCheck; // kiem tra mat dat
    [SerializeField] private Transform wallCheck; // kiem tra tuong
    [SerializeField] private float groundCheckDistance = 1f; // khoang cach kiem tra mat dat
    [SerializeField] private float wallCheckDistance = 1f; // khoang cach kiem tra tuong

    [SerializeField] private Transform player; // tham chieu den player
    [SerializeField] private float chaseRange = 3f; // khoang cach de phat hien player
    [SerializeField] private float attackRange = 2f; // khoang cach de tan cong player

    private Animator animator;
    private Rigidbody2D rb;

    private bool movingRight = true;
    private bool isGroundAhead;
    private bool isWallAhead;

    public enum EnemyType
    {
        Boar,
        SmallBee
    }
    [SerializeField] private EnemyType enemyType;

    private enum EnemyState 
    { 
        Patrol, Chase, Attack
    }
    private EnemyState currentState = EnemyState.Patrol;

    public float knockbackForce;
    public float knockbackCouter;
    public float knockbackTotalTime;

    public bool isKnockback;

    public int health;
    public int maxHealth = 10;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        if (isKnockback)
        {
            knockbackCouter -= Time.deltaTime;
            if (knockbackCouter <= 0)
            {
                isKnockback = false;
            }
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else if (distanceToPlayer <= chaseRange)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }

        switch(currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    private void Patrol() // di chuyen ke dich
    {
        if (groundCheck != null)
        {
            isGroundAhead = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, checkEnemy); // kiem tra mat dat
        }

        Vector2 wallDir = movingRight ? Vector2.right : Vector2.left;
        isWallAhead = Physics2D.Raycast(wallCheck.position, wallDir, wallCheckDistance, checkEnemy);// kiem tra tuong

        if (enemyType == EnemyType.Boar)
        {
            if (!isGroundAhead || isWallAhead)
            {
                Flip();
            }

            float moveDir = movingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(moveDir * speed, rb.linearVelocity.y);
            animator.SetBool("isWalk", true);
            animator.SetBool("isRun", false);
        }
        else if (enemyType == EnemyType.SmallBee)
        {
            if (isWallAhead)
            {
                Flip();
            }
            float moveDir = movingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(moveDir * speed, rb.linearVelocity.y);
        }
    }

    private void Chase()
    {
        if (player.position.x > transform.position.x && !movingRight)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && movingRight)
        {
            Flip();
        }

        float moveDir = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(moveDir * speed, rb.linearVelocity.y);

        if (enemyType == EnemyType.Boar)
        {
            rb.linearVelocity = new Vector2(moveDir * (speed * 2f), rb.linearVelocity.y);
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", true);
        }
    }

    private void Attack()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        if (enemyType == EnemyType.SmallBee)
        {
            animator.SetTrigger("Attack");
        }
    }

    public void StartKnockback()
    {
        isKnockback = true;
        knockbackCouter = knockbackTotalTime;
        currentState = EnemyState.Patrol;
        float direction = (player.position.x > transform.position.x) ? -1f : 1f;
        rb.linearVelocity = new Vector2(direction * knockbackForce, rb.linearVelocity.y);
        if (enemyType == EnemyType.Boar)
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", false);
        }
    }

    public void TakeDamage(int damage, Transform playerTransform = null)
    {
        animator.SetTrigger("Hit");
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        rb.linearVelocity = Vector2.zero;
        enabled = false;
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null) col.enabled = false;
        Destroy(gameObject);
    }

    private void Flip() // dao huong di chuyen
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected() // ve duong raycast trong editor
    {
        if (groundCheck != null) // kiem tra mat dat
        {
            //Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }

        if (wallCheck != null) // kiem tra tuong
        {
            //Gizmos.color = Color.blue;
            Vector3 wall = movingRight ? Vector3.right : Vector3.left;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + wall * wallCheckDistance);
        }

        //Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        //Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
