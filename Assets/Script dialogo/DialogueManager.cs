using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Panel de diálogo")]
    public GameObject dialoguePanel;

    [Header("Textos")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private string[] dialogueLines;
    private int currentLine = 0;
    private bool dialogueActive = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (dialogueActive && Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }

    public void StartDialogue(string characterName, string[] lines)
    {
        dialogueLines = lines;
        currentLine = 0;
        dialogueActive = true;

        dialoguePanel.SetActive(true);
        nameText.text = characterName;
        dialogueText.text = dialogueLines[currentLine];
    }

    void NextLine()
    {
        currentLine++;

        if (currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);
    }

    public bool IsDialogueActive()
    {
        return dialogueActive;
    }
}
