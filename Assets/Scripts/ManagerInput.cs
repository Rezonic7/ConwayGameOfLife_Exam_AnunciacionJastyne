using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInput : MonoBehaviour
{
    private IInteractable iInteractable;

    private IHoverable currentlyHovering = null;

    [SerializeField] private KeyCode NextButtonOnce = KeyCode.Space;
    [SerializeField] private KeyCode ContinousButton = KeyCode.P;
    [SerializeField] private KeyCode RestartButton = KeyCode.R;

    private void Awake()
    {
        foreach (GameObject gameObjects in FindObjectsOfType<GameObject>())
        {
            if (gameObjects.GetComponent<IInteractable>() != null)
                iInteractable = gameObjects.GetComponent<IInteractable>();
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
            iInteractable.Interaction("Interact1");
        }
        if (Input.GetKeyDown(ContinousButton))
        {
            iInteractable.Interaction("Interact2");
        }
        if (Input.GetKeyDown(RestartButton))
        {
            iInteractable.Interaction("Interact3");
        }
    }
    private void MouseInputs()
    {
        if (GetNearestGameObject() != null)
        {
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

            if (Input.GetMouseButtonDown(0))
            {
                var clickable = GetNearestGameObject().GetComponent<IClickable>();
                if (clickable != null)
                {
                    clickable.Click();
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
}
