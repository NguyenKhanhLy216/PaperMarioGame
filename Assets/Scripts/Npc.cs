using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
public AudioSource textsound;
    public GameObject dialogueContainer;
    [TextArea(3,10)] 
    public string[] dialogueLines;
    public float dialogueDuration = 3f;
    private int currentDialogueIndex = 0;
    public string presskey = ""; 
    private bool isPlayerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Da toi npc");
            isPlayerInRange = true;
            //dialogueContainer.SetActive(true); 
            dialogueContainer.GetComponent<TextMesh>().text = dialogueLines[0]; 
            currentDialogueIndex++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isPlayerInRange = false;
            Debug.Log("Ngoai tam npc");
            //dialogueContainer.SetActive(false); 
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.X))
        {
            textsound.Play();
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        dialogueContainer.GetComponent<TextMesh>().text = dialogueLines[currentDialogueIndex]; 

        currentDialogueIndex++; 

        if (currentDialogueIndex >= dialogueLines.Length) 
        {
            currentDialogueIndex = 0; 
        }
    }
}
