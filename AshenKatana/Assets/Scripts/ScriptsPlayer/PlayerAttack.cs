using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    public GameObject attackHitbox;
    public float attackDuration = 0.2f;
    public int damage;
    public Transform currentTarget;
    public float followSpeed = 5f;
    public float stopDistance = 1.5f;






    public void ActivateHitbox() {
        attackHitbox.SetActive(true);
        Invoke("DeactivateHitbox", attackDuration);
    }

    void DeactivateHitbox() {
        attackHitbox.SetActive(false);
    }

    private void Update() {

    }
    public void Attack() {
        Debug.Log("ATAQUE DISPARADO");
    }   
}