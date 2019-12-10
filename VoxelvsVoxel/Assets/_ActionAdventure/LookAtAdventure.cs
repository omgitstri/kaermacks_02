using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private bool useLateUpdate = false;
    [SerializeField]
    private bool useDamping = false;
    [SerializeField]
    private float damping = 0f;

    private Quaternion desiredRotation;

    private void Update()
    {
        if (!useLateUpdate)
        {
            UpdateLookAt();
        }
    }

    private void LateUpdate()
    {
        if (useLateUpdate)
        {
            UpdateLookAt();
        }
    }

    private void UpdateLookAt()
    {

        if (target != null)
        {
            desiredRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
            if (!useDamping)
            {
                transform.rotation = desiredRotation;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, damping * Time.deltaTime);
            }
        }
        else
        {
            desiredRotation = Quaternion.identity;
            if (!useDamping)
            {
                transform.localRotation = desiredRotation;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.localRotation, desiredRotation, damping * Time.deltaTime);
            }
        }
    }

    public void UpdateTarget(Transform pTarget)
    {
        target = pTarget;
    }

    public Transform CurrentTarget()
    {
        return target;
    }
}
