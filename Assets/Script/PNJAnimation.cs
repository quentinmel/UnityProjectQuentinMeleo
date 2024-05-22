using UnityEngine;
using TMPro;
using System.Collections;

public class PNJAnimation : MonoBehaviour
{
    public Animator animator;
    public TextMeshProUGUI dialogueText;
    public float wordsPerMinute = 500f;
    [TextArea(3, 10)]
    public string fullText;

    private void Start()
    {
        if (dialogueText != null)
        {
            dialogueText.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("playerClose");

            if (dialogueText != null)
            {
                dialogueText.enabled = true;
                StartCoroutine(ShowTextProgressively());
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.ResetTrigger("playerClose");

            if (dialogueText != null)
            {
                dialogueText.enabled = false;
            }
        }
    }

    IEnumerator ShowTextProgressively()
    {
        float timePerWord = 60f / wordsPerMinute;
        string displayedText = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            displayedText += fullText[i];
            dialogueText.text = displayedText;

            yield return new WaitForSeconds(timePerWord);
        }
    }
}
