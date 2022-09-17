using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InstantiateCells))]
public class InputManager : MonoBehaviour
{
    private bool continousNextGeneration = false;

    private IInput Iinput;
    private void Awake()
    {
        Iinput = GetComponent<IInput>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Iinput.OnNextGeneration();
        }
        if (Input.GetKeyDown(KeyCode.P))
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
        if (!continousNextGeneration && Input.GetMouseButtonDown(0))
        {
            Iinput.OnSwitchState();
        }
    }

    IEnumerator Play()
    {
        Iinput.OnNextGeneration();
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Play());
    }
}
