using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("Configuración del personaje")]
    public string characterName = "Gran Mago";

    [TextArea(2, 5)]
    public string[] dialogueLines;

    [Header("Referencias")]
    public DialogueManager dialogueManager;

    private bool playerNear = false;

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogueManager.IsDialogueActive())
            {
                dialogueManager.StartDialogue(characterName, dialogueLines);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}