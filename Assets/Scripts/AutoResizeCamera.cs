using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoResizeCamera : MonoBehaviour, IResizeCamera
{
    private Camera thisCamera;
    private void Awake()
    {
        thisCamera = GetComponent<Camera>();
    }
    public void ResizeCamera(int x, int y)
    {
        float bounds = ((float)y / 2);

        thisCamera.orthographicSize = bounds + (bounds * 0.4f);
        thisCamera.transform.position = new Vector3(((float)x / 2) - 0.5f, ((float)-y / 2) + 0.5f, -10);
    }
}
