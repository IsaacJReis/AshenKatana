using UnityEngine;
using System.Collections;

public class SignInteract : MonoBehaviour {
    public GameObject pressEText;
    public CanvasGroup cityNameCanvasGroup;

    private bool playerInRange = false;
    private bool isShowing = false;

    void Update() {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isShowing) {
            StartCoroutine(ShowCityName());
        }
    }

    IEnumerator ShowCityName() {
      
        
        isShowing = true;
        Debug.Log("Coroutine começou");
        float duration = 1f;
        float timer = 0;

        // Fade In
        while (timer < duration) {
            timer += Time.deltaTime;
            cityNameCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / duration);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        // Fade Out
        timer = 0;
        while (timer < duration) {
            timer += Time.deltaTime;
            cityNameCanvasGroup.alpha = Mathf.Lerp(1, 0, timer / duration);
            yield return null;
        }

        isShowing = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;
            pressEText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInRange = false;
            pressEText.SetActive(false);
        }
    }
}
