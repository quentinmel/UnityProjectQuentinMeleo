using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBackToMenuScript : MonoBehaviour
{
    public void OnBackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
