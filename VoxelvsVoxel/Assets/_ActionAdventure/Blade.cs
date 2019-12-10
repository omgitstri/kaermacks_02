using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    Collider weapon;
    Collider needle;

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<TempEnemy>();
        if (enemy != null)
        {
            enemy.IsHit();
            print("DIE MICKY DIE!!");
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
