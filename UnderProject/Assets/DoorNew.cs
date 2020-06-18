using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorNew : MonoBehaviour
{
    public BuildingNew building;

    void Start()
    {
        building = GameObject.FindGameObjectWithTag("Building").GetComponent<BuildingNew>();
    }

    void OnTriggerStay2D(Collider2D thing)
    {
        if(thing.tag == "Player")
        {
            if(Input.GetButtonDown("Interact"))
            {
                building.GoToNextDoor(this.transform,thing.transform);
            }
        }
    }
}
