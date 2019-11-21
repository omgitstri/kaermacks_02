using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    private Vector3 direction = Vector3.zero;

    void Update()
    {
        DoMovement();
    }

    public void DoMovement()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
}
