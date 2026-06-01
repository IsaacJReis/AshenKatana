using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [Header("Vida")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Invencibilidade após dano")]
    public float invincibleDuration = 0.5f;
    private bool isInvincible = false;

    private SpriteRenderer spriteRenderer;

    void Start() {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage) {
        if (isInvincible)
            return;

        currentHealth -= damage;
        Debug.Log("Player HP: " + currentHealth);

        StartCoroutine(InvincibleRoutine());

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator InvincibleRoutine() {
        isInvincible = true;
        // Pisca o sprite pra indicar dano
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
        Debug.Log("Player morreu!");
        // Adicione aqui: tela de game over, respawn, etc.
    }
}