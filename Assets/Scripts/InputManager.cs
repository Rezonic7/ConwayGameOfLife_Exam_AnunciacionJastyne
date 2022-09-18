using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool continousNextGeneration = false;

    private IInputReciever iInputReciever;

    private IHoverable currentlyHovering = null;

    [SerializeField] private KeyCode NextButtonOnce = KeyCode.Space;
    [SerializeField] private KeyCode ContiniusButton = KeyCode.P;


    private void Awake()
    {
        foreach (GameObject gameObjects in FindObjectsOfType<GameObject>())
        {
            if (gameObjects.GetComponent<IInputReciever>() != null)
                iInputReciever = gameObjects.GetComponent<IInputReciever>();
        }
    }
    void Update()
    {
        KeyboardInputs();

        MouseInputs();
    }

    private void KeyboardInputs()
    {
        if (Input.GetKeyDown(NextButtonOnce))
        {
            iInputReciever.InputRecieved();
        }
        if (Input.GetKeyDown(ContiniusButton))
        {
            if (continousNextGeneration)
            {
                StopAllCoroutines();
                continousNextGeneration = false;
            }
            else
            {
                StartCoroutine(Play());
                continousNextGeneration = true;
            }
        }
    }
    private void MouseInputs()
    {
        if (GetNearestGameObject() != null)
        {
            if (!continousNextGeneration && Input.GetMouseButtonDown(0))
            {
                var clickable = GetNearestGameObject().GetComponent<IClickable>();
                if (clickable != null)
                {
                    clickable.Click();
                }
            }

            var hoverable = GetNearestGameObject().GetComponent<IHoverable>();

            if (hoverable != null)
            {
                if (currentlyHovering == null)
                {
                    currentlyHovering = hoverable;
                    currentlyHovering.Hover();
                }

                if (currentlyHovering != hoverable)
                {
                    currentlyHovering.Unhover();
                    currentlyHovering = hoverable;
                    currentlyHovering.Hover();
                }
            }
        }
    }

    public GameObject GetNearestGameObject()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        foreach (GameObject gameObjects in FindObjectsOfType<GameObject>())
        {
            if (Vector2.Distance(gameObjects.transform.position, mousePos) <= 0.5f)
            {
                return gameObjects;
            }
        }
        
        return null;
    }


    IEnumerator Play()
    {
        iInputReciever.InputRecieved();
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Play());
    }
}
