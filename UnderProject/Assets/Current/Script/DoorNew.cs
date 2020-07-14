using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorNew : MonoBehaviour
{
    public bool active;
    public bool canActivate;

    public BombTrigger bomb;

    public SpriteRenderer currentColor;
    public Color[] infoColors;

    public BuildingNew building;

    void Start()
    {
        building = GameObject.FindGameObjectWithTag("Building").GetComponent<BuildingNew>();
        currentColor = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(transform.tag == "downDoor")
        {
            if(building.IsDone)
            {
                Transform grannieTransform = transform.parent.parent.parent;

                if(grannieTransform.Find("Bomb(Clone)"))
                {
                    bomb = grannieTransform.Find("Bomb(Clone)").GetComponent<BombTrigger>();
                    canActivate = true;
                }
            }

            if(active)
            {
                currentColor.color = infoColors[1];
            }
            else
            {
                currentColor.color = infoColors[0];
            }
        }

        if(canActivate)
        {
            if(bomb.activated)
            {
                active = true;
            }
            else
            {
                active = false;
            }
        }
        else
        {
            active = true;
        }
    }

    void OnTriggerStay2D(Collider2D thing)
    {
        if(thing.tag == "Player")
        {
            if(Input.GetButtonDown("Interact") && active)
            {
                building.GoToNextDoor(this.transform,thing.transform);
            }
        }
    }
}
