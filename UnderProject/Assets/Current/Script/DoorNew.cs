using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorNew : MonoBehaviour
{
    public bool active;

    public BombTrigger bomb;

    public SpriteRenderer currentColor;
    public Color[] infoColors;

    public BuildingNew building;

    void Start()
    {
        building = GameObject.FindGameObjectWithTag("Building").GetComponent<BuildingNew>();
        currentColor = GetComponent<SpriteRenderer>();

        active = true;
    }

    void Update()
    {
        if(transform.tag == "downDoor")
        {
            if(building.IsDone)
            {
                Transform grannieTransform = transform.parent.parent.parent;

                if(grannieTransform.name != "LevelEnd(Clone)")
                {
                    if(grannieTransform.tag == "Base")
                    {
                        active = true;
                    }
                    else if(grannieTransform.Find("Bombs"))
                    {
                        active = CheckBomb(grannieTransform.Find("Bombs"));
                    }
                }
                else
                {
                    active = false;
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
    }

    bool CheckBomb(Transform thing)
    {
        foreach (Transform bomb in thing)
        {
            if(!bomb.GetComponent<BombTrigger>().activated) return false;
        }

        return true;
    }

    void OnTriggerStay2D(Collider2D thing)
    {
        if(thing.tag == "Player")
        {
            thing.GetComponent<PlayerController>().uuuuuuuuuuuuuuh = true;

            if(Input.GetButtonDown("Interact") && active)
            {
                building.GoToNextDoor(this.transform,thing.transform);
            }
        }
    }
}
