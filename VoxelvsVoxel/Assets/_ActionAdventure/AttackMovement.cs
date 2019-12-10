using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;

    public void AttackForward()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
    }

    public void Dash()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);

        transform.Translate(moveDirection * Time.deltaTime * speed, Space.Self);
    }
}
