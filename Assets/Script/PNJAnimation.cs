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
    private bool firstTime = true;

    private void Start()
    {
        if (dialogueText != null)
        {
            dialogueText.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (firstTime)
        {
            if (other.CompareTag("Player"))
            {
                animator.SetTrigger("playerClose");
                firstTime = false;

                if (dialogueText != null)
                {
                    dialogueText.enabled = true;
                    StartCoroutine(ShowTextProgressively());
                }
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
