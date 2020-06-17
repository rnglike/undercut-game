using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNew : MonoBehaviour
{
    public Room baseLevel;

    public GameObject level;
    public GameObject levelEnd;
    public Transform[] allDoors;

    public int levelAmount;
    public int levelToMake;
    public int floorMade;

    public Texture2D[] maps;

    public bool IsDone;
    public bool makeLevels;
    public bool HaveDoors;
    
    public int currentRoom;
    public bool dooring;

    void Start()
    {
        baseLevel = transform.GetChild(0).GetComponent<Room>();

        levelToMake = levelAmount;
    }

    void Update()
    {
        IsDone = CheckIsDone();

        if(makeLevels)
        {
            makeLevels = false;
            baseLevel.MakeLevels();
        }

        if((floorMade == levelToMake) && !HaveDoors)
        {
            allDoors = GetAllDoors();
            HaveDoors = true;
        }
    }

    public void GoToNextDoor(Transform currentDoor,Transform thing)
    {
        if(!dooring)
        {
            dooring = true;

            Transform nextDoor = currentDoor;

            if(currentDoor.tag == "upDoor")
            {
                currentRoom--;
                nextDoor = allDoors[currentRoom].Find("DoorDown(Clone)");
            }

            if(currentDoor.tag == "downDoor")
            {
                currentRoom++;
                nextDoor = allDoors[currentRoom].Find("DoorUp(Clone)");
            }

            thing.position = nextDoor.transform.Find("Door").position;

            StartCoroutine("WaitFor5s");
        }
    }

    IEnumerator WaitFor5s()
    {
        yield return new WaitForSeconds(.5f);
        dooring = false;
    }

    Transform[] GetAllDoors()
    {
        Debug.Log("kkk");
        Transform[] doorArray = new Transform[levelAmount + 1];

        foreach(Transform level in this.transform)
        {
            Transform thisDoor = level.Find("Doors");
            doorArray[level.GetSiblingIndex()] = thisDoor;
        }

        return doorArray;
    }

    public bool CheckIsDone()
    {
        if(floorMade < (levelToMake - 1))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void IncrementFloorMade(){floorMade++;}

}