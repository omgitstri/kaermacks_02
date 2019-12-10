using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHeight : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float minHeight = 0f;
    [SerializeField]
    private float maxHeight = 4f;
    private float currentHeight = 0f;

    void Start()
    {
        currentHeight = minHeight;
    }

    void Update()
    {
        currentHeight = Mathf.Clamp(currentHeight + Input.GetAxis("Mouse Y") * speed * Time.deltaTime, minHeight, maxHeight);
        transform.localPosition = new Vector3(0, currentHeight, 0);
    }
}
