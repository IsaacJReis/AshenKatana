using System.Collections;
using System.Reflection;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    [Header("Vida")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Invencibilidade após dano")]
    public float invincibleDuration = 0.5f;
    private bool isInvincible = false;

    public bool playerDead = false;

    Animator animator;
    private int dyingHash = Animator.StringToHash("dying");
    private int movemntingHash = Animator.StringToHash("movementing");
    private int jumpingHash = Animator.StringToHash("jumping");

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage) {
        if (isInvincible || playerDead)
            return;

        currentHealth -= damage;
        Debug.Log("Player HP: " + currentHealth);

        StartCoroutine(InvincibleRoutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator InvincibleRoutine() {
        isInvincible = true;
        for (int i = 0; i < 4; i++)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.3f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        isInvincible = false;
    }

    void Die() {
        if (playerDead) return;
        playerDead = true;

        Debug.Log("Player morreu!");

      
        var movement = GetComponent<BasicMovimentPlayer>();
        if (movement != null) movement.enabled = false;

        var attack = GetComponent<PlayerAttack>();
        if (attack != null) attack.enabled = false;

   
        animator.SetBool(movemntingHash, false);
        animator.SetBool(jumpingHash, false);

        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;


        animator.SetBool(dyingHash, true);

        StartCoroutine(AposMorteRoutine());
    }

    IEnumerator AposMorteRoutine()
    {
        yield return null;
        float duracao = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duracao);
        //NÃO ESQUECER DA TELA DE GAME OVER AQUI!!!
        Destroy(gameObject);
    }
}