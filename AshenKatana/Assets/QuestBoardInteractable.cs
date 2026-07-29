using UnityEngine;

public class QuestBoardInteractable : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private GameObject promptUI;   // opcional: texto "Pressione E" que aparece no range
    [SerializeField] private QuestPanelUI questPanel;

    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            questPanel.Open();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        if (promptUI != null) promptUI.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        if (promptUI != null) promptUI.SetActive(false);

        // Fecha o painel automaticamente se o player sair do range com ele aberto
        if (questPanel != null && questPanel.IsOpen)
            questPanel.Close();
    }
}