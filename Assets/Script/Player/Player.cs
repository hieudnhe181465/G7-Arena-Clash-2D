using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float doubleJumpForce = 8f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask enemyLayer;

    private bool isGrounded = true;
    private int jumpCount = 0;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;

    private int lPressCount = 0;
    private float lastLClickTime = 0f;
    public float comboWindow = 0.6f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded && rb.velocity.y <= 0)
        {
            jumpCount = 0;
        }

        HandleMovement();
        HandleJump();
        HandleCombat();
        HandleBlock();
    }

    private void HandleMovement()
    {
        float playerMoveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(playerMoveInput * moveSpeed, rb.velocity.y);

        if (playerMoveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (playerMoveInput > 0)
        {
            spriteRenderer.flipX = false;
        }

        animator.SetBool("IsRunning", playerMoveInput != 0);
    }

    private void HandleJump()
    {
        animator.SetBool("IsGrounded", isGrounded);

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                jumpCount = 1;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetTrigger("Jump");
            }
            else if (jumpCount == 1)
            {
                jumpCount = 2;
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                animator.SetTrigger("Jump");
            }
        }
    }

    private void HandleCombat()
    {
        if (Time.time - lastLClickTime > comboWindow)
        {
            lPressCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger("Attack");
            DetectEnemyHit();
            lPressCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Attack2");
            DetectEnemyHit();
            lPressCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            lPressCount++;
            lastLClickTime = Time.time;

            if (lPressCount == 3)
            {
                animator.SetTrigger("Combo");
                DetectEnemyHit();
                lPressCount = 0;
            }
        }
    }

    private void DetectEnemyHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            Vector2 direction = enemy.transform.position - transform.position;

            HpAndMpEnemy enemyHP = enemy.GetComponent<HpAndMpEnemy>();

            if (enemyHP != null)
            {
                if (!spriteRenderer.flipX && direction.x > 0)
                {
                    enemyHP.TakeDamage(10f); // trừ 10 máu
                    Debug.Log("Hit enemy: " + enemy.name);
                }

                if (spriteRenderer.flipX && direction.x < 0)
                {
                    enemyHP.TakeDamage(10f);
                    Debug.Log("Hit enemy: " + enemy.name);
                }
            }

            StunController stun = enemy.GetComponent<StunController>();
            if (stun != null)
            {
                stun.Stun();
            }
        }
    }

    private void HandleBlock()
    {
        bool blocking = Input.GetKey(KeyCode.I);
        animator.SetBool("Block", blocking);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}