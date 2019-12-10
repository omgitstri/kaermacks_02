using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour
{
    Collider weapon;
    Collider needle;

    private void Awake()
    {
        weapon = GetComponent<Collider>();
        needle = GetComponent<Collider>();
    }



    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<TempEnemy>();
        if (enemy != null)
        {
            enemy.IsHit();
        }
    }

    public void EnableWeapon()
    {
        weapon.enabled = true;
    }

    public void DisableWeapon()
    {
        weapon.enabled = false;
    }
}
