using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;


public class NPC : MonoBehaviour, IInteractable {
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public Image portraitImage;
    public TMP_Text nameText;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private bool playerIsNear;
    public GameObject pressEText;

    void Update() {

        if (playerIsNear && Input.GetKeyDown(KeyCode.Escape))
        {
            EndDialogue();
        }
        // Se o player est� perto e apertou E
        if (playerIsNear && Input.GetKeyDown(KeyCode.E)) {

            // Se o di�logo N�O est� ativo, come�a ele
            if (!isDialogueActive) {
                StartDialogue();
            }
            // Se J� EST� ativo e J� terminou de digitar, vai para a pr�xima
            else if (!isTyping) {
                NextLine();
            }
            // Se est� digitando, pula a anima��o 
            else {
                CompletarLinhaImediatamente();
            }
        }
    }

    public void Interact() {
        // Esta fun��o agora � chamada pelo Update acima
    }

    void StartDialogue() {
        if (dialogueData == null) return;

        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;
        dialoguePanel.SetActive(true);

        // Se o seu PauseControler trava o jogo, garanta que ele n�o trave o script
        PauseControler.SetPause(true);

        StartCoroutine(TypeLine());
    }

    void NextLine() {
        dialogueIndex++;
        if (dialogueIndex < dialogueData.dialoguesLines.Length) {
            StartCoroutine(TypeLine());
        } else {
            EndDialogue();
        }
    }

    IEnumerator TypeLine() {
        isTyping = true;
        dialogueText.SetText("");

        // Usamos WaitForSecondsRealtime para ignorar o Pause do jogo
        foreach (char letter in dialogueData.dialoguesLines[dialogueIndex]) {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(dialogueData.typingSpeed);
        }

        isTyping = false;
    }

    void CompletarLinhaImediatamente() {
        StopAllCoroutines();
        dialogueText.SetText(dialogueData.dialoguesLines[dialogueIndex]);
        isTyping = false;
    }

    public void EndDialogue() {
        StopAllCoroutines();
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        PauseControler.SetPause(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) playerIsNear = true;
        playerIsNear = true;
        pressEText.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerIsNear = false;
            pressEText.SetActive(false);
            EndDialogue();
        }
    }

    public bool CanInteract() => !isDialogueActive;
}