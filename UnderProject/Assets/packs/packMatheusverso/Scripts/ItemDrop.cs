using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    // Item Lists, position 0 is the default (without item)
    public GameObject[] hairList;
    public GameObject[] weaponList;
    public Transform dropperTransform;

    
    // Drop the item which is the player status
    public void Dropper(int itemCode, bool isHair)
    {
        if (isHair == true)
        {
            Instantiate(hairList[itemCode], dropperTransform.position, dropperTransform.rotation);
        }
        else
        {
            Instantiate(weaponList[itemCode], dropperTransform.position, dropperTransform.rotation);
        }
    }

    
}
