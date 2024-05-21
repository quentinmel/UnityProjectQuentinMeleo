using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragDrop : MonoBehaviour
{
    public GameObject objectToDrag;
    public GameObject objectDragToPos;

    public float dropDistance = 30f;
    private int objectsInPlace = 0;

    public bool isLocked;

    Vector2 objectInitPosition;

    void Start()
    {
        objectInitPosition = objectToDrag.transform.position;
    }

    private void Update()
    {
        if (objectsInPlace == 3)
        {
            LoadNextScene();
        }
    }

    public void DragObject()
    {
        if (!isLocked)
        {
            objectToDrag.transform.position = Input.mousePosition;
            CheckObjectPlacement();
        }
    }

    public void DropObject()
    {
        float Distance = Vector3.Distance(objectToDrag.transform.position, objectDragToPos.transform.position);
        if (Distance < dropDistance)
        {
            isLocked = true;
            objectToDrag.transform.position = objectDragToPos.transform.position;
            objectsInPlace += 1;
        }
        else
        {
            objectToDrag.transform.position = objectInitPosition;
        }
    }

    void CheckObjectPlacement()
    {
        float Distance = Vector3.Distance(objectToDrag.transform.position, objectDragToPos.transform.position);
        if (Distance < dropDistance && !isLocked)
        {
            objectsInPlace = 0;
            foreach (DragDrop obj in FindObjectsOfType<DragDrop>())
            {
                if (obj.isLocked)
                {
                    objectsInPlace++;
                }
            }
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Level3");
    }

}
