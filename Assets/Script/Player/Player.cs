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

    // Hieu
    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask enemyLayer;

    private bool isGrounded = true;
    private int jumpCount = 0;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    void Start() { }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded && rb.velocity.y <= 0) jumpCount = 0;

        HandleMovement();
        HandleJump();
        HandleCombat();
    }

    private void HandleJump()
    {
        animator.SetBool("IsGrounded", isGrounded);

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded) // Nhảy lần 1
            {
                jumpCount = 1;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetTrigger("Jump");
            }
            else if (jumpCount == 1) // Nhảy lần 2 (Double Jump)
            {
                jumpCount = 2;
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce); // Nhảy cao hơn
                animator.SetTrigger("Jump");
            }
        }
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
        if (playerMoveInput != 0)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        animator.SetBool("IsRunning", playerMoveInput != 0);
    }

    //private void HandleCombat()
    //{
    //    if (Input.GetKeyDown(KeyCode.Z))
    //    {
    //        animator.SetTrigger("Attack");
    //    }
    //}

    private void HandleCombat()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("Attack");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
                attackPoint.position,
                attackRange,
                enemyLayer
            );

            foreach (Collider2D enemy in hitEnemies)
            {
                Vector2 direction = enemy.transform.position - transform.position;
                StunController stun = enemy.GetComponent<StunController>();
                animator.SetTrigger("Stun");
                if (stun != null)
                {
                    stun.Stun();
                }
                if (!spriteRenderer.flipX && direction.x > 0)
                {
                    Debug.Log("Đấm trúng enemy phía trước: " + enemy.name);
                }

                if (spriteRenderer.flipX && direction.x < 0)
                {
                    Debug.Log("Đấm trúng enemy phía trước: " + enemy.name);
                }
            }
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    if (attackPoint == null) return;

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    //}
}
