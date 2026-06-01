using UnityEngine;

public class AttackHitbox : MonoBehaviour {
    private BasicMovimentPlayer player;
    public int damage = 20;

    void Awake() {
        // Busca o player no pai (a hitbox é filha do player)
        player = GetComponentInParent<BasicMovimentPlayer>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Dummy"))
        {
            other.GetComponent<DummyHealth>().TakeDamage(damage);
        }

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            if (player != null)
                player.currentTarget = other.transform;
            Debug.Log("ACERTEI INIMIGO: " + other.name);
        }
    }
}