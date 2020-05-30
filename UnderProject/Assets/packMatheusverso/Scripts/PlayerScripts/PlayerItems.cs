using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    // Change to private this entire "block", after test
    public int hairCode;
    public int weaponCode;
    public int howManyHairs = 10;
    public int howManyWeapons = 10;

    // Item drop game object
    public GameObject dropper;


    void Update()
    {
        // Drop the items
        if (Input.GetKey("n") && weaponCode > 0)
        {
            dropper.GetComponent<ItemDrop>().Dropper(weaponCode, false);
            SetWeaponCode(0);
        }
        if (Input.GetKey("b") && hairCode > 0)
        {
            dropper.GetComponent<ItemDrop>().Dropper(hairCode, true);
            SetHairCode(0);
        }
    }


    // Rules to set the hair
    public void SetHairCode(int newHair)
    {
        if (newHair >= 0 && newHair < howManyHairs)
        {
            hairCode = newHair;
        }
    }

    public int GetHairCode()
    {
        return hairCode;
    }


    // Rules to set the weapon
    public void SetWeaponCode(int newWeapon)
    {
        if (newWeapon >= 0 && newWeapon < howManyWeapons)
        {
            weaponCode = newWeapon;
        }
    }

    public int GetWeaponCode()
    {
        return weaponCode;
    }
}
