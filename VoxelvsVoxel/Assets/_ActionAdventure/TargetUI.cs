using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetUI : MonoBehaviour
{
    public Transform target;
    [SerializeField]
    private float minSize;

    void Update()
    {
        if (target != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.position);
            float size = Vector3.Distance(Camera.main.WorldToScreenPoint(target.GetComponent<Collider>().bounds.min), Camera.main.WorldToScreenPoint(target.GetComponent<Collider>().bounds.max));
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Max(size, 50f), Mathf.Max(size, 50f));
        }
    }
}
