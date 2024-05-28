using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CastleDoorScript : MonoBehaviour
{
    private bool playerInRange = false;
    private ParticleInteraction particleInteraction;
    [SerializeField] CanvasGroup openDoorUI;

    private void Start()
    {
        particleInteraction = FindObjectOfType<ParticleInteraction>();
        openDoorUI.alpha = 0;
        openDoorUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        bool playerHaveKey = particleInteraction.haveKey;
        if (playerInRange && playerHaveKey && Input.GetKeyDown(KeyCode.E))
        {
            OpenDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void OpenDoor()
    {
        var playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.DisableMovement();
        }

        StartCoroutine(FadeInOpenDoorUI());
    }

    private IEnumerator FadeInOpenDoorUI()
    {
        openDoorUI.gameObject.SetActive(true);
        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            openDoorUI.alpha = Mathf.Lerp(0, 1, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        openDoorUI.alpha = 1;
    }
}
