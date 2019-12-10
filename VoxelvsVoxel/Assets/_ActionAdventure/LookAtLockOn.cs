using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtLockOn : MonoBehaviour
{
    [SerializeField] private MonoBehaviour rotateScript;
    [SerializeField] private Transform target;
    [SerializeField] Transform leftTarget;
    [SerializeField] Transform rightTarget;
    [SerializeField] float damping = 2f;
    [SerializeField] float distance = 5f;
    private bool lockOn;
    private bool getTarget = true;
    private TargetUI targetUI;

    [SerializeField] private List<Transform> targetList;

    private void Awake()
    {
        targetUI = FindObjectOfType<TargetUI>();
        if (lockOn)
        {
            targetUI.gameObject.SetActive(false);
        }
        else
        {
            targetUI.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (targetList.Count == 0)
        {
            ResetTargets();
        }

        if (lockOn)
        {
            LookAtTarget();
            targetUI.target = target;
        }
        else
        {
            targetUI.gameObject.SetActive(false);
            StopLookAtTarget();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (target != null) //if we have targets
            {
                if (lockOn) //and lockOn is already true
                {
                    targetUI.gameObject.SetActive(false);
                    lockOn = false;
                    getTarget = true;
                }
                else //if lockOn is false
                {
                    targetUI.gameObject.SetActive(true);
                    lockOn = true;
                    getTarget = false;
                }
            }
        }

        GetTargets();
        if (getTarget)
        {
            GetCenteredTarget();
        }

        ChangeTarget();
    }

    void ChangeTarget()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            if (rightTarget != null)
            {
                target = rightTarget;
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if (leftTarget != null)
            {
                target = leftTarget;
            }
        }
        //add sound for changing target
    }

    public void ResetTargets()
    {
        lockOn = false;
        getTarget = true;
        target = null;
        leftTarget = null;
        rightTarget = null;
    }


    void GetTargets()
    {
        targetList.Clear();
        Transform[] temp = FindObjectsOfType<Transform>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].CompareTag("Enemy") && Vector3.Distance(transform.position, temp[i].position) < distance)
            {
                targetList.Add(temp[i]);
            }
        }
    }

    void GetCenteredTarget()
    {
        float rightAngle = 360;
        float leftAngle = 660;

        for (int i = 0; i < targetList.Count; i++)
        {
            if ((UpdateHeight(targetList[i].position) - transform.forward).x > 0)
            {
                if (rightAngle > Vector3.Angle(transform.forward, UpdateHeight(targetList[i].position)))
                {
                    rightAngle = Vector3.Angle(transform.forward, UpdateHeight(targetList[i].position));
                    rightTarget = targetList[i];
                }
            }
            else
            {
                if (leftAngle > Vector3.Angle(transform.forward, UpdateHeight(targetList[i].position)))
                {
                    leftAngle = Vector3.Angle(transform.forward, UpdateHeight(targetList[i].position));
                    leftTarget = targetList[i];
                }
            }
        }

        if (leftAngle > rightAngle)
        {
            target = rightTarget;
        }
        else
        {
            target = leftTarget;
        }
    }

    void LookAtTarget()
    {
        rotateScript.enabled = false;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(UpdateHeight(target.position) - transform.position), 1f);
    }

    void StopLookAtTarget()
    {
        rotateScript.enabled = true;
    }


    Vector3 UpdateHeight(Vector3 currentPos)
    {
        Vector3 newPos = new Vector3(currentPos.x, transform.position.y, currentPos.z);
        return newPos;
    }


}
