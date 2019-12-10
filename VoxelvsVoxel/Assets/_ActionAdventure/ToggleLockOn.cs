using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLockOn : MonoBehaviour
{
    [SerializeField]
    private List<Transform> lTarget;
    private Transform player;
    private LookAt myLookAt;
    private int targetsIndex = 0;
    public RectTransform UI;
    private Transform[] temp;
    //
    Dictionary<GameObject,float> targetDistance;
    private Transform[] test;
    List<Transform> allTargets;
    List<float> allDis;
    List<int> order;

    private void Awake()
    {
        myLookAt = GetComponent<LookAt>();
        player = myLookAt?.CurrentTarget();
        UI.GetComponent<TargetUI>().target = lTarget[targetsIndex];

    }

    void Test()
    {
        temp = FindObjectsOfType<Transform>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].CompareTag("Enemy"))
            {
                Vector3 normalPos = transform.position - temp[i].position;
                float dis = (normalPos.x * normalPos.x) + (normalPos.y * normalPos.y);
                order.Add(order.Count + 1);
                allDis.Add(dis);
                allTargets.Add(temp[i]);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            NextTarget();
            UI.GetComponent<TargetUI>().target = lTarget[targetsIndex];
        }

        if (Input.GetMouseButton(1))
        {
            myLookAt.UpdateTarget(lTarget[targetsIndex]);
        }
        else
        {
            myLookAt.UpdateTarget(player);
        }
    }

    private void NextTarget()
    {
        if (targetsIndex < lTarget.Count - 1)
        {
            targetsIndex++;
        }
        else
        {
            targetsIndex = 0;
        }
    }

    [ContextMenu("Get Targets")]
    private void GetTargets()
    {
        lTarget.Clear();
        temp = FindObjectsOfType<Transform>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].CompareTag("Enemy"))
            {
                lTarget.Add(temp[i]);
            }
        }
    }


}
