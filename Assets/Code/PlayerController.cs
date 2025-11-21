using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    private Animator animator;

    private bool isGrounded;
    private bool isRunning;
    public bool isAttacking;
    public bool isDefending;

    private int comboStep = 0;
    private float comboTimer = 0f;
    private float comboResetTime = 0.8f;

    public int health;
    public int maxHealth = 10;

    public float knockbackForce;
    public float knockbackCouter;
    public float knockbackTotalTime;

    public bool isKnockback;

    private Rigidbody2D rb;
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

    }
    void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
        HandLeRun();
        HandleComBoAttackNormal();
        HandleDefend();
        UpdateAnimation();
        if (comboStep > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= comboResetTime)
            {
                ResetCombo();
            }
        }

        if (knockbackCouter > 0)
        {
            HandleKnockback();
            knockbackCouter -= Time.deltaTime;
            if (knockbackCouter <= 0)
            {
                isKnockback = false;
            }
            return;
        }
    }
    private void HandleMovement()
    {
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.RightArrow) && !isAttacking && !isDefending)
        {
            moveInput = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !isAttacking && !isDefending)
        {
            moveInput = -1f;
        }

        if (isAttacking || isDefending)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // dung lai khi tan cong hoac phong thu
            return;
        }

        float currentSpeed = isRunning ? runSpeed : speed;

        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isAttacking && !isDefending)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void HandLeRun()
    {
        float moveInput = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(moveInput) > 0.1f && !isAttacking && !isDefending) {
            rb.linearVelocity = new Vector2(moveInput * runSpeed, rb.linearVelocity.y);
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void HandleComBoAttackNormal()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isAttacking = true;
            comboStep++;
            comboTimer = 0f;

            if(comboStep > 3)
            {
                comboStep = 1;
            }

            switch (comboStep)
            {
                case 1:
                    animator.SetTrigger("Attack1");
                    break;
                case 2:
                    animator.SetTrigger("Attack2");
                    break;
                case 3:
                    animator.SetTrigger("Attack3");
                    ResetCombo();
                    break;
            }
        }
    }

    private void HandleDefend()
    {
        if (Input.GetKey(KeyCode.S))
        {
            isDefending = true;
            animator.SetBool("isDefend", true);
        }
        else
        {
            isDefending = false;
            animator.SetBool("isDefend", false);
        }
    }

    private void ResetCombo()
    {
        comboStep = 0;
        comboTimer = 0f;
        isAttacking = false;
    }

    public void TakeDamage(int amount, Transform enemyTransform = null)
    {
        animator.SetTrigger("Hurt");
        health -= amount;

        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    private void Die()
    {
        animator.SetBool("isDeath", true);

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        this.enabled = false;

        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null) col.enabled = false;

        StartCoroutine(PlayerDeathRoutine());
    }

    private IEnumerator PlayerDeathRoutine()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f; // Tạm dừng trò chơi

        // (Tùy chọn) Hiện menu Game Over
        // gameOverUI.SetActive(true);
    }

    private void HandleKnockback()
    {
        float direction = isKnockback ? -1f : 1f;
        rb.linearVelocity = new Vector2(direction * knockbackForce, rb.linearVelocity.y);
    }

    private void UpdateAnimation()
    {
        bool isWalking = Mathf.Abs(rb.linearVelocity.x) > 0.1f && !isRunning;
        bool isJumping = !isGrounded;
        animator.SetBool("isWalk", isWalking);
        animator.SetBool("isRun", isRunning);
        animator.SetBool("isJump", isJumping);
    }

}
