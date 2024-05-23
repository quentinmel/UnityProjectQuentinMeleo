using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CastleDoorScript : MonoBehaviour
{
    private bool playerInRange = false;
    private ParticleInteraction particleInteraction;

    private void Start()
    {
        particleInteraction = FindObjectOfType<ParticleInteraction>();
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
        SceneManager.LoadScene("MainMenu");
    }
}
