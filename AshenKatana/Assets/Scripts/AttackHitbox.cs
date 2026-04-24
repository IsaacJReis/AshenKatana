using UnityEngine;

public class AttackHitbox : MonoBehaviour {

    private BasicMovimentPlayer player;
    public int damage = 1;
    public
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Dummy")) {
            other.GetComponent<DummyHealth>().TakeDamage(damage);
        }

        if (other.CompareTag("Enemy")) {
            other.GetComponent<Enemy>().TakeDamage(damage);
            player.currentTarget = other.transform;
            Debug.Log("ACERTEI INIMIGO: " + other.name);
        }

        void Start() {
            player = GetComponentInParent<BasicMovimentPlayer>();

            if (player == null) {
                Debug.LogError("PLAYER NÃO ENCONTRADO!");
            }
        }
    }

}


