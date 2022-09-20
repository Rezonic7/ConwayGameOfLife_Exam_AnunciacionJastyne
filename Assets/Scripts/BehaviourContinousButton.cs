using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BehaviourContinousButton : MonoBehaviour
{
    [SerializeField] private Image buttonIcon;
    [SerializeField] private Sprite playIcon;
    [SerializeField] private Sprite pauseIcon;
    private IManagerCellMatrix iManagerCellMatrix;
    private void Awake()
    {
        foreach (GameObject gameObjects in FindObjectsOfType<GameObject>())
        {
            if (gameObjects.GetComponent<IManagerCellMatrix>() != null)
                iManagerCellMatrix = gameObjects.GetComponent<IManagerCellMatrix>();
        }
    }
    void Start()
    {
        buttonIcon.sprite = playIcon;
    }
    public void SwitchIcon()
    {
        if (iManagerCellMatrix.IsContinousGeneration())
        {
            buttonIcon.sprite = pauseIcon;
        }
        else
        {
            buttonIcon.sprite = playIcon;
        }
    }
}
