using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHorizontalAxis : MonoBehaviour
{
    [SerializeField] private bool invertX = false;

    void Update()
    {
        var mouseY = Input.GetAxis("Mouse X");
        var rot = transform.rotation;

        rot.y += mouseY * Time.deltaTime;
        transform.rotation = rot;
    }
}
