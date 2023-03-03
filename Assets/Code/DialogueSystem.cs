using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public GameObject DialogueControl;
    public GameObject DialogueButton;

    private bool isNPC = false;
    private void Update()
    {
        if (isNPC == true && Input.GetKeyDown(KeyCode.E)) 
        { 
            DialogueControl.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogueButton.SetActive(true);
            isNPC= true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNPC = false;
            DialogueButton.SetActive(false);
        }
    }
}
