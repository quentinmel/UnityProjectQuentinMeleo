using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleInteraction : MonoBehaviour
{
    public bool haveKey = false;
    private bool playerInRange = false;
    private bool doorOpen = false;

    public Image keyImage;
    public new ParticleSystem particleSystem;

    private void Start()
    {
        if (keyImage != null)
        {
            keyImage.enabled = false;
        }

        if (particleSystem != null)
        {
            var emission = particleSystem.emission;
            emission.enabled = true;
        }
    }

    private void Update()
    {
        if (playerInRange && !doorOpen && Input.GetKeyDown(KeyCode.E))
        {
            TakeKey();
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

    private void TakeKey()
    {
        doorOpen = true;
        haveKey = true;

        if (keyImage != null)
        {
            keyImage.enabled = true;
        }

        if (particleSystem != null)
        {
            var emission = particleSystem.emission;
            emission.enabled = false;
            particleSystem.Play(); 
        }
    }
}
