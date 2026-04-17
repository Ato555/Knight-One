using UnityEngine;
using UnityEngine.InputSystem.XInput;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck; // kiem tra mat dat
    [SerializeField] private Transform wallCheck; // kiem tra tuong
    [SerializeField] private float groundCheckDistance = 1f; // khoang cach kiem tra mat dat
    [SerializeField] private float wallCheckDistance = 1f; // khoang cach kiem tra tuong

    //[SerializeField] private Transform player;
    private PlayerController player; // tham chieu den player
    [SerializeField] private float chaseRange = 3f; // khoang cach de phat hien player
    [SerializeField] private float attackRange = 2f; // khoang cach de tan cong player

    private Animator animator;
    private Rigidbody2D rb;
    private AudioManager audioManager;
    private GameManager gameManager;

    private bool movingRight = true;
    private bool isGroundAhead;
    private bool isWallAhead;

    private float flipCooldown = 0.2f;
    private float flipTimer = 0f;

    public enum EnemyType
    {
        Boar,
        SmallBee,
        Slime
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
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        FindNearestPlayer();

        if (isKnockback)
        {
            knockbackCouter -= Time.deltaTime;
            if (knockbackCouter <= 0)
            {
                isKnockback = false;
            }
            return;
        }

        CheckPlayerDistance();

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            //case EnemyState.Attack:
            //    Attack();
            //    break;
        }

        flipTimer -= Time.deltaTime;
    }

    private void FindNearestPlayer()
    {
        PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        float minDistance = Mathf.Infinity;
        PlayerController nearest = null;

        foreach (var p in players)
        {
            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = p;
            }
        }

        player = nearest;
    }


    private void Patrol() // di chuyen ke dich
    {
        if (groundCheck != null)
        {
            isGroundAhead = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer); // kiem tra mat dat
        }

        Vector2 wallDir = movingRight ? Vector2.right : Vector2.left;
        isWallAhead = Physics2D.Raycast(wallCheck.position, wallDir, wallCheckDistance, groundLayer);// kiem tra tuong

        if (enemyType == EnemyType.Boar)
        {
            if (!isGroundAhead || isWallAhead)
            {
                if (flipTimer <= 0)
                {
                    Flip();
                    flipTimer = flipCooldown; // ngăn flip liên tục
                }
            }

            float moveDir = movingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(moveDir * speed, rb.linearVelocity.y);
            animator.SetBool("isWalk", true);
            animator.SetBool("isRun", false);
        }
        else if (enemyType == EnemyType.Slime)
        {
            if (!isGroundAhead || isWallAhead)
            {
                if (flipTimer <= 0)
                {
                    Flip();
                    flipTimer = flipCooldown; // ngăn flip liên tục
                }
            }
            float moveDir = movingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(moveDir * speed, rb.linearVelocity.y);
        }
    }

    private void Chase()
    {
        if (player.transform.position.x > transform.position.x && !movingRight) 
        {
            Flip();
            audioManager.PlayEnemyAttackSound();
        }
        else if (player.transform.position.x < transform.position.x && movingRight)
        {
            Flip();
            audioManager.PlayEnemyAttackSound();
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

    //private void Attack()
    //{
    //    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    //    if (enemyType == EnemyType.SmallBee)
    //    {
    //        animator.SetTrigger("Attack");
    //    }
    //}

    private void CheckPlayerDistance()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

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
            // Nếu đang chase mà mất player => quay trở về Patrol
            if (currentState == EnemyState.Chase || currentState == EnemyState.Attack)
            {
                currentState = EnemyState.Patrol;
                Flip(); // Đổi hướng đi khi không thấy player
            }
        }
    }


    public void StartKnockback()
    {
        isKnockback = true;
        knockbackCouter = knockbackTotalTime;
        currentState = EnemyState.Patrol;
        float direction = (player.transform.position.x > transform.position.x) ? -1f : 1f;
        rb.linearVelocity = new Vector2(direction * knockbackForce, rb.linearVelocity.y);
        if (enemyType == EnemyType.Boar)
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", false);
        }
    }

    public void TakeDamage(int damage, Transform playerTransform = null)
    {
        if (enemyType == EnemyType.Boar)
        {
            animator.SetTrigger("Hit");
        }
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (enemyType == EnemyType.Boar)
        {
            animator.SetTrigger("Hit");
            audioManager.PlayEnemyDeathSound();
        }
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        this.enabled = false;

        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null) col.enabled = false;
        gameManager.AddScore(5);

        StartCoroutine(EnemyDeathRoutine());
    }

    private IEnumerator EnemyDeathRoutine()
    {
        yield return new WaitForSeconds(0.5f);
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
