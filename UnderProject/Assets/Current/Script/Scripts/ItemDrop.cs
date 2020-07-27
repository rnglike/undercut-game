using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    // Item Lists, position 0 is the default (without item)
    public GameObject hair;
    public GameObject[] weaponList;
    public Transform dropperTransform;

    
    // Drop the item which is the player status
    public void Dropper(int itemCode, bool isHair)
    {
        if (isHair == true)
        {
            GameObject hairX = Instantiate(hair, dropperTransform.position, dropperTransform.rotation);
            hairX.transform.Find("peruca").GetComponent<LootBox>().item = itemCode;
        }
        else
        {
            Instantiate(weaponList[itemCode], dropperTransform.position, dropperTransform.rotation);
        }
    }

    
}
