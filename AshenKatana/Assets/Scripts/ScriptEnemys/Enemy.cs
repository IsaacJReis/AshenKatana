using System.Collections;
using UnityEngine;
using Cinemachine;

public class Enemy : MonoBehaviour {
    [Header("Vida")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Perseguição")]
    public float moveSpeed = 2f;
    public float chaseRange = 8f;

    [Header("Ataque")]
    public int attackDamage = 10;
    public float attackRange = 0.8f;
    public float attackCooldown = 1.2f;

    [Header("Hit Stun")]
    public float hitStunDuration = 0.4f;

    [Header("Morte")]
    public float deathAnimationDuration = 1f;

    [Header("Configuração do Sprite")]
    public bool spriteDefaultFacingLeft = true;

    // --- Referências ---
    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private CinemachineImpulseSource impulseSource;

    // --- Estado interno ---
    private bool isChasing = false;
    private bool isStunned = false;
    private bool isDead = false;
    private float attackTimer = 0f;
    private bool isInAttackCooldown = false;

    void Start() {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update() {
        if (player == null || isStunned || isDead)
            return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= chaseRange)
            isChasing = true;

        if (dist > chaseRange)
        {
            isChasing = false;
            rb.velocity = new Vector2(0f, rb.velocity.y);
            SetAnimatorState(false, false);
            return;
        }

        // Flip
        bool playerIsToTheRight = player.position.x > transform.position.x;
        float absScale = Mathf.Abs(transform.localScale.x);
        if (spriteDefaultFacingLeft)
            transform.localScale = new Vector3(playerIsToTheRight ? -absScale : absScale, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(playerIsToTheRight ? absScale : -absScale, transform.localScale.y, transform.localScale.z);

        if (dist <= attackRange)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);

            if (!isInAttackCooldown)
                StartCoroutine(AttackOnce());
        }
        else
        {
            // Andando — garante que isAttacking false
            SetAnimatorState(true, false);
            float dir = playerIsToTheRight ? 1f : -1f;
            rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);
        }
    }

    IEnumerator AttackOnce() {
        isInAttackCooldown = true;

        // Inicia animação de ataque
        SetAnimatorState(false, true);

        // Aplica dano
        PlayerHealth ph = player != null ? player.GetComponent<PlayerHealth>() : null;
        if (ph != null)
            ph.TakeDamage(attackDamage);
        Debug.Log("Inimigo atacou!");

        // Espera a duração do ataque (animação tocando)
        yield return new WaitForSeconds(0.3f);

        // Para animação de ataque
        SetAnimatorState(false, false);

        // Espera o cooldown restante antes de poder atacar de novo
        yield return new WaitForSeconds(attackCooldown - 0.3f);

        isInAttackCooldown = false;
    }

    void SetAnimatorState(bool walking, bool attacking) {
        if (animator == null)
            return;
        animator.SetBool("isWalking", walking);
        animator.SetBool("isAttacking", attacking);
    }

    public void TakeDamage(int damage) {
        if (isDead)
            return;

        if (impulseSource != null)
            CameraShakeManager.instance.CameraShake(impulseSource);

        currentHealth -= damage;
        isChasing = true;
        Debug.Log("Enemy HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(HitStunRoutine());
    }

    IEnumerator HitStunRoutine() {
        isInAttackCooldown = false;
        isStunned = true;
        rb.velocity = Vector2.zero;
        spriteRenderer.color = Color.red;
        SetAnimatorState(false, false);

        yield return new WaitForSeconds(hitStunDuration);

        spriteRenderer.color = Color.white;
        isStunned = false;
    }

    void Die() {
        isDead = true;
        spriteRenderer.color = Color.white;

        StopAllCoroutines();
        SetAnimatorState(false, false);

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 0f;
        rb.isKinematic = true;

        // Desativa o collider pra não ser empurrado
        GetComponent<Collider2D>().enabled = false;

        if (animator != null)
            animator.SetBool("isDead", true);

        Debug.Log("Inimigo morreu!");
        Destroy(gameObject, deathAnimationDuration);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}