using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int maxHealth = 5;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;






    void Start() {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        StartCoroutine(FlashRed());

        Debug.Log("Enemy HP: " + currentHealth);

        if (currentHealth <= 0) {
            Destroy(gameObject);
        }

        IEnumerator FlashRed() {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }
    }
}