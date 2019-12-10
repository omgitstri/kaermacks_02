using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    [SerializeField]
    private Transform target = null;
    [SerializeField]
    private Vector3 offset = Vector3.zero;
    [SerializeField]
    private bool useDamping = false;
    [SerializeField]
    private float damping = 0f;

    void Update()
    {
        if (!useDamping)
        {
            gameObject.transform.position = target.transform.position + offset;
        }
        else
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, target.position + offset, damping * Time.deltaTime);
        }
    }
}